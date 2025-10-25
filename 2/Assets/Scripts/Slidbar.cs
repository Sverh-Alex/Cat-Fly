using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
 

public class Slidbar : MonoBehaviour
{
    public Slider slider;       // Ссылка на UI слайдер
    public Timer timerScript;   // Ссылка на скрипт Timer с lifeTime

    private float elapsedTime = 0f; // Время, прошедшее с начала раунда

    void Update()
    {
        if (timerScript == null || slider == null || timerScript.lifeTime <= 0) return;

        // Увеличиваем локально отслеживаемое время, ограничивая максимальным lifeTime
        elapsedTime += Time.deltaTime;
        if (elapsedTime > timerScript.lifeTime)
            elapsedTime = timerScript.lifeTime;

        // Обновляем прогресс-бар как отношение прошедшего времени к lifeTime
        slider.value = Mathf.Clamp01(elapsedTime / timerScript.lifeTime);
        Debug.Log($"ProgressBar Value: {slider.value:F3}, Target: {elapsedTime:F3}");
    }

    // Для начала раунда можно добавить метод сброса
    public void StartRound()
    {
        elapsedTime = 0f;
    }
    
 
}
