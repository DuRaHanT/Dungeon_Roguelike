using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour, IDamageable, IHealth
{
    public TileData tileData;

    int health;

    public int Health 
    {
        get => health;
        set => health = value;
    }
    
    public void SetHealth(int health)
    {
        this.health = health;
        Debug.Log($"Health set to: {health}");
    }

    public void TakeDamge(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Debug.Log("Player Dead");
        }
    }
}