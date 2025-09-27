using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime = 60f;

    [Header("Lose Panel Stuff")]
    [SerializeField] private int coinsNeeded = 5;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI coinCollectedText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button exitButton;

    [Header("Player")]
    [SerializeField] private GameObject playerObject;

    private bool hasEnded = false;

    void Start()
    {
        if (retryButton != null)
            retryButton.onClick.AddListener(RetryLevel);
        else
            Debug.LogWarning("Timer: retryButton non assegnato!");

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitToMenu);
        else
            Debug.LogWarning("Timer: exitButton non assegnato!");

        if (losePanel != null)
            losePanel.SetActive(false);
        else
            Debug.LogWarning("Timer: losePanel non assegnato!");

        if (timerText == null)
            Debug.LogWarning("Timer: timerText non assegnato!");
    }

    void Update()
    {
        if (hasEnded)
            return;

        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI(remainingTime);
        }
        else
        {
            remainingTime = 0f;
            EndLevelCheck();
        }
    }

    private void UpdateTimerUI(float time)
    {
        if (timerText == null)
            return;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    private void EndLevelCheck()
    {
        if (hasEnded)
            return;

        hasEnded = true;

        if (playerObject == null)
        {
            Debug.LogError("Timer: playerObject non assegnato!");
            return;
        }

        CoinCollection coinScript = playerObject.GetComponent<CoinCollection>();
        int coinsCollected = coinScript != null ? coinScript.GetCoinCount() : 0;

        if (coinsCollected < coinsNeeded)
        {
            Debug.Log("Tempo scaduto! Non abbastanza monete. Game Over.");

            if (losePanel != null)
                losePanel.SetActive(true);

            if (coinCollectedText != null)
                coinCollectedText.text = "Monete: " + coinsCollected;

            if (playerObject.TryGetComponent<ThirdPersonMovement>(out var pc))
                pc.enabled = false;
        }
        else
        {
            Debug.Log("Tempo scaduto! Obiettivo monete raggiunto! Hai quasi vinto!");        }
    }

    private void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
