using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform fill;

    private float maxWidth;

    void Awake()
    {
        if (fill == null)
        {
            Debug.LogError("HealthBarUI: 'fill' RectTransform non assegnato!");
            enabled = false; 
            return;
        }
        maxWidth = fill.sizeDelta.x;
    }

    public void SetHealth(float current, float max)
    {
        if (max <= 0f)
        {
            Debug.LogWarning("HealthBarUI: max health <= 0, salto aggiornamento.");
            return;
        }

        float normalized = Mathf.Clamp01(current / max);
        UpdateFillWidth(normalized);
    }

    public void SetHealthNormalized(float normalizedValue)
    {
        normalizedValue = Mathf.Clamp01(normalizedValue);
        UpdateFillWidth(normalizedValue);
    }

    private void UpdateFillWidth(float normalizedValue)
    {
        float width = maxWidth * normalizedValue;
        fill.sizeDelta = new Vector2(width, fill.sizeDelta.y);
    }
}
