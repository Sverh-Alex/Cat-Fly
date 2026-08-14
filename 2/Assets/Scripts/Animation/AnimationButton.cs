using System.Collections;
using UnityEngine;

public class AnimationButton : MonoBehaviour
{
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 1.1f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float currentScale = 1f;

    private RectTransform rectTransform;
    private bool isAnimating = false;  // Флаг: идёт ли анимация
    private float targetScale;          // Целевой scale (куда стремимся)

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        currentScale = rectTransform.localScale.x;  // Берём текущий scale из объекта
    }

    // Увеличить кнопку
    public void BtnMax()
    {
        targetScale = maxScale;  // Целевой scale = максимальный
        if (!isAnimating)
        {
            StartCoroutine(AnimateScale());
        }
    }

    // Уменьшить кнопку
    public void BtnMin()
    {
        targetScale = minScale;  // Целевой scale = минимальный
        if (!isAnimating)
        {
            StartCoroutine(AnimateScale());
        }
    }

    private System.Collections.IEnumerator AnimateScale()
    {
        isAnimating = true;  // Помечаем, что анимация идёт

        while (true)
        {
            // Определяем направление
            float direction = 0f;

            if (currentScale < targetScale)
            {
                direction = 1f;  // Увеличиваем
            }
            else if (currentScale > targetScale)
            {
                direction = -1f;  // Уменьшаем
            }
            else
            {
                // Достигли цели
                isAnimating = false;
                yield break;  // Выходим из корутины
            }

            // Меняем scale
            currentScale += direction * speed * Time.deltaTime;

            // Ограничиваем, чтобы не вышло за границы
            currentScale = Mathf.Clamp(currentScale, minScale, maxScale);

            // Применяем
            rectTransform.localScale = new Vector3(currentScale, currentScale, currentScale);

            yield return null;
        }
    }
}