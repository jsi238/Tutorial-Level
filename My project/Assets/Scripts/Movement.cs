using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Movement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 1f; //create variable to set player speed

    private GameObject rock; //rock object to be removed when crushed

    private bool isTouchingRock = false; //check if player is a touching rock

    /*check which direction their is an obstacle to stop movement in that direction */
    private bool isTouchingRight = false;
    private bool isTouchingLeft = false;
    private bool isTouchingUp = false;
    private bool isTouchingDown = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*maybe less typing to just do it this way */
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveRight = Input.GetKey(KeyCode.D);

        bool destroyRock = Input.GetKeyDown(KeyCode.F);

        /* check which key is pressed and if movement is impeded in that directions */
        if (moveUp && isTouchingUp == false)
        {
            this.gameObject.transform.Translate(Vector3.up * playerSpeed * Time.deltaTime);
        }
        if (moveDown && isTouchingDown == false)
        {
            this.gameObject.transform.Translate(Vector3.down * playerSpeed * Time.deltaTime);
        }
        if (moveLeft && isTouchingLeft == false)
        {
            this.gameObject.transform.Translate(Vector3.left * playerSpeed * Time.deltaTime);
        }
        if (moveRight && isTouchingRight == false)
        {
            this.gameObject.transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
        }
        if (destroyRock && isTouchingRock)
        {
            rock.SetActive(false); //'destory' rock in players way
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
            rock = collision.gameObject;
        }
        Vector2 direction = collision.GetContact(0).normal;
        /* determine if playing is making contact from above, below, left, or right */
        if (direction.x == 1)
            isTouchingLeft = true;
        if (direction.x == -1)
            isTouchingRight = true;
        if (direction.y == 1)
            isTouchingDown = true;
        if (direction.y == -1)
            isTouchingUp = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 collisionPos = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
        Vector2 playerPos = this.transform.position;

        if (collision.gameObject.tag == "rock")
        {
            isTouchingRock = true; //check if player is actively touching rock      
        }

        Vector2 direction = collision.GetContact(0).normal;
        /* determine if playing is making contact from above, below, left, or right */
        if (direction.x == 1)
            isTouchingLeft = true;
        if (direction.x == -1)
            isTouchingRight = true;
        if (direction.y == 1)
            isTouchingDown = true;
        if (direction.y == -1)
            isTouchingUp = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "rock")
        {
            isTouchingRock = false; //check that player is no longer touching rock
        }

        /* reset is touching since we are no longer touching */
        isTouchingLeft = false;
        isTouchingRight = false;
        isTouchingDown = false;
        isTouchingUp = false;
    }
}
