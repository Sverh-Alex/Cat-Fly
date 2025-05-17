using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static event Action OnCoinsChanged; // Событие, которое уведомляет подписчиков об изменении количества монет

    [SerializeField] private TextMeshProUGUI score; // UI текст для отображения монет
    [SerializeField] private int addBonus = 0; // бонус за обычное действие
    [SerializeField] private int addBonusADS = 0; // бонус за просмотр рекламы
    [SerializeField] private GameObject effectPSClick; // эффект при клике
    [SerializeField] private GameObject effectPSCoin; // эффект монеток

    public static void SendCoinsChanged()
    {
        OnCoinsChanged.Invoke();

    }
    void Start()
    {
        effectPSCoin.SetActive(false); // изначально эффект монеток выключен

        int coins = PlayerPrefs.GetInt("coins"); // Получаем текущее количество монет из сохранений
        score.text = coins.ToString();
                
    }
    private void Test()
    {
        Debug.Log("Передал сообщение");
       
    }
    private void UpdateCoins(int newCoins) // Вспомогательный метод обновления монет и вызова события
    {
        PlayerPrefs.SetInt("coins", newCoins); // сохраняем новое значение монет
        score.text = newCoins.ToString(); // обновляем UI
       // OnCoinsChanged?.Invoke(newCoins); // вызываем событие для подписчиков
    }

    public void AddToScore() // Добавляет 1 монету и обновляет UI и событие
    {
        int coins = PlayerPrefs.GetInt("coins") + 1;
        UpdateCoins(coins);
    }

    public void AddBonus() // Добавляет бонусные монеты (например, за действие)
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonus;
        UpdateCoins(coins);

        Instantiate(effectPSClick); // создаём эффект клика
        effectPSCoin.SetActive(true); // включаем эффект монеток
    }

    public void AddBonusADS() // Добавляет бонусные монеты за просмотр рекламы
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonusADS;
        UpdateCoins(coins);

        Instantiate(effectPSClick); // создаём эффект клика
        effectPSCoin.SetActive(true); // включаем эффект монеток
    }
}
