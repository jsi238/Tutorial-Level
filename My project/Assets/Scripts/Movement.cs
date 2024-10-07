using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private static int sceneNum = 0; //keep track of which scene the player is in

    private Animator animator;

    [SerializeField] private AudioSource pigSound;
    [SerializeField] private AudioSource cowSound;
    [SerializeField] private AudioSource horseSound;
    [SerializeField] private AudioSource stepSound;
    [SerializeField] private AudioSource swingAxeSound;
    [SerializeField] private AudioSource destroyRockSound;
    [SerializeField] private AudioSource pickUpSound;
    [SerializeField] private AudioSource weakAxeSound;
    [SerializeField] private AudioSource doorCloseSound;

    [SerializeField] private Image goldPick;
    [SerializeField] private Image bluePick;
    //[SerializeField] private TextMesh gameOverText;

    private SpriteRenderer srBarn;
    [SerializeField] private Sprite closedBarn;

    private readonly float speed = 7; //create variable to set player speed
    private Vector2 movement; // vector for movement
    private Rigidbody2D rb;
    private bool facingRight = true;

    private GameObject rock; //rock object to be removed when crushed

    private bool isTouchingRock = false; //check if player is a touching rock

    private bool hasGoldPick = false; //checks if player has collected gold pickaxe upgrade
    private bool hasDiamondPick = false; //checks if player has collected diamond pickaxe upgrade

    private bool moving = false;

    private bool isFull = false; //checks if all animals has been collected.

    private int[] numAnimals = { 3, 10 }; //hard-coded number of animals in each level
    private int collectedAnimals = 0;

    // these 3 variables are used to control the pickaxe animation and movement delay
    private bool swingingPick = false;
    private double swingingTime = 0;
    private bool endOfSwingAnimation = false;
    private double PICKAXE_ANIMATION_TIME = 2; // in seconds

    private double NUM_LEVELS = 2; //numbers of levels in the game

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(numAnimals[sceneNum]);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        srBarn = GameObject.Find("Barn").GetComponent<SpriteRenderer>();

        InvokeRepeating("playStepSound", .001f, .3f);
        InvokeRepeating("playAxeSwing", .001f, 0);
    }

    void Update()
    {
        /*maybe less typing to just do it this way */
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveRight = Input.GetKey(KeyCode.D);

        if (collectedAnimals >= numAnimals[sceneNum])
        {
            srBarn.sprite = closedBarn;
            doorCloseSound.volume = 1;
            doorCloseSound.Play();
            collectedAnimals = 0;
            isFull = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !swingingPick)
        {
            swingingPick = true;
            playAxeSwing();
        }

        if (swingingPick)
        {
            swingingTime += Time.fixedDeltaTime;
            Debug.Log(swingingTime);
            if (swingingTime > PICKAXE_ANIMATION_TIME)
            {
                swingingTime = 0;
                swingingPick = false;
                endOfSwingAnimation = true;
            }
        }

        /* check which key is pressed and if movement is impeded in that directions */
        if (moveUp) movement.y = 1;
        if (moveDown) movement.y = -1;
        if (moveLeft)
        {
            movement.x = -1;
            facingRight = false;
        }
        if (moveRight)
        {
            movement.x = 1;
            facingRight = true;
        }

        if (!moveUp && !moveDown) movement.y = 0;
        if (!moveLeft && !moveRight) movement.x = 0;

        movement.Normalize();

        // can't move if swinging pickaxe
        if (!swingingPick)
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        if (isTouchingRock && endOfSwingAnimation)
        {
            if (rock.tag == "rock")
            {
                destroyRockSound.Play();
                rock.SetActive(false); //'destory' rock in players way
            }
            else if (rock.tag == "gold rock" && hasGoldPick == true)
            {
                destroyRockSound.Play();
                rock.SetActive(false);
            }
            else if (rock.tag == "strong rock" && hasDiamondPick == true)
            {
                rock.SetActive(false);
                destroyRockSound.Play();
            }
            else
            {
                weakAxeSound.Play();
            }
        }

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
        }
        moving = moveRight || moveLeft || moveUp || moveDown;

        if (moving)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (swingingPick)
        {
            animator.SetBool("swingingPickaxe", true);
        }
        else
        {
            animator.SetBool("swingingPickaxe", false);
        }

        Vector3 theScale = transform.localScale;
        if (facingRight)
        {
            if (theScale.x < 0) theScale.x *= -1;
            transform.localScale = theScale;
        }
        else
        {
            if (theScale.x > 0) theScale.x *= -1;
            transform.localScale = theScale;
        }

        endOfSwingAnimation = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "barn" && isFull == true)
        {
            sceneNum++;
            if (sceneNum == NUM_LEVELS)
            {
                //gameOverText.gameObject.SetActive(true);
            }
            SceneManager.LoadScene("Level 2"); //move to level 2 if all animals (even carcasses) have been collected
        }

        if (collision.gameObject.tag == "pig" || collision.gameObject.tag == "horse" || collision.gameObject.tag == "cow")
        {
            collectedAnimals++;
            Debug.Log("Num collected animals: " + collectedAnimals);
            Debug.Log("Left to collect: " + (numAnimals[sceneNum] - collectedAnimals));

            if (collision.gameObject.GetComponent<Play_Sound>().getIsDead() == false)
            {
                if (collision.gameObject.tag == "pig")
                {
                    pigSound.Play();
                }
                else if (collision.gameObject.tag == "cow")
                {
                    cowSound.Play();
                }
                else if (collision.gameObject.tag == "horse")
                {
                    horseSound.Play();
                }
            }

            collision.gameObject.SetActive(false); //collect animals when touched

        }

        if (collision.gameObject.tag == "gold pick")
        {
            pickUpSound.Play();
            hasGoldPick = true;
            goldPick.gameObject.SetActive(true);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "diamond pick")
        {
            pickUpSound.Play();
            hasDiamondPick = true;
            bluePick.gameObject.SetActive(true);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "rock" || collision.gameObject.tag == "gold rock" || collision.gameObject.tag == "strong rock") //detects player is touching a rock
        {
            rb.velocity = Vector2.zero;
            rock = collision.gameObject;
            Debug.Log("ROCKKCK");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Vector2 collisionPos = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
        // Vector2 playerPos = this.transform.position;

        if (collision.gameObject.tag == "rock" || collision.gameObject.tag == "gold rock" || collision.gameObject.tag == "strong rock")
        {
            isTouchingRock = true; //check if player is actively touching rock
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "rock" || collision.gameObject.tag == "gold rock" || collision.gameObject.tag == "strong rock")
        {
            isTouchingRock = false; //check that player is no longer touching rock
        }
    }

    public void playStepSound()
    {
        if (moving)
        {
            stepSound.Play();
        }
    }

    public void playAxeSwing()
    {
        if (swingingPick)
        {
            swingAxeSound.Play();
        }
    }
}
