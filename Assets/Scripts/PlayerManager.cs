using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    private int score = 0;
    [HideInInspector]
    public float fuel = 0f;
    public float maxFuel = 0f;

    private int health = 0;
    public int maxHealth = 0;

    private int bullet = 0;
    public int maxBullet = 0;
    public float bulletSpeed = 0f;

    private PlayerController playerController;
    private Rigidbody2D myRigidbody;
    private Collider2D myCollider;
    private UIManager UIManager;

    private void Start()
    {
        playerController = transform.GetComponent<PlayerController>();
        myCollider = transform.GetComponent<Collider2D>();
        myRigidbody = transform.GetComponent<Rigidbody2D>();
        UIManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        bullet = maxBullet;
        fuel = maxFuel;
        health = maxHealth;
        InitUIElements(fuel, health);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void DecreaseFuel(float amount)
    {
        fuel = fuel - amount < 0 ? 0 : fuel - amount;
        UIManager.SetFuelSlider(-1, fuel);
    }

    public void IncreaseFuel(float amount)
    {
        fuel = fuel + amount > maxFuel ? maxFuel : fuel + amount;
        UIManager.SetFuelSlider(-1, fuel);
    }

    public void DecreaseHealth(int amount)
    {
        health = health - amount < 0 ? 0 : health - amount;
        UIManager.SetHealthSlider(-1, health);
    }

    public void IncreaseHealth(int amount)
    {
        health = health + amount > maxHealth ? maxHealth : health + amount;
        UIManager.SetHealthSlider(-1, health);
    }

    public void DecreaseBullet(int amount)
    {
        bullet = bullet - amount < 0 ? 0 : bullet - amount;
        UIManager.SetBulletAmount(bullet.ToString());
    }

    public void IncreaseBullet(int amount)
    {
        bullet = bullet + amount > maxBullet ? maxBullet : bullet + amount;
        UIManager.SetBulletAmount(bullet.ToString());
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    public int GetScore()
    {
        return score;
    }

    float cooldown = 0.2f;
    float cooldownTimestamp;
    private void Fire()
    {
        if (Time.time < cooldownTimestamp || bullet <= 0) return;
        var bulletInstance = Instantiate(bulletPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

        Vector2 bulletVelocity = myRigidbody.transform.up * bulletSpeed;
        bulletVelocity += myRigidbody.velocity * 1.0f;
        bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletVelocity;

        Destroy(bulletInstance,3);
        DecreaseBullet(1);
        cooldownTimestamp = Time.time + cooldown;
    }
    
    private void InitUIElements(float fuel ,int health)
    {
        UIManager.SetFuelSlider(fuel, fuel);
        UIManager.SetHealthSlider(health, health);
        UIManager.SetBulletAmount(bullet.ToString());
    }
}
