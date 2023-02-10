using UnityEngine;
using UnityEngine.UI;

public class CounterManager : MonoBehaviour
{
    public enum CounterState
    {
        StartCountDown,
        StartCountUp,
        Pause,
        Stop
    }
    [SerializeField] private TMPro.TextMeshProUGUI counterText;
    [SerializeField] private Image counterImage;
    private UIManager _uiManager;
    private CounterState _counterState;
    
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
            case CounterState.StartCountDown:
                Countdown();
                break;            
            case CounterState.StartCountUp:
                CountUp();
                break;
            case CounterState.Stop:
                Stop();
                break;
        }
    }
    
    public void InitialCountDown(float sec)
    {
        _counterState = CounterState.StartCountDown;
        SetTime(sec ,sec);
        Show();
    }
    
    public void InitialCountUp(float secLimit)
    {
        _counterState = CounterState.StartCountUp;
        SetTime(0f ,secLimit);
        Show();
    }

    public float GetTime()
    {
        return _timer;
    }

    public CounterState GetState()
    {
        return _counterState;
    }

    public void Pause()
    {
        _counterState = CounterState.Pause;
    }
    public void ResumeCounterDown()
    {
        _counterState = CounterState.StartCountDown;
    }
    public void ResumeCounterUp()
    {
        _counterState = CounterState.StartCountUp;
    }
    public void Stop()
    {
        _counterState = CounterState.Stop;
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
            _counterState = CounterState.Stop;
        }
        UpdateGameTimer(_timer);
    }
    
    private void CountUp()
    {
        _timer += Time.deltaTime;
        if (_timerLimit > 0 && _timer >= _timerLimit)
        {
            _counterState = CounterState.Stop;
        }
        UpdateGameTimer(_timer);
    }
    
    private void UpdateGameTimer(float sec)
    {
        var time = Utilities.Instance.ToTimeString(sec);

        if (_counterState == CounterState.StartCountDown && sec < _timerLimit/3) _uiManager.ChangeText(counterText, time, Color.red);
        else if (_counterState == CounterState.StartCountUp && sec > 20) _uiManager.ChangeText(counterText, time, Color.red);
        else if (_counterState == CounterState.Pause) _uiManager.ChangeText(counterText, time, Color.blue);
        else _uiManager.ChangeText(counterText, time ,Color.white);
        
        if (ReferenceEquals(counterImage, null)) return;
        var fillAmount = _timer / _timerLimit;
        _uiManager.FillImage(counterImage, fillAmount);
    }
}
