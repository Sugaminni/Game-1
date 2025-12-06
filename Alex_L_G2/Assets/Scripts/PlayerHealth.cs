using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;    
    public float currentHealth;       

    [Header("Optional")]
    public bool destroyOnDeath = false;

    public Action<float, float> OnHealthChanged;
    public Action OnDied;

    // Properties to access health values
    public float currentHP { get => currentHealth; set { currentHealth = value; Notify(); } }
    public float maxHP     { get => maxHealth;     set { maxHealth     = value; Notify(); } }

    // Initialize health
    void Awake()
    {
        if (currentHealth <= 0f) currentHealth = maxHealth;
        Notify();
    }

    // Handle taking damage
    public void TakeDamage(float amount)
    {
        if (amount <= 0f || currentHealth <= 0f) return;
        currentHealth = Mathf.Max(0f, currentHealth - amount);
        Notify();
        if (currentHealth <= 0f) Die();
    }

    // Handle healing
    public void Heal(float amount)
    {
        if (amount <= 0f || currentHealth <= 0f) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        Notify();
    }

    // Handle death
    void Die()
    {
        OnDied?.Invoke();
        if (destroyOnDeath) Destroy(gameObject);
    }

    void Notify() => OnHealthChanged?.Invoke(currentHealth, maxHealth);
}
