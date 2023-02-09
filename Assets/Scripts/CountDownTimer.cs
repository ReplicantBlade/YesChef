using System;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    private enum State
    {
        Start,
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
            case State.Start:
                CountdownTime();
                break;
            case State.Stop:
                Stop();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void Initial(float time)
    {
        _counterState = State.Start;
        SetTimer(time);
        Show();
    }
    
    private void Stop()
    {
        _counterState = State.Stop;
        SetTimer(0f);
        Hide();
    }

    private void SetTimer(float t)
    {
        _timer = t;
        _timerLimit = _timer;
    }

    private void Show()
    {
        transform.gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        transform.gameObject.SetActive(false);
    }
    
    private void CountdownTime()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _counterState = State.Stop;
        }
        UpdateGameTimer();
    }
    private void UpdateGameTimer()
    {
        var time = Utilities.Instance.ToTimeString(_timer);
        _uiManager.ChangeText(counterText, time);
        var fillAmount = _timer / _timerLimit;
        _uiManager.FillImage(counterImage ,fillAmount);
    }
}
