using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Ingredient", order = 0)]
public class Ingredient : ScriptableObject
{
    public string ingredientName;
    public GameObject rawStylePrefab;
    public GameObject CookedStylePrefab;
    public int score;

    private bool isCooked = false;
    public void Cook()
    {
        isCooked= true;
    }
    public GameObject GetStylePrefab()
    {
        return isCooked ? CookedStylePrefab : rawStylePrefab;
    }
}
