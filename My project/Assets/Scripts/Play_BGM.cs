using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_BGM : MonoBehaviour
{
    [SerializeField] AudioSource bgm;

    // Start is called before the first frame update
    void Start()
    {
        bgm.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
