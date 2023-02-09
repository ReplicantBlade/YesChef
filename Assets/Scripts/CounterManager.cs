using System;
using UnityEngine;
using UnityEngine.UI;

public class CounterManager : MonoBehaviour
{
    private enum State
    {
        StartCountDown,
        StartCountUp,
        Stop
    }
    [SerializeField] private TMPro.TextMeshProUGUI counterText;
    [SerializeField] private Image counterImage;
    private UIManager _uiManager;
    private State _counterState;
    
    private float _timer;
    private float _timerLimit;
    private void Start()
    {
        _uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        Stop();
    }

    private void Update()
    {
        switch (_counterState)
        {
            case State.StartCountDown:
                Countdown();
                break;            
            case State.StartCountUp:
                CountUp();
                break;
            case State.Stop:
                Stop();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void InitialCountDown(float sec)
    {
        _counterState = State.StartCountDown;
        SetTime(sec ,sec);
        Show();
    }
    
    public void InitialCountUp(float secLimit)
    {
        _counterState = State.StartCountUp;
        SetTime(0f ,secLimit);
        Show();
    }
    
    public void Stop()
    {
        _counterState = State.Stop;
        SetTime(0f,0f);
        Hide();
    }

    private void SetTime(float startTime ,float limit)
    {
        _timer = startTime;
        _timerLimit = limit;
    }

    private void Show()
    {
        transform.gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        transform.gameObject.SetActive(false);
    }
    
    private void Countdown()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _counterState = State.Stop;
        }
        UpdateGameTimer(_timer);
    }
    
    private void CountUp()
    {
        _timer += Time.deltaTime;
        if (_timerLimit > 0 && _timer >= _timerLimit)
        {
            _counterState = State.Stop;
        }
        UpdateGameTimer(_timer);
    }
    
    private void UpdateGameTimer(float sec)
    {
        var time = Utilities.Instance.ToTimeString(sec);
        _uiManager.ChangeText(counterText, time);
        if (ReferenceEquals(counterImage, null)) return;
        var fillAmount = _timer / _timerLimit;
        _uiManager.FillImage(counterImage, fillAmount);
    }
}
