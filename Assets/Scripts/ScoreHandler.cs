using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    static public int score;
    static public int bombs;
    
    void Start()
    {
        score = 0;
        bombs = 10;
        Debug.Log(bombs);
    }

    void Update()
    {
        // transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }
}
