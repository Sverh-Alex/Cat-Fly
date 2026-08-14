using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextPulse : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float speed = 2f;

    private RectTransform rectTransform;
    private Text legacyText;
    private TextMeshProUGUI tmpText;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        legacyText = GetComponent<Text>();
        tmpText = GetComponent<TextMeshProUGUI>();

        Debug.Log($"[TextPulse] legacyText: {legacyText != null}");
        Debug.Log($"[TextPulse] tmpText: {tmpText != null}");
    }

    void Start()
    {
        StartCoroutine(Pulse());
    }

    private System.Collections.IEnumerator Pulse()
    {
        float currentScale = 1f;
        float direction = 1f;

        while (true)
        {
            currentScale += direction * speed * Time.deltaTime;

            if (currentScale >= maxScale)
            {
                currentScale = maxScale;
                direction = -1f;
            }
            else if (currentScale <= minScale)
            {
                currentScale = minScale;
                direction = 1f;
            }

            // Если Legacy Text — меняем fontSize
            if (legacyText != null)
            {
                int fontSizeInt = Mathf.RoundToInt(currentScale * 100);
                legacyText.fontSize = fontSizeInt;
                Debug.Log($"[TextPulse] Legacy fontSize: {legacyText.fontSize}");
            }
            // Если TextMeshPro — меняем fontSize
            else if (tmpText != null)
            {
                tmpText.fontSize = currentScale * 100;
                Debug.Log($"[TextPulse] TMP fontSize: {tmpText.fontSize}");
            }

            yield return null;
        }
    }
}