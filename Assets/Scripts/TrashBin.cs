using System.Linq;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    private static ChefPlateManager _chefPlate;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_chefPlate == null)
            _chefPlate = other.transform.GetComponent<ChefPlateManager>();
        CleanChefPlate();
    }

    private static void CleanChefPlate()
    {
        if (_chefPlate == null) return;
        var ingredients = _chefPlate.GetAllInHandIngredients().Keys.ToList();
        foreach (var key in ingredients)
        {
            _chefPlate.GetIngredient(key);
        }
    } 
}
