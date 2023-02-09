using System.Linq;
using System.Collections.Generic;
using UnityEngine;


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
    public enum RestaurantState
    {
        Open,
        Break,
        Close
    }
    public RestaurantState state { get; set; } = new RestaurantState();

    public float GameDurationInSec = 60 * 3;
    public UIManager uiManager;
    public TMPro.TextMeshProUGUI gameTimer;

    private readonly List<Transform> moveOrderList = new();
    private float currentLefttime;

    private void Start()
    {
        currentLefttime = GameDurationInSec;
        state = RestaurantState.Open;
    }

    private void Update()
    {
        switch (state)
        {
            case RestaurantState.Open:
                CountdownTime();
                break;
            case RestaurantState.Break:
                break;
            case RestaurantState.Close:
                break;
            default:
                break;
        }
    }
    public void StartGame()
    {
        state = RestaurantState.Open;
    }
    public void PauseGame()
    {
        state = RestaurantState.Break;
    }

    private void EndGame()
    {
        state = RestaurantState.Close;
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
        if (moveOrderList.Count > 0)
            moveOrderList.RemoveAt(0);
    }
}
