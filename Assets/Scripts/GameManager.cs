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
    public enum StateType
    {
        Wave,
        EndWave,
        Win,
        Lose
    };
    public StateType state { get; set; } = new StateType();
    public int totalWaveCount;
    public float waveDuration;
    public float waveDifficultyStepUp;
    public float changeWaveDuration;
    private float waveDifficulty = 1f;
    private int currentWaveNumber = 1;

    public int maxAsteroidToSpawn;
    public int maxBulletToSpawn;
    public int maxHealthPointsToSpawn;
    public int maxFuelToSpawn;

    public GameObject player;

    public List<Transform> spawnLocations = new();
    public List<GameObject> Asteroids = new();
    public List<GameObject> Bullets = new();
    public List<GameObject> HealthPoints = new();
    public List<GameObject> Fuel = new();

    private List<string> allSpaceTypes = new List<string>();
    private UIManager UIManager;
    private PlayerManager playerManager;

    private float waveCooldownTimestamp;
    private float endWaveCooldownTimestamp;

    void Start()
    {
        UIManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        player = GameObject.FindWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        waveCooldownTimestamp = Time.time + waveDuration;
        UIManager.SetTotalWave(totalWaveCount.ToString());
        ResetStage();
    }

    void Update()
    {
        switch (state)
        {
            case StateType.Wave:
                WaveSpawnState();
                break;
            case StateType.EndWave:
                EndWaveState();
                break;
            case StateType.Win:
                WinGameState();
                break;
            case StateType.Lose:
                LoseGameState();
                break;
            default:
                break;
        }
    }

    float cooldown = 0f;
    float cooldownTimestamp;
    private void SpawnSpaceObjects(float difficulty)
    {
        if (Time.time < cooldownTimestamp) return;

        float minSpeedOfObject = 1f * difficulty, maxSpeedOfObject = minSpeedOfObject * 2f * difficulty;

        Transform spawnLocation = GetRandomSpawnLocation();
        GameObject spaceObject = GetSpaceObjectToSpawn();
        spaceObject.GetComponent<SpaceObject>().SetPlayerManager(playerManager);
        spaceObject.GetComponent<SpaceObject>().SetGameManager(this);

        var spaceObjectInstance = Instantiate(spaceObject, spawnLocation.transform);
        spaceObjectInstance.GetComponent<Rigidbody2D>().velocity = spawnLocation.transform.up * Random.Range(minSpeedOfObject, maxSpeedOfObject);
        spaceObjectInstance.GetComponent<Rigidbody2D>().AddTorque(Random.Range(0f,1.5f), ForceMode2D.Impulse);
        Destroy(spaceObjectInstance, 30);

        float minSpawnTimeOfObject = 1f / difficulty, maxSpawnTimeOfObject = minSpawnTimeOfObject * 2f / difficulty;

        cooldownTimestamp = Time.time + cooldown;
        cooldown = Random.Range(minSpawnTimeOfObject, maxSpawnTimeOfObject);
    }

    private Transform GetRandomSpawnLocation()
    {
        return spawnLocations[Random.Range(0, spawnLocations.Count)];
    }

    private int maxAsteroid, maxBullet, maxHealthPoints, maxFuel;
    
    private GameObject GetSpaceObjectToSpawn()
    {
        if (allSpaceTypes.Count > 0)
        {
            List<GameObject> choosenList = null;

            while (choosenList == null)
            {
                int index = Random.Range(0, allSpaceTypes.Count);
                switch (allSpaceTypes[index])
                {
                    case "Asteroids":
                        choosenList = Asteroids;
                        if (maxAsteroid <= 0) { allSpaceTypes.RemoveAt(index); break; };
                        maxAsteroid = maxAsteroid-- < 0 ? 0 : maxAsteroid--;
                        break;

                    case "Bullets":
                        choosenList = Bullets;
                        if (maxBullet <= 0) { allSpaceTypes.RemoveAt(index); break; };
                        maxBullet = maxBullet-- < 0 ? 0 : maxBullet--;
                        break;

                    case "HealthPoints":
                        choosenList = HealthPoints;
                        if (maxHealthPoints <= 0) { allSpaceTypes.RemoveAt(index); break; };
                        maxHealthPoints = maxHealthPoints-- < 0 ? 0 : maxHealthPoints--;
                        break;

                    case "Fuel":
                        choosenList = Fuel;
                        if (maxFuel <= 0) { allSpaceTypes.RemoveAt(index); break; };
                        maxFuel = maxFuel-- < 0 ? 0 : maxFuel--;
                        break;

                    default:
                        choosenList = null;
                        break;
                }
            }

            return choosenList[Random.Range(0, choosenList.Count)];
        }
        //If there was nothing to spawn just finish the wave with one healPoint
        else { state = StateType.EndWave; return HealthPoints[0]; }
    }

    private void ResetStage()
    {
        allSpaceTypes.Add("Asteroids");
        allSpaceTypes.Add("Bullets");
        allSpaceTypes.Add("HealthPoints");
        allSpaceTypes.Add("Fuel");
        maxAsteroid = maxAsteroidToSpawn - 1;
        maxBullet = maxBulletToSpawn - 1;
        maxHealthPoints = maxHealthPointsToSpawn - 1;
        maxFuel = maxFuelToSpawn - 1;
    }

    private bool IsWaveChanged()
    {
        if (Time.time > waveCooldownTimestamp)
        {
            IncreaseDifficulty();
            state = StateType.EndWave;
            endWaveCooldownTimestamp = Time.time + changeWaveDuration;
            waveCooldownTimestamp = Time.time + waveDuration + changeWaveDuration;
            playerManager.IncreaseScore(10 * (int)waveDifficulty);
            return true;
        }
        return false;
    }

    private bool HasGameFinished()
    {
        return currentWaveNumber > totalWaveCount;
    }

    private void IncreaseDifficulty()
    {
        waveDifficulty += waveDifficultyStepUp;
        currentWaveNumber++;
        UIManager.SetCurrentWave(currentWaveNumber.ToString());
        maxAsteroidToSpawn += 2;
    }

    private void WaveSpawnState()
    {
        if (!IsWaveChanged())
        {
            SpawnSpaceObjects(waveDifficulty);
        }
    }

    private void EndWaveState()
    {
        if (HasGameFinished())
        {
            state = StateType.Win;
        }
        if (Time.time > endWaveCooldownTimestamp)
        {
            state = StateType.Wave;
            ResetStage();
        }
    }

    private void WinGameState()
    {
        player.SetActive(false);
        UIManager.InitialEndGamePanel("Victory" , playerManager.GetScore());
    }

    private void LoseGameState()
    {
        player.SetActive(false);
        UIManager.InitialEndGamePanel("Lose", playerManager.GetScore());
    }

    public void CheckPlayerHealth()
    {
        if (playerManager.GetHealth() <= 0) { state = StateType.Lose; }
    }
}
