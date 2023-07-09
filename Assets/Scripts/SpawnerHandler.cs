using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnerHandler : MonoBehaviour
{
    public GameObject SimpleCubePrefab1;
    public GameObject SimpleCubePrefab2;

    public void NewCube(int r)
    {
        GameObject NewObject;
        if (r == 0)
            NewObject = Instantiate(SimpleCubePrefab1, transform.position, Quaternion.identity);
        else if (r == 1)
            NewObject = Instantiate(SimpleCubePrefab2, transform.position, Quaternion.identity);
    }
    
    void Start()
    {
        // NewCube(Random.Range(0, 2));
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     NewCube(Random.Range(0, 2));
        // }
    }
}
