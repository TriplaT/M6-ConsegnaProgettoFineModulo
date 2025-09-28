using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] private float fallYThreshold = -10f;

    [Header("UI Lose Panel")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI coinCollectedText;

    [Header("Player Reference")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private HealthBarUI healthBarUI;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (playerObject == null) playerObject = gameObject;
        UpdateHealthBar();
    }

    void Update()
    {
        if (!isDead && transform.position.y < fallYThreshold)
            Die();
    }

    public void TakeDamage(float amount)
    {
        if (isDead || amount <= 0f) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (isDead || amount <= 0f) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        UpdateHealthBar();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        if (CheckpointSystem.Instance != null)
            CheckpointSystem.Instance.RespawnPlayer();

        isDead = false;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBarUI != null)
            healthBarUI.SetHealth(currentHealth, maxHealth);
    }

    public void SetHealth(float value)
    {
        currentHealth = value;
        UpdateHealthBar();
    }

    public float CurrentHealth()
    {
        return currentHealth;
    }
}
