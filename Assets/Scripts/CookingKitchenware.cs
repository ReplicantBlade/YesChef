using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class CookPositions
{
    public CountDownTimer timer;
    public Transform transform;
}
public class CookingKitchenware : MonoBehaviour
{
    [SerializeField] private bool keepChefBusy;
    [SerializeField] private bool chopVeges;
    [SerializeField] private bool cookMeat;
    [SerializeField] private List<CookPositions> cookPositions;
    private ChefPlateManager _chefPlate;
    private List<IngredientModel> _cookedIngredient = new();
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Transform transform1;
        _chefPlate = (transform1 = other.transform).GetComponent<ChefPlateManager>();
        GiveChefOneCookedItems();
        CheckChef(transform1);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _chefPlate = null;
    }
    private void CheckChef(Component chef)
    {
        if (_chefPlate == null || GetFreePosition() == null) return;
        var ingredients = _chefPlate.GetAllInHandIngredients();
        var choppingItem = ingredients.Where(ingredient => CheckWareDuty(ingredient.Value.Ingredient) && !ingredient.Value.IsCooked).Select(ingredient => _chefPlate.GetIngredient((ingredient.Key))).FirstOrDefault();
        if (choppingItem == null) return;
        StartCoroutine(StartChopping(choppingItem, chef.GetComponent<ChefController>()));
    }
    private bool CheckWareDuty(Ingredient ingredient)
    {
        return ingredient.NeedChopping() == chopVeges || ingredient.NeedCooking() == cookMeat;
    }
    private IEnumerator StartChopping(IngredientModel cookingItem, ChefController chefController) {
        if (keepChefBusy) chefController.ChefIsBusy = true;
        var cookPosition = PlaceIngredientOnKitchenware(cookingItem);
        cookPosition.timer.Initial(cookingItem.Ingredient.GetCookTime());
        cookingItem.Cook();
        yield return new WaitForSeconds(cookingItem.Ingredient.GetCookTime());
        if (keepChefBusy) chefController.ChefIsBusy = false;
        _cookedIngredient.Add(cookingItem);
        if (_chefPlate == null) yield break;
        GiveChefOneCookedItems();
    }
    private CookPositions GetFreePosition()
    {
        return cookPositions.FirstOrDefault(variable => variable.transform.childCount == 0);
    }
    private CookPositions PlaceIngredientOnKitchenware(IngredientModel ingredient)
    {
        var cookPosition = GetFreePosition();
        var visual = ingredient.Ingredient.GetStylePrefab(ingredient.IsCooked);
        var parent = cookPosition.transform;
        Instantiate(visual, parent.position, visual.transform.rotation ,parent);
        return cookPosition;
    }
    private void GiveChefOneCookedItems()
    {
        if (_cookedIngredient.Count <= 0) return;
        _chefPlate.AddIngredient(_cookedIngredient[0]);
        _cookedIngredient.Remove(_cookedIngredient[0]);
        RemoveIngredientFromKitchenware();
    }
    private void RemoveIngredientFromKitchenware()
    {
        foreach (var pos in cookPositions.Where(pos => pos.transform.childCount > 0))
        {
            Destroy(pos.transform.GetChild(0).gameObject);
            break;
        }
    }
}
