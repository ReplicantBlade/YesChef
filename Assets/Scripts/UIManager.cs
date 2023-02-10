using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject restartBtn;
    [SerializeField] private GameObject pauseBtn;

    private void Update()
    {
        switch (GameManager.Instance.RestaurantStatus)
        {
            case GameManager.RestaurantState.Open:
                if (mainMenu.activeSelf)
                {
                    mainMenu.SetActive(false);
                    pauseBtn.SetActive(true);
                }
                break;
            case GameManager.RestaurantState.Break:
                if (!mainMenu.activeSelf)
                {
                    mainMenu.SetActive(true);
                    pauseBtn.SetActive(false);
                    restartBtn.SetActive(false);
                    startBtn.SetActive(true);
                }
                break;
            case GameManager.RestaurantState.Close:
                if (!mainMenu.activeSelf)
                {
                    mainMenu.SetActive(true);
                    pauseBtn.SetActive(false);
                    restartBtn.SetActive(true);
                    startBtn.SetActive(false);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public void FillDropDownOption(TMPro.TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData
        {
            text = "Refrigerator",
        });
        foreach (var t in options)
        {
            dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData{
                text = t,
            });
        }
    }
    public void ChangeText(TMPro.TextMeshProUGUI textMeshPro, string text)
    {
        textMeshPro.text = text;
    }
    public void FillImage(Image img ,float amount)
    {
        img.fillAmount = amount < 1 ? amount : 1;
    }
}
