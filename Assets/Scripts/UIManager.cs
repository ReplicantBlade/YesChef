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
    public void FillDropDownOption(TMPro.TMP_Dropdown dropdown, List<string> options ,List<Sprite> sprites)
    {
        dropdown.ClearOptions();
        for (int i = 0; i < options.Count; i++)
        {
            TMPro.TMP_Dropdown.OptionData data = new()
            {
                text = options[i],
                image = sprites[i],
            };
            dropdown.options.Add(data);
        }
    }

    public void ChangeText(TMPro.TextMeshProUGUI textMeshPro, string text)
    {
        textMeshPro.text = text;
    }
}
