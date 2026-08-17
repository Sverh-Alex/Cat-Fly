using System.Collections;                         // Подключает Coroutine
using UnityEngine;                                // Подключает основные классы Unity
using UnityEngine.UI;                             // Подключает компонент Image

public class ButtonClose : MonoBehaviour
{
    [SerializeField] private GameObject image;    // Объект вкладки
    [SerializeField] private Image[] images;      // Массив изображений
    [SerializeField] private float scaleDuration = 0.5f;  // Время уменьшения в секундах

    private Coroutine scaleCoroutine;             // Текущая Coroutine

    public void CloseTab()
    {
        image.SetActive(false);                   // Отключает объект вкладки
    }

    public void OpenTab()
    {
        image.SetActive(true);                    // Включает объект вкладки
    }

    public void CloseImg()
    {
        foreach (Image currentImage in images)    // Перебирает все изображения
        {
            if (currentImage == null)             // Проверяет, назначено ли изображение
            {
                continue;                          // Переходит к следующему изображению
            }

            currentImage.enabled = false;         // Отключает текущее изображение
        }
    }

    public void ScaleMinImg()
    {
        if (scaleCoroutine != null)               // Проверяет, выполняется ли анимация
        {
            StopCoroutine(scaleCoroutine);       // Останавливает предыдущую анимацию
        }

        scaleCoroutine = StartCoroutine(ScaleImagesToZero());  // Запускает уменьшение
    }

    public void OpenImages()
    {
        foreach (Image currentImage in images)    // Перебирает все изображения
        {
            if (currentImage == null)             // Проверяет, назначено ли изображение
            {
                continue;                          // Переходит к следующему изображению
            }

            currentImage.enabled = true;          // Включает текущее изображение
            currentImage.rectTransform.localScale = Vector3.one;  // Возвращает масштаб
        }
    }

    private IEnumerator ScaleImagesToZero()
    {
        Vector3[] startScales = new Vector3[images.Length];  // Массив начальных масштабов

        for (int i = 0; i < images.Length; i++)             // Перебирает изображения
        {
            if (images[i] == null)                          // Проверяет изображение
            {
                continue;                                    // Переходит к следующему
            }

            startScales[i] = images[i].rectTransform.localScale;  // Сохраняет масштаб
        }

        float elapsedTime = 0f;                             // Прошедшее время анимации

        while (elapsedTime < scaleDuration)                 // Выполняет анимацию
        {
            elapsedTime += Time.unscaledDeltaTime;          // Учитывает время без паузы

            float progress = elapsedTime / scaleDuration;   // Рассчитывает прогресс

            for (int i = 0; i < images.Length; i++)         // Перебирает изображения
            {
                if (images[i] == null)                      // Проверяет изображение
                {
                    continue;                                // Переходит к следующему
                }

                images[i].rectTransform.localScale = Vector3.Lerp(
                    startScales[i],                         // Начальный масштаб
                    Vector3.zero,                            // Конечный масштаб
                    progress                                 // Прогресс анимации
                );
            }

            yield return null;                              // Ждёт следующий кадр
        }

        for (int i = 0; i < images.Length; i++)             // Перебирает изображения
        {
            if (images[i] == null)                          // Проверяет изображение
            {
                continue;                                    // Переходит к следующему
            }

            images[i].rectTransform.localScale = Vector3.zero;  // Устанавливает нулевой масштаб
        }

        scaleCoroutine = null;                              // Очищает ссылку на Coroutine
    }
}