using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
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
    public static GameManager Get() { return Instance; }
    public enum RestaurantState
    {
        Open,
        Break,
        Close
    }
    public RestaurantState State { get; set; }

    [SerializeField]private float gameDurationInSec = 60 * 3;
    [SerializeField]private CounterManager timer;
    private readonly List<Transform> _moveOrderList = new();
    private void Start()
    {
        StartGame();
    }
    private void Update()
    {
        switch (State)
        {
            case RestaurantState.Open:
                CheckTimer();
                break;
            case RestaurantState.Break:
                
                break;
            case RestaurantState.Close:
                
                break;
        }
    }

    private void CheckTimer()
    {
        if (timer.GetState() == CounterManager.CounterState.Stop || timer.GetTime() <= 0)
        {
            EndGame();
        }
    }

    public void StartGame()
    {
        State = RestaurantState.Open;
        timer.InitialCountDown(gameDurationInSec);
    }
    public void PauseGame()
    {
        State = RestaurantState.Break;
        timer.Pause();
    }
    private void EndGame()
    {
        State = RestaurantState.Close;
        timer.Stop();
    }
    public void MovementOrder(Transform standPosition)
    {
        if (_moveOrderList.Count == 0 || _moveOrderList.Last() != standPosition)
        {
            _moveOrderList.Add(standPosition);
        }
    }
    public IEnumerable<Transform> GetOrderList() { return _moveOrderList; }
    public void OrderAccomplish()
    {
        if (_moveOrderList.Count > 0)
            _moveOrderList.RemoveAt(0);
    }
}
