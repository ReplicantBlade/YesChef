using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Ingredient", order = 0)]
public class Ingredient : ScriptableObject
{
    [SerializeField] private string ingredientName;
    [SerializeField] private GameObject rawStylePrefab;
    [SerializeField] private GameObject CookedStylePrefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int score;
    [SerializeField] private bool needChopping;
    [SerializeField] private bool needCooking;

    private bool isCooked = false;
    public void Cook()
    {
        isCooked= true;
    }
    public string GetName()
    {
        return ingredientName;
    }
    public GameObject GetStylePrefab()
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
    public bool NeedChopping()
    {
        return needChopping;
    }
    public bool NeedCooking()
    {
        return needCooking;
    }
}
