using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 1f;

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

        if (moveUp)
        {
            this.gameObject.transform.Translate(Vector3.up * playerSpeed * Time.deltaTime);
        }
        if (moveDown)
        {
            this.gameObject.transform.Translate(Vector3.down * playerSpeed * Time.deltaTime);
        }
        if (moveLeft)
        {
            this.gameObject.transform.Translate(Vector3.left * playerSpeed * Time.deltaTime);
        }
        if (moveRight)
        {
            this.gameObject.transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
        }
    }
}
