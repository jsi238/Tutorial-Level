using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Movement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 1f;

    private GameObject rock;

    private bool isTouchingRock = false;

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
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S);
        bool moveLeft = Input.GetKey(KeyCode.A);
        bool moveRight = Input.GetKey(KeyCode.D);

        bool destroyRock = Input.GetKeyDown(KeyCode.F);

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
            rock.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "pig" || collision.tag == "horse" || collision.tag == "cow")
        {
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector2 collisionPos = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
        Vector2 playerPos = this.transform.position;

        if (collision.tag == "rock")
        {
            isTouchingRock = true;
            rock = collision.gameObject;
            if (collisionPos.x > playerPos.x)
            {
                isTouchingRight = true;
            }
            if (collisionPos.x < playerPos.x)
            {
                isTouchingLeft = true;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "rock")
        {
            isTouchingRock = false;
            isTouchingLeft = false;
            isTouchingRight = false;
            isTouchingDown = false;
            isTouchingUp = false;
        }
    }
}
