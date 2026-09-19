using System;
using TMPro;
using UnityEngine;
using YG;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class ScoreManager : MonoBehaviour
{
    public static event Action OnCoinsChanged; // Событие: изменилось количество монет
    public static event Action OnAlive; // Событие: игра продолжена
    public static event Action OnTutorWeb; // Событие: включена клавиатура (веб)
    public static event Action OnTutorApp; // Событие: включён джойстик (приложение)

    [SerializeField] private TextMeshProUGUI score; // UI: текст с монетами
    [SerializeField] public int addBonusReg = 0; // Бонус за обычное действие
    [SerializeField] private int addBonusMin = 0; // Бонус за рекламу (мало)
    [SerializeField] private int addBonusMax = 0; // Бонус за рекламу (много)
    [SerializeField] private GameObject effectPSClick; // Эффект клика
    [SerializeField] private GameObject effectPSCoin; // Эффект монет (обычный)
    [SerializeField] private GameObject effectPSCoinADS; // Эффект монет (реклама)
    [SerializeField] private GameObject effectPSCoinInapp; // Эффект монет (инап)
    [SerializeField] private string inappProductId = "1"; // ID товара для инапа
    [SerializeField] private TextMeshProUGUI isAddBonusText; // UI: текст бонуса
    [SerializeField] private GameObject revard; // Меню x2 монет
    [SerializeField] private TextMeshProUGUI textCatCoinValue; // UI: монеты за уровень
    [SerializeField] private GameObject rewardMenu; // Меню награды (жизнь)
    [SerializeField] private GameObject loseMenu; // Меню проигрыша
    
    private string currentRewardId; // Текущий ID награды для рекламы

    public static void SendCoinsChanged()
    {
        OnCoinsChanged?.Invoke(); // Уведомляем подписчиков об изменении монет
    }

    public static void SendContinue()
    {
        OnAlive?.Invoke(); // Уведомляем о продолжении игры
    }

    public static void SendTutorialWeb()
    {
        Debug.Log("[ScoreManager] SendTutorialWeb вызван");
        Debug.Log("[ScoreManager] OnTutorWeb == null? " + (OnTutorWeb == null));
        OnTutorWeb?.Invoke(); // Уведомляем о включении клавиатуры
    }

    public static void SendTutorialApp()
    {
        Debug.Log("[ScoreManager] SendTutorialApp вызван");
        OnTutorApp?.Invoke(); // Уведомляем о включении джойстика
    }

    void Start()
    {
        Application.targetFrameRate = 60; // Ограничиваем FPS до 60

        if (revard)
            revard.SetActive(false); // Скрываем меню x2

        effectPSCoin.SetActive(false); // Выключаем эффект монет

        int coins = PlayerPrefs.GetInt("coins"); // Читаем монеты из сохранений
        score.text = coins.ToString(); // Обновляем UI

        if (isAddBonusText)
            isAddBonusText.text = $"+{addBonusReg}"; // Показываем бонус
    }

    private void UpdateCoins(int newCoins)
    {
        PlayerPrefs.SetInt("coins", newCoins); // Сохраняем новые монеты
        score.text = newCoins.ToString(); // Обновляем UI
    }

    public void AddToScore()
    {
        int coins = PlayerPrefs.GetInt("coins") + 1; // +1 монета
        UpdateCoins(coins);
        SendCoinsChanged(); // Уведомляем об изменении
    }

    public void ShowRewardAd(string id)
    {
        currentRewardId = id; // Запоминаем ID текущей награды
        YG2.RewardedAdvShow(id, () => { }); // Показываем рекламу, колбэк пустой
    }

    private void OnReward(string id)
    {
        switch (id)
        {
            case "AddBonusMin":
                AddBonusMin(); // Мало монет
                break;

            case "AddBonusMax":
                AddBonusMax(); // Много монет
                break;

            case "AddBonusX3":
                AddBonusX3(); // x3 монет
                break;

            case "AddLife":
                AddLife(); // Жизнь
                loseMenu.SetActive(false); // Убираем меню проигрыша
                rewardMenu.SetActive(true); // Показываем меню
                break;
        }
    }

    public void AddBonusReg()
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonusReg; // +бонус
        UpdateCoins(coins);
        EffectClick(); // Эффект клика
        effectPSCoin.SetActive(true); // Включаем эффект монет
        var ps = effectPSCoin.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Сброс
            ps.Play(); // Запуск
        }
        SendCoinsChanged(); // Уведомляем
    }

    public void AddBonusMin()
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonusMin; // +мало монет
        UpdateCoins(coins);
        effectPSCoinADS.SetActive(true); // Эффект рекламы
        var ps = effectPSCoinADS.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play();
        }
        EffectClick();
        SendCoinsChanged();
    }

    public void AddBonusMax()
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonusMax; // +много монет
        UpdateCoins(coins);
        effectPSCoinInapp.SetActive(true); // Эффект инапа
        var ps = effectPSCoinInapp.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play();
        }
        EffectClick();
        SendCoinsChanged();
    }

    void OnEnable()
    {
        YG2.onPurchaseSuccess += OnPurchaseSuccess; // Подписка на покупку
        YG2.onRewardAdv += OnRewardAdv; // Подписка на успешную рекламу
    }

    void OnDisable()
    {
        YG2.onPurchaseSuccess -= OnPurchaseSuccess; // Отписка от покупки
        YG2.onRewardAdv -= OnRewardAdv; // Отписка от рекламы
    }

    private void OnRewardAdv(string id)
    {
        if (id != currentRewardId) return; // Проверка ID
        OnReward(id); // Выдаём награду
    }

    private void OnPurchaseSuccess(string id)
    {
        Debug.Log($"[ScoreManager] Purchase success: {id}");
        YG2.SetState(id, 1); // Обновляем состояние

        if (id == inappProductId)
            AddBonusMax(); // Начисляем бонус за инап
    }

    public void BuyInapp()
    {
        Debug.Log("[ScoreManager] Try purchase inapp product with ID: " + inappProductId);
        YG2.PurchaseByID(inappProductId); // Запускаем покупку
    }

    public void AddBonusX3()
    {
        if (revard)
        {
            revard.SetActive(true); // Показываем меню x2
            int value = Cat.coinCounterLevel;
            int valueX3 = value * 3; // x3 монет
            Debug.Log("catCoinValue" + valueX3);
            PlayerPrefs.SetInt("valueX2", valueX3);
            textCatCoinValue.text = $"+{valueX3}"; // UI
            EffectClick();
        }
    }

    public void AddLife()
    {
        EffectClick(); // Эффект клика
    }

    public void CloseOnOpenX2()
    {
        if (revard)
        {
            int valuX2 = UnityEngine.PlayerPrefs.GetInt("valueX2");
            int coins = UnityEngine.PlayerPrefs.GetInt("coins") + valuX2; // +x2 монет
            UpdateCoins(coins);
            PSCoin();
            EffectClick();
            revard.SetActive(false); // Скрываем меню
        }
    }

    public void PSCoin()
    {
        effectPSCoin.SetActive(true); // Эффект монет
        var ps = effectPSCoin.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play();
        }
    }

    public void EffectClick()
    {
        Vector3 mouseScreenPos = Input.mousePosition; // Позиция мыши
        mouseScreenPos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos); // Конвертация в мир
        worldPos.z = 0f;
        Instantiate(effectPSClick, worldPos, Quaternion.identity); // Создаём эффект
    }

    void Update()
    {
        // Debug.Log($"Current coin text: {score.text}");
    }
}