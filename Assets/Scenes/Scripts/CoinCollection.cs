using TMPro;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    private int coinCount = 0;

    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        if (coinText == null)
            Debug.LogWarning("CoinCollection: coinText non assegnato!");
        else
            UpdateCoinUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            UpdateCoinUI();
            Debug.Log($"Monete raccolte: {coinCount}");
            Destroy(other.gameObject);
        }
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coin Collected: " + coinCount;
    }

    public int GetCoinCount()
    {
        return coinCount;
    }
    public void SetCoinCount(int count)
    {
        coinCount = count;
        UpdateCoinUI();
    }

}
