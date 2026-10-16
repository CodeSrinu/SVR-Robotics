using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public event Action<float> Damaged;
    public event Action<float, float> Changed;
    public event Action Dead;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead { get; private set; }

    private void Awake()
    {
        ResetHealth();
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        if(currentHealth - damage > 0)
        {
            currentHealth -= damage;
            Damaged?.Invoke(damage);
            Changed?.Invoke(currentHealth, maxHealth);
        }
        else
        {
            currentHealth = 0;
            IsDead = true;
            Damaged?.Invoke(damage);  
            Changed?.Invoke(currentHealth, maxHealth);
            Dead?.Invoke();
        }
    }

    public void Initialize(float health)
    {
        maxHealth = health;
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        Changed?.Invoke(currentHealth, maxHealth);
        IsDead = false;
    }
}
