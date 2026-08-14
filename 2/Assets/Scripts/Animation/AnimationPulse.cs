using System.Collections;
using UnityEngine;

/// <summary>
/// Пульсация UI-объекта с безопасным повторным запуском
/// после включения объекта.
/// </summary>
public class AnimationPulseReusable : MonoBehaviour
{
    // Минимальный масштаб.
    [SerializeField] private float minScale = 0.9f;

    // Максимальный масштаб.
    [SerializeField] private float maxScale = 1.1f;

    // Скорость пульсации.
    [SerializeField] private float speed = 2f;

    // RectTransform объекта.
    private RectTransform rectTransform;

    // Ссылка на coroutine.
    private Coroutine pulseCoroutine;

    private void Awake()
    {
        // Получаем RectTransform.
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // Запускаем анимацию после каждого включения объекта.
        pulseCoroutine = StartCoroutine(Pulse());
    }

    private void OnDisable()
    {
        // Останавливаем текущую coroutine.
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
            pulseCoroutine = null;
        }

        // Возвращаем исходный масштаб.
        rectTransform.localScale = Vector3.one;
    }

    private IEnumerator Pulse()
    {
        // Текущий масштаб.
        float currentScale = 1f;

        // Направление изменения масштаба.
        float direction = 1f;

        // Бесконечный цикл до отключения объекта.
        while (true)
        {
            // Используем время, независимое от паузы.
            currentScale += direction * speed * Time.unscaledDeltaTime;

            // Обрабатываем верхнюю границу.
            if (currentScale >= maxScale)
            {
                currentScale = maxScale;
                direction = -1f;
            }
            // Обрабатываем нижнюю границу.
            else if (currentScale <= minScale)
            {
                currentScale = minScale;
                direction = 1f;
            }

            // Применяем масштаб.
            rectTransform.localScale = Vector3.one * currentScale;

            // Переходим к следующему кадру.
            yield return null;
        }
    }
}