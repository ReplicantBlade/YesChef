using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    public GameObject endGamePanel;
    public RectTransform gameResult;
    public RectTransform currentWaveUI;
    public RectTransform totalWaveUI;
    public RectTransform bulletAmountUI;
    public RectTransform scoreUI;
    public RectTransform highestScoreUI;
    public UnityEngine.UI.Slider fuelSlider;
    public UnityEngine.UI.Slider healthSlider;
    
    public void InitialEndGamePanel(string message ,int playerScore)
    {
        gameResult.GetComponent<TextMeshProUGUI>().text = message;
        endGamePanel.SetActive(true);

        CalculateScore(playerScore);
    }

    public void SetCurrentWave(string txt)
    {
        currentWaveUI.GetComponent<TextMeshProUGUI>().text = txt;
    }

    public void SetTotalWave(string txt)
    {
        totalWaveUI.GetComponent<TextMeshProUGUI>().text = txt;
    }

    public void SetBulletAmount(string txt)
    {
        bulletAmountUI.GetComponent<TextMeshProUGUI>().text = txt;
    }

    public void SetFuelSlider(float max ,float value)
    {
        if (max > 0) fuelSlider.maxValue = max;
        fuelSlider.value = value;
    }

    public void SetHealthSlider(float max, float value)
    {
        if (max > 0) healthSlider.maxValue = max;
        healthSlider.value = value;
    }

    public void RestartGame() 
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public void ExitGame()
    {
        RestartGame();
        //Application.Quit();
    }

    private void CalculateScore(int score)
    {
        int highestScore = 10;//DataSerializer.TryLoad("HighestScore", out int value) ? value : 0;

        if (score > highestScore)
        {
            highestScore = score;
            //DataSerializer.Save("HighestScore", score);
            highestScoreUI.GetComponentInParent<Animator>().enabled = true;
        }

        scoreUI.GetComponent<TextMeshProUGUI>().text = score.ToString();
        highestScoreUI.GetComponent<TextMeshProUGUI>().text = highestScore.ToString();
    }
}
