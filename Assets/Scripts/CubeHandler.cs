using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{
    bool isSmash = false;
    float speed = 10.0f;
    float timeToUnspawn = 5.0f;

    void DestroyCube()
    {
        Destroy(gameObject);
        // GetComponent<AudioSource>().Play();
        // para Alex del futuro: eventualmente poner un audio de destruccion :V
    }

    void OnTriggerExit(Collider collider)
    {
        // Debug.Log("COLISION SABER");
        if (collider.CompareTag("Destroyer"))
        {
            DestroyCube();
            ScoreHandler.bombs += 2;
        }
    }

    // void OnTriggerExit(Collider collider)
    // {
    //     if (collider.gameObject.tag == "RedSaber" || collider.gameObject.tag == "BlueSaber")
    //     {
    //         if (collider.gameObject.tag.Substring(0, 3) == gameObject.tag.Substring(0, 3))
    //             ScoreHandler.score += 3;
    //         else
    //             ScoreHandler.score += 1;
    //     }
    // }

    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
    }

    void Update()
    {
        if (!isSmash)
        {
            transform.Translate(new Vector3(0, 0, -1) * speed * Time.deltaTime); 
        }
    }
}
