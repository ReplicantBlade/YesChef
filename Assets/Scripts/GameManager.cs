using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.IO;
using TMPro;

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
        GettingNewOrder,
        EndWave
    }

    public GameState state { get; set; } = new GameState();

    private List<Transform> moveOrderList = new List<Transform>();
    //private UIManager UIManager;

    void Start()
    {
        //UIManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
       // player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        switch (state)
        {
            //case GameState.Wave:
                //WaveSpawnState();
                //break;
           // case GameState.EndWave:
                //EndWaveState();
                //break;
            //case GameState.Win:
                //WinGameState();
               // break;
            //case GameState.Lose:
                //LoseGameState();
               // break;
            default:
                break;
        }
    }

    public void GiveOrder(Transform standPosition)
    {
        if (moveOrderList.Count == 0 || moveOrderList.Last() != standPosition)
        {
            moveOrderList.Add(standPosition);
        }
    }

    public List<Transform> GetOrderList() { return moveOrderList; }
    public void OrderAccomplished()
    {
        moveOrderList.RemoveAt(0);
    }
}
