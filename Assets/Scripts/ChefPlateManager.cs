using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefPlateManager : MonoBehaviour
{
    [SerializeField] private GameObject Plate;
    private readonly List<Transform> slots = new();
    private readonly Dictionary<int ,Ingredient> inHandIngredients = new();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Plate.transform.childCount; i++)
        {
            slots.Add(Plate.transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Dictionary<int ,Ingredient> GetAllInHandIngredients()
    {
        return inHandIngredients;
    }

    public bool RemoveIngredientFromPlate(int slot)
    {
        if (inHandIngredients.ContainsKey(slot))
        {
            inHandIngredients.Remove(slot);
            DestroyIngredientVisual(slot);
            return true;
        }
        return false;
    }

    public bool AddIngredientToPlate(Ingredient newIngredient)
    {
        int slotNum = GetFreeSlotNumber();
        if (slotNum == -1) return false;
        inHandIngredients.Add(slotNum, newIngredient);
        InstantiateIngredientVisual(newIngredient.GetStylePrefab(), slots[slotNum]);
        return true;
    }

    private int GetFreeSlotNumber()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (inHandIngredients.ContainsKey(i) == false)
            {
                return i;
            }
        }
        return -1;
    }

    private void DestroyIngredientVisual(int slot)
    {
        Destroy(slots[slot].GetChild(0).gameObject);
    }

    private void InstantiateIngredientVisual(GameObject visual, Transform parent)
    {
        Instantiate(visual, parent.position, Quaternion.identity ,parent);
    }
}
