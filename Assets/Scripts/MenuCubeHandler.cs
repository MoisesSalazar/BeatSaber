using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCubeHandler : MonoBehaviour
{
    void LoadGame()
    {
        SceneManager.LoadScene("InGame");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "RedSaber" || collider.gameObject.tag == "BlueSaber")
        {
            GetComponent<AudioSource>().Play();
            Destroy(gameObject);
            SceneManager.LoadScene("InGame");
            // Invoke("LoadGame", 2.0f);            
        }
    }

    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
    }

    void Update()
    {

    }
}
