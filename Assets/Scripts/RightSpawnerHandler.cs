using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightSpawnerHandler : MonoBehaviour
{
    public GameObject SimpleCubePrefab1;
    public GameObject SimpleCubePrefab2;

    bool canInput = true;
    float cooldownTime = 1.0f; // Tiempo de enfriamiento en segundos
    float cooldownTimer = 0.0f; // Temporizador actual

    public void NewCube(int r)
    {
        GameObject NewObject;
                if (r == 0)
            // NewObject = Instantiate(SimpleCubePrefab1, transform.position, Quaternion.identity);
            NewObject = Instantiate(SimpleCubePrefab1, transform.position, Quaternion.Euler(0f, 45f, 0f));
        else if (r == 1)
            // NewObject = Instantiate(SimpleCubePrefab2, transform.position, Quaternion.identity);
            NewObject = Instantiate(SimpleCubePrefab2, transform.position, Quaternion.Euler(0f, 45f, 0f));
    }
    
    void Update()
    {
        if (!canInput)
        {
            // Si no se puede ingresar, incrementa el temporizador
            cooldownTimer += Time.deltaTime;

            // Si el temporizador alcanza el tiempo de enfriamiento, permite el input nuevamente
            if (cooldownTimer >= cooldownTime)
            {
                canInput = true;
                cooldownTimer = 0.0f; // Reinicia el temporizador
            }
        }

        // Aquí puedes manejar tu lógica de input normalmente, pero solo se activará si canInput es verdadero
        if (canInput && ScoreHandler.bombs > -1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NewCube(Random.Range(0, 2));
                ScoreHandler.bombs -= 1;
                Debug.Log(ScoreHandler.bombs);
                canInput = false; // Desactiva el input hasta que se complete el cooldown
            }
        }
    }
}
