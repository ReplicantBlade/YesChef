using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpaceObject : MonoBehaviour
{
    public enum Type
    {
        AsteroidSmall,
        AsteroidMedium,
        AsteroidBig,
        HealthPoint,
        Bullet,
        Fuel
    }

    public Type type = new();

    private int objectScore = 0;
    private int objectDamage = 0;
    private int objectHealthPoint = 0;
    private int objectBulletPoint = 0;
    private float objectFuelPoint = 0f;

    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        switch (type)
        {
            case Type.AsteroidSmall:
                objectScore = 10;
                objectDamage = 10;
                break;
            case Type.AsteroidMedium:
                objectScore = 30;
                objectDamage = 20;
                break;
            case Type.AsteroidBig:
                objectScore = 50;
                objectDamage = 30;
                break;
            case Type.HealthPoint:
                objectHealthPoint = 20;
                break;
            case Type.Bullet:
                objectBulletPoint = 50;
                break;
            case Type.Fuel:
                objectFuelPoint = 30f;
                break;
            default:
                break;
        }
    }

    public void SetPlayerManager(PlayerManager playermanager)
    {
        playerManager = playermanager;
    }

    public void SetGameManager(GameManager gamemanager)
    {
        gameManager = gamemanager; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerManager.DecreaseHealth(objectDamage);
            playerManager.IncreaseHealth(objectHealthPoint);
            playerManager.IncreaseBullet(objectBulletPoint);
            playerManager.IncreaseFuel(objectFuelPoint);
            gameManager.CheckPlayerHealth();
            Destroy(gameObject);
        }
        else if (collision.transform.tag == "Bullet")
        {
            if (type.Equals(Type.AsteroidSmall) || type.Equals(Type.AsteroidMedium) || type.Equals(Type.AsteroidBig))
            {
                playerManager.IncreaseScore(objectScore);
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
        }
    }
}
