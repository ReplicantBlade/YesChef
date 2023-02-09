using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Ingredient", order = 0)]
public class Ingredient : ScriptableObject
{
    [SerializeField] private string ingredientName;
    [SerializeField] private GameObject rawStylePrefab;
    [SerializeField] private GameObject CookedStylePrefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int score;
    [SerializeField] private float cookTime;
    [SerializeField] private bool needChopping;
    [SerializeField] private bool needCooking;
    
    public string GetName()
    {
        return ingredientName;
    }
    public GameObject GetStylePrefab(bool isCooked)
    {
        return isCooked ? CookedStylePrefab : rawStylePrefab;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }
    public int GetScore()
    {
        return score;
    }
    public float GetCookTime()
    {
        return cookTime;
    }
    public bool NeedChopping()
    {
        return needChopping;
    }

    public bool NeedCooking()
    {
        return needCooking;
    }
}

public class IngredientModel
{
    public Ingredient Ingredient { get; }
    private bool _isCooked;

    public IngredientModel(Ingredient item)
    {
        Ingredient = item;
    }
    
    public bool IsCooked => _isCooked;

    public void Cook()
    {
        _isCooked = true;
    }
}
