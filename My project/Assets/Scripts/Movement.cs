using UnityEngine;

public class Movement : MonoBehaviour
{
    private Animator animator;
    [SerializeField] float playerSpeed = 1f; //create variable to set player speed
    private Vector2 movement; // vector for movement
    private Rigidbody2D rb;
    private bool facingRight = true;

    private GameObject rock; //rock object to be removed when crushed

    private bool isTouchingRock = false; //check if player is a touching rock

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        /*maybe less typing to just do it this way */
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveRight = Input.GetKey(KeyCode.D);

        bool destroyRock = Input.GetKeyDown(KeyCode.F);

        /* check which key is pressed and if movement is impeded in that directions */
        if (moveUp) movement.y = 1;
        if (moveDown ) movement.y = -1;
        if (moveLeft ) {
            movement.x = -1;
            facingRight = false;
        }
        if (moveRight ) {
            movement.x = 1;
            facingRight = true;
        }

        if (!moveUp && !moveDown) movement.y = 0;
        if (!moveLeft && !moveRight) movement.x = 0;

        movement.Normalize();

        rb.MovePosition(rb.position + movement * playerSpeed * Time.fixedDeltaTime);

        if (destroyRock && isTouchingRock)
        {
            rock.SetActive(false); //'destory' rock in players way
        }

        // animation
        if (movement.x != 0 || movement.y != 0) {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);
        }
        bool moving = moveRight || moveLeft || moveUp || moveDown;

        if (moving) {
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }

        Vector3 theScale = transform.localScale;
        if (facingRight) {
            if (theScale.x < 0) theScale.x *= -1;
            transform.localScale = theScale;
        } else {
            if (theScale.x > 0) theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "pig" || collision.gameObject.tag == "horse" || collision.gameObject.tag == "cow")
        {
            collision.gameObject.SetActive(false); //collect animals when touched
        }

        if (collision.gameObject.tag == "rock") //detects player is touching a rock or fence
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

        if (collision.gameObject.tag == "rock")
        {
            isTouchingRock = true; //check if player is actively touching rock
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "rock")
        {
            isTouchingRock = false; //check that player is no longer touching rock
        }
    }
}
