using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public void FillDropDownOption(TMPro.TMP_Dropdown dropdown, List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData
        {
            text = "Refrigerator",
        });
        for (int i = 0; i < options.Count; i++)
        {
            dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData{
                text = options[i],
            });
        }
    }

    public void ChangeText(TMPro.TextMeshProUGUI textMeshPro, string text)
    {
        textMeshPro.text = text;
    }
}
