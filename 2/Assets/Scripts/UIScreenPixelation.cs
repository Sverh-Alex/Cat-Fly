using Coffee.UIEffects; // Подключает UIEffect
using UnityEngine; // Подключает Unity API

public class UIScreenPixelation : MonoBehaviour
{
    [Header("UIEffect для пикселизации")]
    [SerializeField] private UIEffect[] effects; // Компоненты UIEffect, назначенные вручную

    [Header("Настройки")]
    [SerializeField, Range(0f, 5f)] private float startValue = 0f; // Начальное значение Sampling Scale

    private void Awake()
    {
        SetPixelation(startValue); // Применяем начальное значение
    }

    public void SetPixelation(float value)
    {
        value = Mathf.Clamp(value, 0f, 5f); // Ограничиваем значение от 0 до 5

        if (effects == null) // Проверяем массив
        {
            return; // Выходим, если массив не создан
        }

        foreach (UIEffect effect in effects) // Перебираем назначенные UIEffect
        {
            if (effect == null) // Проверяем компонент
            {
                continue; // Пропускаем пустой элемент
            }

            effect.samplingScale = value; // Меняем Sampling Scale
        }
    }

    public void EnablePixelation()
    {
        SetPixelation(5f); // Включаем максимальную пикселизацию
    }

    public void DisablePixelation()
    {
        SetPixelation(0f); // Выключаем пикселизацию
    }
}