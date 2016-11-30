using System;
using UnityEngine;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    public delegate void OnChangeHealth(float health);
    public event OnChangeHealth onChangeHealthEvent;

    public float initHealth = 100;
    public float maxHealth = 100;

    private float health;

    void Start()
    {
        health = initHealth;
    }

    public float Amount {
        get
        {
            return health;
        }
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            onChangeHealthEvent.Invoke(health);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            Amount -= 5;
    }
}

