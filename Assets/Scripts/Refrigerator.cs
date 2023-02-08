using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Refrigerator : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown dropdown;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private List<Ingredient> inventory = new();

    // Start is called before the first frame update
    void Start()
    {
        FillDropDown();
        CloseDropDown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDropDownMenu();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseDropDown();
        }
    }

    private void OpenDropDownMenu() 
    {
        dropdown.interactable = true;
        dropdown.Show();
    }

    private void CloseDropDown()
    {
        dropdown.interactable = false;
        dropdown.Hide();
    }

    private void FillDropDown()
    {
        List<string> options = new();
        List<Sprite> sprites = new();
        foreach (Ingredient ingredient in inventory)
        {
            options.Add(ingredient.GetName());
            sprites.Add(ingredient.GetSprite());
        }
        uiManager.FillDropDownOption(dropdown, options, sprites);
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
    }

    private void DropdownValueChanged()
    {
        Debug.Log(inventory[dropdown.value].GetName());
        CloseDropDown();
    }
}
