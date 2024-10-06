using UnityEngine;

public class Play_Sound : MonoBehaviour
{
    [SerializeField] public AudioSource animalSound;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject soundWave;

    [SerializeField] float soundInterval = 5;

    float timeSinceLastSound = 0;

    float angleToPlayer;
    Vector3 nearestPoint;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); //locate player object
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSound +=Time.deltaTime;
        //if animal is
        if (isVisible())
        {
            timeSinceLastSound = 0;
        }
        else
        {
            if (timeSinceLastSound >= soundInterval)
            {
                animalSound.enabled = true;
                animalSound.Play();
                //Debug.Log("Playing sounds");
                angleToPlayer = getDirectionToPlayer();
                nearestPoint = nearestEdge();
                nearestPoint = new Vector3(nearestPoint.x, nearestPoint.y, 0);
                //when playing sound, fire a sound wave from direction of sound
                Instantiate(soundWave, nearestPoint, Quaternion.identity);

                //Sound wave should be angled towards the player and move briefly towards them
                //might need its own script to do this
                soundWave.transform.eulerAngles -= new Vector3(0, 0, angleToPlayer);
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

    private float getDirectionToPlayer()
    {
        return Vector2.Angle(this.transform.position, player.transform.position);
    }

    private Vector3 nearestEdge()
    {
        Collider2D cameraBounds = Camera.main.GetComponent<BoxCollider2D>();
        Collider2D animalBounds = this.GetComponent<Collider2D>();
        Vector3 nearestEdge = cameraBounds.ClosestPoint(this.transform.position);

        return nearestEdge;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animalSound.Play();
        }
    }
}
