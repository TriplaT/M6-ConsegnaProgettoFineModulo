using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ArchTrigger : MonoBehaviour
{
    [Header("Coin Requirements")]
    [SerializeField] private CoinCollection playerCoinCollection;
    [SerializeField] private int coinsNeeded = 1;

    [Header("UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI coinCollectedText;

    [Header("Player Control")]
    [SerializeField] private ThirdPersonMovement playerMovement;

    private bool levelCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!levelCompleted && other.CompareTag("Player"))
            TryCompleteLevel();
    }

    private void TryCompleteLevel()
    {
        int collected = playerCoinCollection.GetCoinCount();

        if (collected > coinsNeeded)
        {
            levelCompleted = true;
            winPanel.SetActive(true);
            coinCollectedText.text = $"Monete: {collected}";

            if (playerMovement != null)
                playerMovement.enabled = false;
            else
                Debug.LogWarning("ThirdPersonMovement non assegnato!");

            Time.timeScale = 0f;
            Debug.Log("Livello completato!");
        }
        else
        {
            Debug.Log("Non hai abbastanza coin!");
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
