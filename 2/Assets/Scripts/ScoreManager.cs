using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static UnityEngine.Rendering.DebugUI;

public class ScoreManager : MonoBehaviour
{
    public static event Action OnCoinsChanged; // Событие, которое уведомляет подписчиков об изменении количества монет
    public static event Action OnAlive; // Событие, которое уведомляет подписчиков о проолжении игры
    public static event Action OnTutorWeb; // Событие, о включеной клавиатуре
    public static event Action OnTutorApp; // Событие, о включенном джостике

    [SerializeField] private TextMeshProUGUI score; // UI текст для отображения монет
    [SerializeField] public int addBonus = 0; // бонус за обычное действие
    [SerializeField] private int addBonusADS = 0; // бонус за просмотр рекламы
    [SerializeField] private GameObject effectPSClick; // эффект при клике
    [SerializeField] private GameObject effectPSCoin; // эффект монеток
    [SerializeField] private TextMeshProUGUI isAddBonusText; // UI текст для отображения монет
    [SerializeField] private GameObject revard; // меню получения х2 монеток
    [SerializeField] private TextMeshProUGUI textCatCoinValue; // текст монет полученных за уровень


    public static void SendCoinsChanged()
    {
        OnCoinsChanged?.Invoke();
    
    }
    public static void SendContinue()
    {
        OnAlive.Invoke();

    }
    public static void SendTutorialWeb()
    {
        OnTutorWeb.Invoke();

    }
    public static void SendTutorialApp()
    {
        OnTutorApp.Invoke();

    }
    void Start()
    {
        revard.SetActive(false);
        effectPSCoin.SetActive(false); // изначально эффект монеток выключен
        
        int coins = PlayerPrefs.GetInt("coins"); // Получаем текущее количество монет из сохранений
        score.text = coins.ToString();
        isAddBonusText.text = $"+{addBonus}";

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
        ScoreManager.SendCoinsChanged();
    }

    public void AddBonus() // Добавляет бонусные монеты (например, за действие)
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonus;
        UpdateCoins(coins);

        EffectClick();
        effectPSCoin.SetActive(true); // включаем эффект монеток
        ScoreManager.SendCoinsChanged();
    }

    public void AddBonusADS() // Добавляет бонусные монеты за просмотр рекламы
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonusADS;
        UpdateCoins(coins);

        effectPSCoin.SetActive(true); // включаем эффект монеток

        EffectClick();

        SendCoinsChanged();
    }
    public void OnOpenX2()
    {
        if (revard)
        {
            revard.SetActive(true);
            int value = Cat.coinCounterLevel;
            int valueX2 = value * 2;
            Debug.Log("catCoinValue" + valueX2);
            PlayerPrefs.SetInt("valueX2", valueX2);
            textCatCoinValue.text = $"+{valueX2}";

            EffectClick();
        }
    }
    public void CloseOnOpenX2()
    {
        if (revard)
        {
            revard.SetActive(false);
            int valuX2 = PlayerPrefs.GetInt("valueX2");
            int coins = PlayerPrefs.GetInt("coins") + valuX2;
            UpdateCoins(coins);
            effectPSCoin.SetActive(true); // включаем эффект монеток

            EffectClick();
        }

    }
    public void EffectClick()
    {
        // Получаем позицию мыши в мировых координатах
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10f; // расстояние от камеры до плоскости, на которой создаём объект (подбери под свою сцену)
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0f; // если у тебя 2D, чтобы объект был на нужном слое

        // Создаём эффект в позиции мыши
        Instantiate(effectPSClick, worldPos, Quaternion.identity);
    }
   
}
