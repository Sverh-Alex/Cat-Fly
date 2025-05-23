using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TextFade : MonoBehaviour
{
    [SerializeField] private Graphic[] uiElements = new Graphic[10]; // Массив до 10 UI элементов
    [SerializeField] private float fadeDuration = 2f;                // Время затухания
    [SerializeField] private float delayBeforeFade = 3f;             // Задержка перед началом затухания

    void Start()
    {
        // Устанавливаем всем элементам полную непрозрачность
        foreach (var element in uiElements)
        {
            if (element != null)
                element.canvasRenderer.SetAlpha(1f);
        }

        // Запускаем затухание с задержкой
        Invoke(nameof(StartFadeOut), delayBeforeFade);
    }

    private void StartFadeOut()
    {
        foreach (var element in uiElements)
        {
            if (element != null)
                element.CrossFadeAlpha(0f, fadeDuration, false);
        }

        // Удаляем объект после окончания затухания (если нужно)
        Destroy(gameObject, fadeDuration);
    }
}
