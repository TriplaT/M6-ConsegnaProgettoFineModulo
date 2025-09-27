using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;

    private void Awake()
    {
        if (pointsText == null)
            Debug.LogWarning("GameOverScreen: pointsText non assegnato!");
        gameObject.SetActive(false); 
    }

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = "Monete: " + score;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
