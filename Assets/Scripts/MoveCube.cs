using UnityEngine;

public class MoveCube : MonoBehaviour
{
    public Vector3 targetPosition = Vector3.zero;
    public float speed = 1f;

    private void Update()
    {
        // Calcula la dirección hacia el destino
        Vector3 direction = targetPosition - transform.position;

        // Mueve el cubo hacia el destino
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        // Comprueba si el cubo ha alcanzado el destino
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            // Si ha llegado al destino, detén el movimiento
            enabled = false;
            Debug.Log("El cubo ha llegado a la posición (0, 0, 0).");
        }
    }
}
