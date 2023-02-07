using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private static EventSystem _Instance;    
    public static EventSystem Instance { get { return _Instance; } }
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
    public static EventSystem Get() { return _Instance; }

    public delegate void ClickAction();
    public static event ClickAction OnClick;
    private void OnMouseDown()
    {
        Debug.Log(transform.tag);
        OnClick?.Invoke();
    }
}
