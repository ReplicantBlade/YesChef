using System;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.PauseGame();
        }
    }

    public static EventSystem Get() { return Instance; }
}