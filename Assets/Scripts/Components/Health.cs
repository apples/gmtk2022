using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int maxHealth;

    public UnityEvent<Health> onChange;

    private int currentHealth;

    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            onChange.Invoke(this);
        }
    }

    void Start()
    {
        CurrentHealth = maxHealth;
    }
}
