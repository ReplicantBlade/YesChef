using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Shelf : MonoBehaviour
{
    [SerializeField] private ScrollRect ingredientsView;
    [SerializeField] private GameObject ingredientsViewContentPrefab;
    [SerializeField] private CounterManager timer;
    [FormerlySerializedAs("platePosition")] [SerializeField] private Transform shelfPlate;
    
    private ChefPlateManager _chefPlate;
    private DateTime _lastAccomplishOrderDateTime;
    private List<IngredientModel> _inProgressOrder = new();
    private readonly List<IngredientModel> _ingredientStorage = new();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _chefPlate = other.transform.GetComponent<ChefPlateManager>();
        if (_chefPlate.GetAllInHandIngredients().Count > 0 && IsShelfPlateEmptyToUse()) GetChefInHandIngredients();
        else if (_chefPlate.GetAllInHandIngredients().Count == 0 && !IsShelfPlateEmptyToUse()) GiveChefIngredientsInStorage();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _chefPlate = null;
    }

    private void GetChefInHandIngredients()
    {
        var ingredients = _chefPlate.GetAllInHandIngredients().Keys.ToList();
        foreach (var key in ingredients)
        {
            AddIngredientToStorage(_chefPlate.GetIngredient(key));
        }
        //check if its our ready Order
        CheckOrderWithStorage();
    }
    
    private void GiveChefIngredientsInStorage()
    {
        foreach (var ingredient in _ingredientStorage)
        {
            _chefPlate.AddIngredient(ingredient);
        }
        RemoveAllIngredientInStorage();
    }

    public void SetNewOrder(List<IngredientModel> newOrder)
    {
        foreach (var ingredientModel in newOrder)
        {
            var content = ingredientsViewContentPrefab;
            content.GetComponent<Image>().sprite = ingredientModel.Ingredient.GetSprite();
            AddItemToOrderView(content);
        }
        timer.InitialCountUp(-1);
        _inProgressOrder = newOrder;
    }

    private void CheckOrderWithStorage()
    {
        if(!HaveInProgressOrder()) return;
        var totalIngredientsInOrder = _inProgressOrder.Count;
        var localStorage = _ingredientStorage;
        var localOrders = _inProgressOrder;
        foreach (var ingredientModel in localStorage)
        {
            var storageIngredient = ingredientModel.Ingredient;
            if (storageIngredient.GetCookTime() > 0 && !ingredientModel.IsCooked) return;
            foreach (var model in localOrders)
            {
                var orderIngredient = model.Ingredient;
                if (orderIngredient.GetName() != storageIngredient.GetName()) continue;
                totalIngredientsInOrder--;
                if (totalIngredientsInOrder <= 0)
                {
                    AccomplishOrder();
                    return;
                };
            }
        }
    }
    
    private void AccomplishOrder()
    {
        _lastAccomplishOrderDateTime = DateTime.Now;
        timer.Stop();
        _inProgressOrder.Clear();
        ResetOrderView();
        RemoveAllIngredientInStorage();
    }
    
    public DateTime GetLastAccomplishOrderDateTime()
    {
        return _lastAccomplishOrderDateTime;
    }

    public bool HaveInProgressOrder()
    {
        return _inProgressOrder.Count > 0;
    }

    private bool IsShelfPlateEmptyToUse()
    {
        return shelfPlate.childCount == 0;
    }

    private void AddItemToOrderView(GameObject item)
    {
        var newItem = Instantiate(item, ingredientsView.content);
        var rectTransform = newItem.GetComponent<RectTransform>();
        var newSize = ingredientsView.content.sizeDelta;
        newSize.y += rectTransform.sizeDelta.y;
        ingredientsView.content.sizeDelta = newSize;
        var newPosition = rectTransform.localPosition;
        newPosition.y -= newSize.y;
        rectTransform.localPosition = newPosition;
    }

    private void ResetOrderView()
    {
        for (var i = 0; i < ingredientsView.content.childCount; i++)
        {
            Destroy(ingredientsView.content.GetChild(i).gameObject);
        }
        ingredientsView.content.sizeDelta = Vector2.zero;
    }

    private void AddIngredientToStorage(IngredientModel ingredientModel)
    {
        _ingredientStorage.Add(ingredientModel);
        var prefab = ingredientModel.Ingredient.GetStylePrefab(ingredientModel.IsCooked);
        var shelfPlatePosition = shelfPlate.position;
        var position = IsShelfPlateEmptyToUse() ? 
            shelfPlate.position : new Vector3(shelfPlatePosition.x ,shelfPlatePosition.y ,shelfPlate.GetChild(shelfPlate.childCount - 1).position.z + 3f);
        Instantiate(prefab, position, prefab.transform.rotation ,shelfPlate);
    }
    
    private void RemoveAllIngredientInStorage()
    {
        _ingredientStorage.Clear();
        for (var i = 0; i < shelfPlate.childCount; i++)
        {
            Destroy(shelfPlate.GetChild(i).gameObject);
        }
    }
}
