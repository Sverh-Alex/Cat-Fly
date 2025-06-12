using UnityEngine;
using TMPro;
using System;

public class TimerBonus : MonoBehaviour
{
    public GameObject buttonBonus; // Кнопка, которая появляется после окончания таймера
    public GameObject buttonTimerBonus; // Кнопка, которая появляется после окончания таймера
    public TextMeshProUGUI textTimerBonus; // Текст для отображения таймера
    [SerializeField] float cdTime; // Время таймера в секундах (например, 300 для 5 минут)
    float remainingTime; // Сколько осталось времени

    private const string TimerEndTimeKey = "TimerEndTime"; // Ключ для сохранения времени окончания

    private void Start()
    {
        buttonBonus.SetActive(false);
        buttonTimerBonus.SetActive(true);

        if (UnityEngine.PlayerPrefs.HasKey(TimerEndTimeKey))
        {
            string savedEndTimeStr = UnityEngine.PlayerPrefs.GetString(TimerEndTimeKey);
            DateTime endTime = DateTime.Parse(savedEndTimeStr);

            TimeSpan timeLeft = endTime - DateTime.Now;
            remainingTime = (float)timeLeft.TotalSeconds;

            if (remainingTime <= 0)
            {
                // Таймер уже истек
                remainingTime = 0;
                buttonBonus.SetActive(true);
                buttonTimerBonus.SetActive(false);
                UnityEngine.PlayerPrefs.DeleteKey(TimerEndTimeKey);
            }
        }
        else
        {
            // Таймер не запущен, показываем кнопку бонуса
            remainingTime = 0;
            buttonBonus.SetActive(true);
            buttonTimerBonus.SetActive(false);
        }
    }
    public void OnButtonBonus()
    {
        // Запускаем таймер заново
        remainingTime = cdTime;

        // Сохраняем время окончания таймера
        DateTime endTime = DateTime.Now.AddSeconds(remainingTime);
        UnityEngine.PlayerPrefs.SetString(TimerEndTimeKey, endTime.ToString());
        UnityEngine.PlayerPrefs.Save();

        buttonBonus.SetActive(false);
        buttonTimerBonus.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                buttonBonus.SetActive(true);
                buttonTimerBonus.SetActive(false);
                UnityEngine.PlayerPrefs.DeleteKey(TimerEndTimeKey);
            }

            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            textTimerBonus.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
