using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefPlateManager : MonoBehaviour
{
    [SerializeField] private GameObject Plate;
    private readonly List<Transform> _slots = new();
    private readonly Dictionary<int ,Ingredient> _inHandIngredients = new();
    // Start is called before the first frame update
    private void Start()
    {
        for (var i = 0; i < Plate.transform.childCount; i++)
        {
            _slots.Add(Plate.transform.GetChild(i));
        }
    }

    public Dictionary<int ,Ingredient> GetAllInHandIngredients()
    {
        return _inHandIngredients;
    }

    public Ingredient GetIngredient(int slot)
    {
        var item = _inHandIngredients[slot];
        return RemoveIngredientFromPlate(slot) ? item : null;
    }

    public bool AddIngredient(Ingredient newIngredient)
    {
        var slotNum = GetFreeSlotNumber();
        if (slotNum == -1) return false;
        _inHandIngredients.Add(slotNum, newIngredient);
        InstantiateIngredientVisual(newIngredient.GetStylePrefab(), _slots[slotNum]);
        return true;
    }

    private bool RemoveIngredientFromPlate(int slot)
    {
        if (!_inHandIngredients.ContainsKey(slot)) return false;
        _inHandIngredients.Remove(slot);
        DestroyIngredientVisual(slot);
        return true;
    }

    private int GetFreeSlotNumber()
    {
        for (var i = 0; i < _slots.Count; i++)
        {
            if (_inHandIngredients.ContainsKey(i) == false)
            {
                return i;
            }
        }
        return -1;
    }

    private void DestroyIngredientVisual(int slot)
    {
        Destroy(_slots[slot].GetChild(0).gameObject);
    }

    private static void InstantiateIngredientVisual(GameObject visual, Transform parent)
    {
        Instantiate(visual, parent.position, visual.transform.rotation ,parent);
    }
}
