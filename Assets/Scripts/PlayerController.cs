using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0f;
    public float maxSpeed = 0f;
    public float decreaseFuelAmount = 0f;

    private Rigidbody2D myRigidbody;
    private PlayerManager playerManager;

    void Awake()
    {
        myRigidbody = transform.GetComponent<Rigidbody2D>();
        playerManager = transform.GetComponent<PlayerManager>();
    }

    // Player Inputs
    private void FixedUpdate()
    {
        if (!Input.anyKey || playerManager.fuel <= 0)
        {
            MovePlayer(Vector2.zero.normalized);
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                MovePlayer(Vector2.up.normalized);
            }
            if (Input.GetKey(KeyCode.S))
            {
                MovePlayer(Vector2.down.normalized);
            }
            if (Input.GetKey(KeyCode.D))
            {
                MovePlayer(Vector2.right.normalized);
            }
            if (Input.GetKey(KeyCode.A))
            {
                MovePlayer(Vector2.left.normalized);
            }
        }
    }

    void Update()
    {
        CurrectPlayerPosition();
    }

    void MovePlayer(Vector2 direction)
    {
        if (direction == Vector2.zero.normalized)
        {
            myRigidbody.velocity -= myRigidbody.velocity / 8f;
        }
        else if (myRigidbody.velocity.magnitude >= maxSpeed)
        {
            myRigidbody.AddForce(-myRigidbody.velocity.normalized * moveSpeed);
        }
        else 
        {
            myRigidbody.AddForce(direction * moveSpeed);
            playerManager.DecreaseFuel(decreaseFuelAmount);
        }
    }

    private void CurrectPlayerPosition()
    {
        var pos = Camera.main.WorldToScreenPoint(transform.position);
        if (pos.x > (Screen.safeArea.xMax))
        {
            Vector2 newpos = new(Screen.safeArea.xMin, pos.y);
            transform.position = new Vector2(Camera.main.ScreenToWorldPoint(newpos).x, transform.position.y);
        }
        if (pos.x < Screen.safeArea.xMin)
        {
            Vector2 newpos = new(Screen.safeArea.xMax, pos.y);
            transform.position = new Vector2(Camera.main.ScreenToWorldPoint(newpos).x, transform.position.y);
        }
        if (pos.y > Screen.safeArea.yMax)
        {
            Vector2 newpos = new(pos.x, Screen.safeArea.yMin);
            transform.position = new Vector2(transform.position.x, Camera.main.ScreenToWorldPoint(newpos).y);
        }
        if (pos.y < Screen.safeArea.yMin)
        {
            Vector2 newpos = new(pos.x, Screen.safeArea.yMax);
            transform.position = new Vector2(transform.position.x, Camera.main.ScreenToWorldPoint(newpos).y);
        }
    }
}
