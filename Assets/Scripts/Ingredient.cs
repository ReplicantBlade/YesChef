using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Ingredient", order = 0)]
public class Ingredient : ScriptableObject
{
    [SerializeField] private string ingredientName;
    [SerializeField] private GameObject rawStylePrefab;
    [SerializeField] private GameObject cookedStylePrefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int score;
    [SerializeField] private float cookTime;
    [SerializeField] private int chance;
    [SerializeField] private bool needChopping;
    [SerializeField] private bool needFire;
    
    public string GetName()
    {
        return ingredientName;
    }
    public GameObject GetStylePrefab(bool isCooked)
    {
        return isCooked ? cookedStylePrefab : rawStylePrefab;
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
    public int GetChance()
    {
        return chance;
    }
    public bool NeedChopping()
    {
        return needChopping;
    }

    public bool NeedFire()
    {
        return needFire;
    }
}

public class IngredientModel
{
    public Ingredient Ingredient { get; }

    public IngredientModel(Ingredient item)
    {
        Ingredient = item;
    }
    
    public bool IsCooked { get; private set; }

    public void Cook()
    {
        IsCooked = true;
    }
}
