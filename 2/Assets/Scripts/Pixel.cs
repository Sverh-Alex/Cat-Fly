using Coffee.UIEffects;  // Подключает UIEffect
using UnityEngine;  // Подключает Unity API

public class Pixel : MonoBehaviour
{
    [Header("UIEffect для пикселизации")]
    [SerializeField] private UIEffect[] effects;  // Массив компонентов UIEffect

    [Header("Диапазон Sampling Scale")]
    [SerializeField, Min(0f)] private float minScale = 0f;  // Минимальное значение Sampling Scale
    [SerializeField, Min(0f)] private float maxScale = 5f;  // Максимальное значение Sampling Scale

    [Header("Скорость")]
    [SerializeField, Min(0f)] private float speed = 5f;  // Скорость изменения Sampling Scale

    [Header("Начальное состояние")]
    [SerializeField] private float startScale = 0f;  // Начальное значение Sampling Scale
    [SerializeField] AnimationPulse animationPulse;

    private float currentScale;  // Текущее значение Sampling Scale
    private float targetScale;  // Целевое значение Sampling Scale
    private bool isChanging;  // Выполняется ли изменение значения

    private void Awake()
    {
        currentScale = Mathf.Clamp(startScale, minScale, maxScale);  // Ограничивает начальное значение
        targetScale = currentScale;  // Устанавливает начальную цель
        ApplyScale(currentScale);  // Применяет начальный Sampling Scale
        animationPulse.enabled = false; 
    }

    private void Update()
    {
        if (!isChanging)  // Проверяет, выполняется ли изменение
        {
            return;  // Завершает выполнение метода
        }

        currentScale = Mathf.MoveTowards(
            currentScale,  // Текущее значение
            targetScale,  // Целевое значение
            speed * Time.unscaledDeltaTime  // Изменение за текущий кадр
        );

        ApplyScale(currentScale);  // Применяет новое значение

        if (Mathf.Approximately(currentScale, targetScale))  // Проверяет достижение цели
        {
            currentScale = targetScale;  // Устанавливает точное конечное значение
            ApplyScale(currentScale);  // Применяет конечное значение
            isChanging = false;  // Завершает изменение
        }
    }

    public void EnablePixelation()
    {
        targetScale = maxScale;  // Устанавливает максимальный Sampling Scale
        isChanging = true;  // Запускает изменение
    }

    public void DisablePixelation()
    {
        targetScale = minScale;  // Устанавливает минимальный Sampling Scale
        isChanging = true;  // Запускает изменение
    }

    public void TogglePixelation()
    {
        if (Mathf.Approximately(targetScale, minScale) &&
            Mathf.Approximately(currentScale, minScale))  // Проверяет выключенное состояние
        {
            EnablePixelation();  // Запускает пикселизацию
        }
        else
        {
            DisablePixelation();  // Уменьшает Sampling Scale
        }
    }

    public void SetPixelation(float value)
    {
        targetScale = Mathf.Clamp(value, minScale, maxScale);  // Ограничивает целевое значение
        isChanging = true;  // Запускает изменение
    }

    public void SetSpeed(float newSpeed)
    {
        speed = Mathf.Max(0f, newSpeed);  // Запрещает отрицательную скорость
    }

    private void ApplyScale(float value)
    {
        
        if (effects == null)  // Проверяет массив компонентов
        {
            return;  // Завершает выполнение метода
        }

        foreach (UIEffect effect in effects)  // Перебирает все UIEffect
        {
            if (effect == null)  // Проверяет конкретный компонент
            {
                continue;  // Переходит к следующему компоненту
            }

            effect.samplingScale = value;  // Изменяет только Sampling Scale
        }
    }
    public void EnablePulse()
    {
        if (animationPulse != null)  // Проверяет ссылку
        {
            animationPulse.enabled = true;  // Включает AnimationPulse
        }
    }
}