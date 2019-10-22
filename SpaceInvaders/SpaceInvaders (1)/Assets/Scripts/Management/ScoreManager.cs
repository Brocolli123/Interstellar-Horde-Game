using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI highScore;

    public static int score;      //change score to be saved here
    static int enemyCount;
    public TextMeshProUGUI scoreTxt;


    void Start()
    {
        score = 0;
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        GameManager.Load(); //creates the empty text file
        highScore.text = "High Score: " + SaveData.current.highScore.ToString("0");   //puts highscore on text formatted to 2 d.p
    }

    public static void IncreaseScore()
    {
        score++;
        if (score >= enemyCount)
        {
            //GameManager.PlayerWon();
        }
    }

    void Update()
    {
     //   if (!GameManager.Paused)                  Apply to our score
       // {
        //    score += Time.deltaTime;
          //  scoreObject.text = "Score: " + score.ToString("0.00");
        //}

    }

    public void LateUpdate()
    {
        scoreTxt.text = "Score: " + score;
    }

    public static int GetScore()
    {
        return score;
    }

    public static string GetHighScore()
    {
        return SaveData.current.highScore.ToString("0");
    }
}
