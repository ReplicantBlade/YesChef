using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Tooltip("Every index of list represents Total Ingredient Order Can Have and Value Of it represent its chance")] [SerializeField]
    private List<int> totalIngredientCountChance = new();
    [SerializeField] private float newOrderTimeOut;
    [SerializeField] private List<Shelf> customersOrderShelf = new();
    private List<Ingredient> _inventory = new ();

    private void OnValidate()
    {
        if (totalIngredientCountChance.Count <= 3) return;
        Debug.LogWarning("List length cannot be greater than maxCostumerOrderSize as mentioned in GameManager");
    }

    private void Start()
    {
        _inventory = Utilities.Instance.GetAvailableIngredients();
        if (totalIngredientCountChance.Count <= GameManager.Instance.maxCostumerOrderSize) return;
        totalIngredientCountChance.RemoveRange(GameManager.Instance.maxCostumerOrderSize, totalIngredientCountChance.Count - GameManager.Instance.maxCostumerOrderSize);
    }

    private void Update()
    {
        switch (GameManager.Instance.RestaurantStatus)
        {
            case GameManager.RestaurantState.Open:
                CheckShelfList();
                SetShelfTimerStatus(false);
                break;
            case GameManager.RestaurantState.Break:
                SetShelfTimerStatus(true);
                break;
            case GameManager.RestaurantState.Close:
                //ClearShelfOrderList();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetShelfTimerStatus(bool pause)
    {
        foreach (var shelf in customersOrderShelf)
        {
            if (pause) shelf.PauseTimer();
            else shelf.ResumeTimer();
        }
    }
    
    private void ClearShelfOrderList()
    {
        foreach (var shelf in customersOrderShelf)
        {
            shelf.RemoveAllIngredientInStorage();
            shelf.AccomplishOrder(0);
        }
    }

    private void CheckShelfList()
    {
        foreach (var shelf in customersOrderShelf)
        {
            if (DateTime.Now - shelf.GetLastAccomplishOrderDateTime() > TimeSpan.FromSeconds(newOrderTimeOut) && !shelf.HaveInProgressOrder())
            {
                SendNewOrder(shelf);
            }
        }
    }

    private void SendNewOrder(Shelf shelf)
    {
        shelf.SetNewOrder(GenerateNewOrder());
    }

    private List<IngredientModel> GenerateNewOrder()
    {
        var ingredientsForOrder = new List<IngredientModel>();
        var orderCount = new List<int>();
        for (var i = 0; i < _inventory.Count; i++)
        {
            orderCount.Add(i+1);
        }
        var howManyIngredientForOrder = Utilities.Instance.GetWeightedRandomItem(orderCount ,totalIngredientCountChance);
        var ingredientsChance = _inventory.Select(ingredient => ingredient.GetChance()).ToList();
        for (var i = 0; i < howManyIngredientForOrder; i++)
        {
            var item = Utilities.Instance.GetWeightedRandomItem(_inventory, ingredientsChance);
            ingredientsForOrder.Add(new IngredientModel(item));
        }
        return ingredientsForOrder;
    }
}
