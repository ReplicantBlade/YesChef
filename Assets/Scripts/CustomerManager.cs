using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomerManager : MonoBehaviour
{
    [Tooltip("Every index of list represents Total Ingredient Order Can Have and Value Of it represent its chance")] [SerializeField]
    private List<int> totalIngredientCountChance = new();
    [SerializeField] private float newOrderTimeOut;
    [SerializeField] private List<Shelf> customersOrderShelf = new();
    private List<Ingredient> _inventory = new ();

    private void OnValidate()
    {
        if (totalIngredientCountChance.Count <= _inventory.Count) return;
        totalIngredientCountChance.RemoveRange(3, totalIngredientCountChance.Count - 3);
        Debug.LogWarning("List length cannot be greater than 3 as mentioned in YesChef Doc");
    }

    private void Start()
    {
        _inventory = Utilities.Instance.GetAvailableIngredients();
    }

    private void Update()
    {
        switch (GameManager.Instance.state)
        {
            case GameManager.RestaurantState.Open:
                CheckShelfList();
                break;
            case GameManager.RestaurantState.Break:
                break;
            case GameManager.RestaurantState.Close:
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
        var howManyIngredientForOrder = Utilities.Instance.GetWeightedRandomItem<int>(orderCount ,totalIngredientCountChance);
        var ingredientsChance = _inventory.Select(ingredient => ingredient.GetChance()).ToList();
        for (var i = 0; i < howManyIngredientForOrder; i++)
        {
            var item = Utilities.Instance.GetWeightedRandomItem<Ingredient>(_inventory, ingredientsChance);
            ingredientsForOrder.Add(new IngredientModel(item));
        }
        return ingredientsForOrder;
    }
}
