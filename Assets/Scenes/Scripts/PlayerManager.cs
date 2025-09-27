using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private HealthBarUI healthBar;

    void Start()
    {
        health = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        if (Input.GetKeyDown("d"))
        {
            ChangeHealth(-20f);
        }
        else if (Input.GetKeyDown("h"))
        {
            ChangeHealth(20f);
        }
    }

    public void ChangeHealth(float delta)
    {
        health = Mathf.Clamp(health + delta, 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(health, maxHealth);
        }
        else
        {
            Debug.LogWarning("PlayerManager: healthBar non assegnata");
        }
    }
}
