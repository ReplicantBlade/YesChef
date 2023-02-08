using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown dropdown;
    [SerializeField] private UIManager uiManager;
    private static List<Ingredient> _inventory = new ();
    private static ChefPlateManager _chefPlate;
    // Start is called before the first frame update
    private void Start()
    {
        _inventory = Utilities.Instance.GetAvailableIngredients();
        FillDropDown();
        CloseDropDown();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        OpenDropDownMenu();
        _chefPlate = other.transform.GetComponent<ChefPlateManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        CloseDropDown();
        _chefPlate = null;
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
        ResetDropDown();
    }

    private void ResetDropDown()
    {
        dropdown.value = 0;
    }

    private void DropdownValueChanged(int value)
    {
        if (value < 1) return;
        var ingredientIndex = value - 1;
        GiveChefChosenIngredient(ingredientIndex);
        ResetDropDown();
    }

    private static void GiveChefChosenIngredient(int ingredientIndex)
    {
        if (_chefPlate == null) return;
        _chefPlate.AddIngredient(_inventory[ingredientIndex]);
    }

    private void FillDropDown()
    {
        var options = _inventory.Select(ingredient => ingredient.GetName()).ToList();
        uiManager.FillDropDownOption(dropdown, options);
        dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown.value); });
    }
}
