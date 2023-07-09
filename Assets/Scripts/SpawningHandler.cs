using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningHandler : MonoBehaviour
{
    public GameObject SpawnerPrefab;
    public GameObject SimpleCubePrefab;

    public float umbral = 1.9f; // El umbral para activar la instancia.
    public float tiempoEntreInstancias = 0.5f; // El tiempo entre cada instancia.
    private float tiempoUltimaInstancia;

    GameObject[,] SpawnersMatrix;
    int rows;
    int columns;

    void SpawnMirrorHorizontal(int i, int j, int r)
    {
        SpawnersMatrix[i, j].GetComponent<SpawnerHandler>().NewCube(r);
        SpawnersMatrix[i, columns - j - 1].GetComponent<SpawnerHandler>().NewCube(r);
    }

    void SpawnMirrorVertical(int i, int j, int r)
    {
        SpawnersMatrix[i, j].GetComponent<SpawnerHandler>().NewCube(r);
        SpawnersMatrix[rows - i - 1, j].GetComponent<SpawnerHandler>().NewCube(r);
    }

    void Start()
    {

        string musicName = GlobalVariables.Instance.__music;
        AudioClip musicClip = Resources.Load<AudioClip>(musicName);

        if (musicClip != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = musicClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No se pudo cargar la música: " + musicName);
        }

        rows = 2;
        columns = 4;

        float space = SimpleCubePrefab.GetComponent<Renderer>().bounds.size.x + 1.0f;
        Vector3 position = new Vector3(
            transform.position.x - space * 2.0f - space / 2,
            transform.position.y + space * 2.0f,
            transform.position.z
        );

        SpawnersMatrix = new GameObject[rows, columns];

        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < columns; ++j)
            {
                SpawnersMatrix[i, j] = Instantiate(
                    SpawnerPrefab,
                    position,
                    Quaternion.identity,
                    transform
                );
                position = new Vector3(position.x + space, position.y, position.z);
            }
            position = new Vector3(position.x - columns * space, position.y - space, position.z);
        }
    }

    void Update()
    {
        float[] spectrumData = new float[1024];
        GetComponent<AudioSource>().GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
        float maxFrequency = 0f;
        float maxAmplitude = 0f;

        for (int i = 0; i < spectrumData.Length; i++)
        {
            if (spectrumData[i] > maxAmplitude)
            {
                maxAmplitude = spectrumData[i];
                maxFrequency = i;
            }
        }

        if (
            GetComponent<AudioSource>().time > tiempoUltimaInstancia + tiempoEntreInstancias
            && GetComponent<AudioSource>().time % umbral < Time.deltaTime
        )
        {
            if (maxFrequency < 500f)
            {
                if (Random.Range(0, 2) > 0)
                    SpawnMirrorHorizontal(
                        Random.Range(0, rows / 2 + 1),
                        Random.Range(0, columns),
                        Random.Range(0, 2)
                    );
                else
                    SpawnMirrorVertical(
                        Random.Range(0, rows / 2 + 1),
                        Random.Range(0, columns),
                        Random.Range(0, 2)
                    );
            }
            else
            {
                if (Random.Range(0, 2) > 0)
                    SpawnMirrorHorizontal(
                        Random.Range(rows / 2 + 1, rows),
                        Random.Range(0, columns),
                        Random.Range(0, 2)
                    );
                else
                    SpawnMirrorVertical(
                        Random.Range(rows / 2 + 1, rows),
                        Random.Range(0, columns),
                        Random.Range(0, 2)
                    );
            }

            tiempoUltimaInstancia = GetComponent<AudioSource>().time; // Actualizar el tiempo desde la última instancia.
        }
    }
}
