using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shelf : MonoBehaviour
{
    [SerializeField] private ScrollRect ingredientsView;
    [SerializeField] private GameObject ingredientsViewContentPrefab;
    [SerializeField] private GameObject scorePointPrefab;
    [SerializeField] private CounterManager timer;
    [SerializeField] private Transform shelfPlate;
    
    private ChefPlateManager _chefPlate;
    private DateTime _lastAccomplishOrderDateTime;
    private List<IngredientModel> _inProgressOrder = new();
    private readonly List<IngredientModel> _ingredientStorage = new();
    private UIManager _uiManager;

    private void Start()
    {
        _uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _chefPlate = other.transform.GetComponent<ChefPlateManager>();
        if (_chefPlate.GetAllInHandIngredients().Count > 0 && DoShelfPlateHaveFreeSpace()) GetChefInHandIngredients();
        else if (_chefPlate.GetAllInHandIngredients().Count == 0 && _ingredientStorage.Count > 0) GiveChefIngredientsInStorage();
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
        var localCloneStorage = new List<IngredientModel>(_ingredientStorage);
        var localCloneOrders = new List<IngredientModel>(_inProgressOrder);
        var score = 0;
        for (int i = 0; i < localCloneStorage.Count; i++)
        {
            var ingredientModel = localCloneStorage[i];
            var storageIngredient = ingredientModel.Ingredient;
            if (storageIngredient.GetCookTime() > 0 && !ingredientModel.IsCooked) return;
            for (int j = 0; j < localCloneOrders.Count; j++)
            {
                var model = localCloneOrders[j];
                var orderIngredient = model.Ingredient;
                if (orderIngredient.GetName() != storageIngredient.GetName()) continue;
                localCloneOrders.Remove(model);
                score += storageIngredient.GetScore();
                totalIngredientsInOrder--;
                if (totalIngredientsInOrder <= 0)
                {
                    AccomplishOrder(score);
                    return;
                }
            }
        }
    }
    
    public void AccomplishOrder(int score)
    {
        var calculatedScore = score - (int)timer.GetTime();
        PopScorePoint(calculatedScore);
        GameManager.Instance.NewScore(calculatedScore);
        _lastAccomplishOrderDateTime = DateTime.Now;
        timer.Stop();
        _inProgressOrder.Clear();
        ResetOrderView();
        RemoveAllIngredientInStorage();
    }

    private void PopScorePoint(int score)
    {
        var ingredientsViewTransform = ingredientsView.transform;
        var scorePoint = Instantiate(scorePointPrefab, ingredientsViewTransform.position,Quaternion.Euler(0,-90,0) ,ingredientsViewTransform);
        _uiManager.ChangeText(scorePoint.GetComponent<TMPro.TextMeshProUGUI>() ,score > 0 ? $"+{score}" : $"{score}");
        scorePoint.GetComponent<Animation>().Play(score > 0 ? "ScorePointPositive" : "ScorePointNegative");
        Destroy(scorePoint.gameObject ,5);
    }
    
    public DateTime GetLastAccomplishOrderDateTime()
    {
        return _lastAccomplishOrderDateTime;
    }

    public bool HaveInProgressOrder()
    {
        return _inProgressOrder.Count > 0;
    }

    private bool DoShelfPlateHaveFreeSpace()
    {
        return shelfPlate.childCount < GameManager.Instance.maxCostumerOrderSize;
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
        var position = shelfPlate.childCount == 0 ? 
            shelfPlate.position : new Vector3(shelfPlatePosition.x ,shelfPlatePosition.y ,shelfPlate.GetChild(shelfPlate.childCount - 1).position.z + 3f);
        Instantiate(prefab, position, prefab.transform.rotation ,shelfPlate);
    }
    
    public void RemoveAllIngredientInStorage()
    {
        _ingredientStorage.Clear();
        for (var i = 0; i < shelfPlate.childCount; i++)
        {
            Destroy(shelfPlate.GetChild(i).gameObject);
        }
    }

    public void PauseTimer()
    {
        timer.Pause();
    }

    public void ResumeTimer()
    {
        if (timer.GetState() == CounterManager.CounterState.Pause)
            timer.ResumeCounterUp();
    }
}
