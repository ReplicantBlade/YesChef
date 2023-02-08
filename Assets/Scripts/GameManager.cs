using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.IO;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;
    public static GameManager Instance { get { return _Instance; } }
    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _Instance = this;
        }
    }
    public static GameManager Get() { return _Instance; }
    public enum GameState
    {
        OnGoing,
        Pause,
        End
    }
    public GameState state { get; set; } = new GameState();

    public float GameDurationInSec = 60 * 3;
    public UIManager uiManager;
    public TMPro.TextMeshProUGUI gameTimer;

    private readonly List<Transform> moveOrderList = new();
    private float currentLefttime;

    void Start()
    {
        currentLefttime = GameDurationInSec;
        state = GameState.OnGoing;
    }

    void Update()
    {
        switch (state)
        {
            case GameState.OnGoing:
                CountdownTime();
                break;
            case GameState.Pause:
                break;
            case GameState.End:
                break;
            default:
                break;
        }
    }
    public void StartGame()
    {
        state = GameState.OnGoing;
    }
    public void PauseGame()
    {
        state = GameState.Pause;
    }
    public void EndGame()
    {
        state = GameState.End;
    }
    private void CountdownTime()
    {
        currentLefttime -= Time.deltaTime;
        if (currentLefttime <= 0)
        {
            EndGame();
        }
        UpdateGameTimer();
    }
    private void UpdateGameTimer()
    {
        var time = Utilities.Instance.ToTimeString(currentLefttime);
        uiManager.ChangeText(gameTimer, time);
        
    }
    public void MovementOrder(Transform standPosition)
    {
        if (moveOrderList.Count == 0 || moveOrderList.Last() != standPosition)
        {
            moveOrderList.Add(standPosition);
        }
    }
    public List<Transform> GetOrderList() { return moveOrderList; }
    public void OrderAccomplish()
    {
        moveOrderList.RemoveAt(0);
    }
}
