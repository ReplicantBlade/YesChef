using System.Collections.Generic;
using UnityEngine;

public class ChefPlateManager : MonoBehaviour
{
    [SerializeField] private GameObject plate;
    private readonly List<Transform> _slots = new();
    private readonly Dictionary<int ,IngredientModel> _inHandIngredients = new();
    private void Start()
    {
        for (var i = 0; i < plate.transform.childCount; i++)
        {
            _slots.Add(plate.transform.GetChild(i));
        }

        PlateVisualHandler();
    }

    public Dictionary<int ,IngredientModel> GetAllInHandIngredients()
    {
        return _inHandIngredients;
    }

    public IngredientModel GetIngredient(int slot)
    {
        var item = _inHandIngredients[slot];
        return RemoveIngredientFromPlate(slot) ? item : null;
    }

    public bool AddIngredient(IngredientModel newIngredient)
    {
        var slotNum = GetFreeSlotNumber();
        if (slotNum == -1) return false;
        _inHandIngredients.Add(slotNum, newIngredient);
        InstantiateIngredientVisual(newIngredient.Ingredient.GetStylePrefab(newIngredient.IsCooked), _slots[slotNum]);
        PlateVisualHandler();
        return true;
    }

    private bool RemoveIngredientFromPlate(int slot)
    {
        if (!_inHandIngredients.ContainsKey(slot)) return false;
        _inHandIngredients.Remove(slot);
        DestroyIngredientVisual(slot);
        PlateVisualHandler();
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

    private void PlateVisualHandler()
    {
        plate.SetActive(_inHandIngredients.Count > 0);
    }
}
