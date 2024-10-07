using UnityEngine;

public class Play_Sound : MonoBehaviour
{
    [SerializeField] public AudioSource animalSound;
    [SerializeField] public AudioSource dyingSound;

    public GameObject player;

    private Animator animator;

    [SerializeField] public Sprite deadAnimal;

    [SerializeField] float soundInterval = 5;

    float timeSinceLastSound = 0;
    private int numCalls = 0;

    [SerializeField] private int TIME_TILL_STARVE = 5;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<Animator>() != null)
        {
            animator = this.GetComponent<Animator>();
        }
        else
        {
            animator = null;
        }
        player = GameObject.Find("Player"); //locate player object
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSound += Time.deltaTime;
        //if animal is
        if (isVisible())
        {
            timeSinceLastSound = 0;
        }
        else
        {
            if (timeSinceLastSound >= soundInterval && !isDead)
            {
                numCalls++;
                if (numCalls > TIME_TILL_STARVE)
                {
                    isDead = true;
                    if (animator != null)
                    {
                        animator.enabled = false;
                    }
                    dyingSound.Play();
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = deadAnimal;
                    this.gameObject.transform.localScale = new Vector3(.5f, .5f, 0);
                }
                else
                {
                    animalSound.Play();
                }
                timeSinceLastSound = 0;
            }
        }
    }

    bool isVisible()
    {
        //check where object is
        Vector3 viewPort = Camera.main.WorldToViewportPoint(this.transform.position);
        //if object is visible by camera return true, else false
        if (viewPort.x >= 0 && viewPort.y >= 0 && viewPort.x <= 1 && viewPort.y <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
