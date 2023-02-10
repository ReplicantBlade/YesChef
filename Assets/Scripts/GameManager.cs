using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public RestaurantState RestaurantStatus { get; set; }
    public int maxCostumerOrderSize = 3;
    [SerializeField] private float gameDurationInSec = 60 * 3;
    [SerializeField] private int maxNegativeScore = -100;
    [SerializeField] private CounterManager timer;
    [SerializeField] private Animation newHighScore;
    [SerializeField] private TextMeshProUGUI currentGameScoreUI;
    [SerializeField] private TextMeshProUGUI highestAchievedScoreUI;
    [SerializeField] private List<Ingredient> allIngredients = new();
    private readonly List<Transform> _moveOrderList = new();
    private int _highestAchievedScore;
    private int _currentGameScore;
    private UIManager _uiManager;
    private void Start()
    {
        RestaurantStatus = RestaurantState.Break;
        _uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        if (!PlayerPrefs.HasKey("Score")) return;
        _highestAchievedScore = PlayerPrefs.GetInt("Score");
        _uiManager.ChangeText(highestAchievedScoreUI ,$"{_highestAchievedScore}");
    }
    private void Update()
    {
        switch (RestaurantStatus)
        {
            case RestaurantState.Open:
                CheckTimer();
                break;
            case RestaurantState.Break:
                //Open Main Menu
                break;
            case RestaurantState.Close:
                //Open Main Menu
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
        RestaurantStatus = RestaurantState.Open;
        _currentGameScore = 0;
        _uiManager.ChangeText(currentGameScoreUI ,$"{_currentGameScore}");
        if (timer.GetState() != CounterManager.CounterState.Pause) timer.InitialCountDown(gameDurationInSec);
        else if (timer.GetState() == CounterManager.CounterState.Pause) timer.ResumeCounterDown();
    }
    public void PauseGame()
    {
        RestaurantStatus = RestaurantState.Break;
        timer.Pause();
    }
    private void EndGame()
    {
        RestaurantStatus = RestaurantState.Close;
        _moveOrderList.Clear();
        PlayerPrefs.SetInt("Score" ,_highestAchievedScore);
        timer.Stop();
    }
    public void NewScore(int score)
    {
        _currentGameScore += score < maxNegativeScore ? maxNegativeScore : score;
        var strScore = _currentGameScore > 0 ? $"+{_currentGameScore}" : $"{_currentGameScore}";
        _uiManager.ChangeText(currentGameScoreUI ,strScore);
        if (_currentGameScore <= _highestAchievedScore) return;
        _highestAchievedScore = _currentGameScore;
        _uiManager.ChangeText(highestAchievedScoreUI ,strScore);
        newHighScore.Play();
    }
    public void MovementOrder(Transform standPosition)
    {
        if ((_moveOrderList.Count == 0 || _moveOrderList.Last() != standPosition) && RestaurantStatus == RestaurantState.Open)
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
    public List<Ingredient> GetAvailableIngredients()
    {
        return allIngredients;
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
