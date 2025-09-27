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
        if (healthBarUI == null) Debug.LogWarning("HealthManager: healthBarUI non assegnato!");
        if (losePanel == null) Debug.LogWarning("HealthManager: losePanel non assegnato!");
        if (coinCollectedText == null) Debug.LogWarning("HealthManager: coinCollectedText non assegnato!");
        if (playerObject == null) playerObject = gameObject;

        UpdateHealthBar();
    }

    void Update()
    {
        if (!isDead && transform.position.y < fallYThreshold)
        {
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead || amount <= 0f) return;  

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead || amount <= 0f) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBarUI != null)
        {
            healthBarUI.SetHealth(currentHealth, maxHealth);
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0f;
        UpdateHealthBar();

        if (losePanel != null)
        {
            losePanel.SetActive(true);

            if (playerObject != null && playerObject.TryGetComponent<CoinCollection>(out var coinScript))
            {
                int coins = coinScript.GetCoinCount();
                if (coinCollectedText != null)
                {
                    coinCollectedText.text = "Monete: " + coins;
                }
            }
        }

        if (playerObject != null && playerObject.TryGetComponent<ThirdPersonMovement>(out var pc))
        {
            pc.enabled = false;
        }
    }
}
