using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance;

    public GameObject player;
    public CoinCollection coinCollection;
    public HealthManager healthManager;

    private Vector3 lastCheckpointPos;
    private int savedCoins;
    private float savedHealth;
    private bool hasCheckpoint = false;

    private int consecutiveDeaths = 0;
    private const int maxConsecutiveDeaths = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void SaveCheckpoint(Vector3 pos)
    {
        lastCheckpointPos = pos;
        savedCoins = coinCollection != null ? coinCollection.GetCoinCount() : 0;
        savedHealth = healthManager != null ? healthManager.CurrentHealth() : 0;
        hasCheckpoint = true;
        consecutiveDeaths = 0;
        Debug.Log($"Checkpoint salvato: Monete={savedCoins}, Salute={savedHealth}");
    }

    public void RespawnPlayer()
    {
        consecutiveDeaths++;

        if (!hasCheckpoint || consecutiveDeaths >= maxConsecutiveDeaths)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (player != null) player.transform.position = lastCheckpointPos;

        if (coinCollection != null) coinCollection.SetCoinCount(savedCoins);
        if (healthManager != null) healthManager.SetHealth(savedHealth);

        if (player.TryGetComponent<ThirdPersonMovement>(out var thirdPersonMovement))
        {
            thirdPersonMovement.ResetAnimator();
        }
        else if (player.TryGetComponent<Animator>(out var anim))
        {
            anim.Rebind();
            anim.Update(0f);
            anim.SetFloat("speed", 0f);
            anim.SetBool("isJumping", false);
        }

        Debug.Log($"Respawn al checkpoint. Morti consecutive: {consecutiveDeaths}");
    }
}
