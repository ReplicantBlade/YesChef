using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private static EventSystem Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public static EventSystem Get() { return Instance; }
}
