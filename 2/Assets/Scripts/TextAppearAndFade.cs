using System.Collections;
using UnityEngine;
using TMPro;

public class MultiTextAppearAndFade : MonoBehaviour
{
    [Header("До 10 текстов (TextMeshProUGUI)")]
    [SerializeField] private TextMeshProUGUI[] texts = new TextMeshProUGUI[10];

    [Header("Настройки времени (в секундах)")]
    [SerializeField] private float delayBeforeAwake = 0f;    // Задержка перед началом появления
    [SerializeField] private float fadeInDuration = 1f;       // Длительность появления
    [SerializeField] private float delayBeforeFadeOut = 0.5f; // Пауза после появления
    [SerializeField] private float fadeOutDuration = 1f;      // Длительность затухания

    [Header("Размер шрифта")]
    [SerializeField] private float minFontSize = 20f;         // Стартовый размер
    [SerializeField] private float maxFontSize = 40f;         // Конечный размер

    [Header("Автостарт эффекта")]
    [SerializeField] private bool playOnStart = true;

    private Coroutine currentRoutine;

    private void Start()
    {
        // Инициализация всех текстов
        for (int i = 0; i < texts.Length; i++)
        {
            var tmp = texts[i];
            if (tmp == null) continue; // этот слот не используется

            // Отключаем авторазмер, чтобы управлять fontSize вручную
            tmp.enableAutoSizing = false;

            // Начальное состояние: минимальный размер, прозрачный
            tmp.fontSize = minFontSize;
            Color c = tmp.color;
            c.a = 0f;
            tmp.color = c;
        }

        if (playOnStart)
        {
            PlayEffect();
        }
    }

    /// <summary>
    /// Запустить эффект для всех назначенных текстов.
    /// </summary>
    public void PlayEffect()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(AppearAndFadeRoutine());
    }

    private IEnumerator AppearAndFadeRoutine()
    {
        // 0. Задержка перед появлением
        if (delayBeforeAwake > 0f)
        {
            yield return new WaitForSeconds(delayBeforeAwake);
        }

        // 1. Появление: альфа 0→1, размер min→max
        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / fadeInDuration);

            for (int i = 0; i < texts.Length; i++)
            {
                var tmp = texts[i];
                if (tmp == null) continue;

                Color col = tmp.color;
                col.a = Mathf.Lerp(0f, 1f, n);
                tmp.color = col;

                tmp.fontSize = Mathf.Lerp(minFontSize, maxFontSize, n);
            }

            yield return null;
        }

        // Зафиксировать финальные значения
        for (int i = 0; i < texts.Length; i++)
        {
            var tmp = texts[i];
            if (tmp == null) continue;

            Color col = tmp.color;
            col.a = 1f;
            tmp.color = col;

            tmp.fontSize = maxFontSize;
        }

        // 2. Пауза перед затуханием
        if (delayBeforeFadeOut > 0f)
        {
            yield return new WaitForSeconds(delayBeforeFadeOut);
        }

        // 3. Затухание: альфа 1→0
        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / fadeOutDuration);

            for (int i = 0; i < texts.Length; i++)
            {
                var tmp = texts[i];
                if (tmp == null) continue;

                Color col = tmp.color;
                col.a = Mathf.Lerp(1f, 0f, n);
                tmp.color = col;
            }

            yield return null;
        }

        // Финальное состояние — прозрачные
        for (int i = 0; i < texts.Length; i++)
        {
            var tmp = texts[i];
            if (tmp == null) continue;

            Color col = tmp.color;
            col.a = 0f;
            tmp.color = col;
        }

        currentRoutine = null;
    }
}