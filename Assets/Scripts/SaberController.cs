using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-1, 0, 0) * 100 * Time.deltaTime); 
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(1, 0, 0) * 100 * Time.deltaTime); 
        }
        // if (Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //     transform.Translate(new Vector3(0, 1, 0) * 100 * Time.deltaTime); 
        // }
        // if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     transform.Translate(new Vector3(0, -1, 0) * 100 * Time.deltaTime); 
        // }
    }
}
