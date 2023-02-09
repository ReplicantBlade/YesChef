using System.Collections.Generic;
using UnityEngine;
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
