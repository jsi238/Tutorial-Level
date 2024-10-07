using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Position : MonoBehaviour
{
    [SerializeField] Vector2 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
