using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    private GameController gameController;
    public float speed = 10f;
    Rigidbody rb;

    private void Awake()
    {
        var gc = GameObject.FindWithTag("GameController");
        if (null == gc)
        {
            Debug.LogError("[ProjectileMovement] GameController missing");
        }
        else
        {
            gameController = gc.GetComponent<GameController>();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    void Update()
    {
        if (gameController.isPaused == false)
        {
            rb.velocity = transform.forward * speed;
        }
        else
        {
            rb.velocity = transform.forward * 0f;
        }
    }
}
