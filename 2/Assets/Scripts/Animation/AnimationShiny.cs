using UnityEngine;
using Coffee.UIExtensions;

public class ShinyEffectAnimator : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private ShinyEffectForUGUI shinyEffect;  // Компонент ShinyEffectForUGUI
    [SerializeField] private float speed = 1f;  // Скорость анимации (полный цикл от 1 до 0)
    [SerializeField] private float startLocation = 1f;  // Начальное значение location (1 = справа, 0 = слева)

    private bool isAnimating;

    private void Awake()
    {
        // Если эффект не назначен, ищем на этом же объекте
        if (shinyEffect == null)
        {
            shinyEffect = GetComponent<ShinyEffectForUGUI>();
        }

        if (shinyEffect == null)
        {
            Debug.LogError("[ShinyEffectAnimator] ОШИБКА: ShinyEffectForUGUI не найден!");
            return;
        }

        // Устанавливаем начальное значение
        shinyEffect.location = startLocation;
    }

    private void OnEnable()
    {
        // Запускаем анимацию при включении объекта
        StartAnimation();
    }

    private void OnDisable()
    {
        // Останавливаем анимацию при выключении
        StopAnimation();
    }

    // Запускает зацикленную анимацию
    public void StartAnimation()
    {
        if (isAnimating)
        {
            return;
        }

        isAnimating = true;
        StartCoroutine(AnimateLoop());
        Debug.Log("[ShinyEffectAnimator] Анимация запущена");
    }

    // Останавливает анимацию
    public void StopAnimation()
    {
        isAnimating = false;
        StopAllCoroutines();
        Debug.Log("[ShinyEffectAnimator] Анимация остановлена");
    }

    // Зацикленная анимация от 1 до 0
    private System.Collections.IEnumerator AnimateLoop()
    {
        float currentLocation = startLocation;

        while (isAnimating)
        {
            // Двигаем от текущего значения до 0
            while (isAnimating && currentLocation > 0f)
            {
                currentLocation -= speed * Time.deltaTime;
                currentLocation = Mathf.Max(0f, currentLocation);  // Не меньше 0

                shinyEffect.location = currentLocation;

                yield return null;
            }

            // Сбрасываем на 1 и начинаем заново
            if (isAnimating)
            {
                currentLocation = 1f;
                shinyEffect.location = currentLocation;
            }
        }
    }

    // Устанавливает скорость анимации
    public void SetSpeed(float newSpeed)
    {
        speed = Mathf.Max(0.01f, newSpeed);
        Debug.Log($"[ShinyEffectAnimator] Скорость изменена: {speed}");
    }

    // Устанавливает начальное значение
    public void SetStartLocation(float location)
    {
        startLocation = Mathf.Clamp(location, 0f, 1f);
        shinyEffect.location = startLocation;
        Debug.Log($"[ShinyEffectAnimator] Начальное значение: {startLocation}");
    }
}