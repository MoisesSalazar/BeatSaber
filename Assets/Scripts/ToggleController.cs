using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public Toggle beat_saber;
    public Toggle beat_boxer;
    public Toggle music1;
    public Toggle music2;
    public Toggle music3;
    public Toggle music4;
    public Toggle music5;
    public Toggle music6;

    private Toggle[] main_toggles;
    private Toggle[] second_toggles;
    private int currentMusicIndex = 0;
    private int maxMusicIndex = 5;

    void Start()
    {
        main_toggles = new Toggle[] { beat_saber, beat_boxer };
        // Asignar un listener a cada toggle del arreglo
        for (int i = 0; i < main_toggles.Length; i++)
        {
            int index = i; // Almacenar el índice actual para utilizar en el listener
            main_toggles[i].onValueChanged.AddListener(
                (isOn) => OnToggleValueChanged(main_toggles, index, isOn)
            );
        }

        second_toggles = new Toggle[] { music1, music2, music3, music4, music5, music6 };
        // Asignar un listener a cada toggle del arreglo
        for (int i = 0; i < second_toggles.Length; i++)
        {
            int index = i; // Almacenar el índice actual para utilizar en el listener
            second_toggles[i].onValueChanged.AddListener(
                (isOn) => OnToggleValueChanged(second_toggles, index, isOn)
            );
        }
    }

    void OnToggleValueChanged(Toggle[] toggles, int selectedToggleIndex, bool isOn)
    {
        if (isOn)
        {
            // Desactivar los otros toggles
            for (int i = 0; i < toggles.Length; i++)
            {
                if (i != selectedToggleIndex)
                {
                    toggles[i].isOn = false;
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Cambiar al toggle de música anterior
            currentMusicIndex--;
            if (currentMusicIndex < 0)
            {
                currentMusicIndex = maxMusicIndex;
            }
            SetSelectedMusicToggle();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Cambiar al siguiente toggle de música
            currentMusicIndex++;
            if (currentMusicIndex > maxMusicIndex)
            {
                currentMusicIndex = 0;
            }
            SetSelectedMusicToggle();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            beat_saber.isOn = true;
            beat_boxer.isOn = false;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            beat_saber.isOn = false;
            beat_boxer.isOn = true;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Verificar si beat_saber está marcado
            GlobalVariables.Instance.__music = GetSelectedMusic();

            if (beat_saber.isOn)
            {
                SceneManager.LoadScene("InGame");
            }
            else if (beat_boxer.isOn)
            {
                SceneManager.LoadScene("BoxGame");
            }
            // ...
        }
    }

    void SetSelectedMusicToggle()
    {
        // Desactivar todos los toggles de música
        foreach (Toggle toggle in second_toggles)
        {
            toggle.isOn = false;
        }

        // Activar el toggle de música correspondiente al índice actual
        second_toggles[currentMusicIndex].isOn = true;
    }

    string GetSelectedMusic()
    {
        if (music1.isOn)
            return "music1";
        else if (music2.isOn)
            return "music2";
        else if (music3.isOn)
            return "music3";
        else if (music4.isOn)
            return "music4";
        else if (music5.isOn)
            return "music5";
        else if (music6.isOn)
            return "music6";
        else
            return "No music selected";
    }
}
