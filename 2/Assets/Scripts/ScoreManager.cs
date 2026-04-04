using System;
using TMPro;
using UnityEngine;
using YG;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static UnityEngine.Rendering.DebugUI;

public class ScoreManager : MonoBehaviour
{
    public static event Action OnCoinsChanged; // Событие, которое уведомляет подписчиков об изменении количества монет
    public static event Action OnAlive; // Событие, которое уведомляет подписчиков о проолжении игры
    public static event Action OnTutorWeb; // Событие, о включеной клавиатуре
    public static event Action OnTutorApp; // Событие, о включенном джостике

    [SerializeField] private TextMeshProUGUI score; // UI текст для отображения монет
    [SerializeField] public int addBonusReg = 0; // бонус за обычное действие
    [SerializeField] private int addBonusMin = 0; // Сколько монет давать за одну просмотренную рекламу
    [SerializeField] private int addBonusMax = 0; // Сколько монет давать за одну просмотренную рекламу
    [SerializeField] private GameObject effectPSClick; // эффект при клике
    [SerializeField] private GameObject effectPSCoin; // эффект монеток
    [SerializeField] private GameObject effectPSCoinADS; // эффект монеток для кнопки рекламы
    [SerializeField] private TextMeshProUGUI isAddBonusText; // UI текст для отображения монет
    [SerializeField] private GameObject revard; // меню получения х2 монеток
    [SerializeField] private TextMeshProUGUI textCatCoinValue; // текст монет полученных за уровень

    public string rewardID = "10"; // ID награды для рекламы 



    public static void SendCoinsChanged()
    {
        OnCoinsChanged?.Invoke();
    
    }
    public static void SendContinue()
    {
        OnAlive?.Invoke();

    }
    public static void SendTutorialWeb()
    {
        Debug.Log("[ScoreManager] SendTutorialWeb вызван");

        Debug.Log("[ScoreManager] OnTutorWeb == null? " + (OnTutorWeb == null));
        OnTutorWeb?.Invoke();
    }
    public static void SendTutorialApp()
    {
        Debug.Log("[ScoreManager] SendTutorialApp вызван");
        OnTutorApp?.Invoke();

    }
    void Start()
    {
        Application.targetFrameRate = 60;

        if (revard)
        {
            revard.SetActive(false);
        }
        effectPSCoin.SetActive(false); // изначально эффект монеток выключен
        
        int coins = PlayerPrefs.GetInt("coins"); // Получаем текущее количество монет из сохранений
        score.text = coins.ToString();
        
        if(isAddBonusText)
        {
            isAddBonusText.text = $"+{addBonusReg}";
        }
        
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
    public void ShowRewardAd(string id)
    {

        YG2.RewardedAdvShow(id, () => OnReward(id)); // вызовется, когда пользователь досмотрит рекламу до конца.
    }

    private void OnReward(string id)
    {
        switch (id)
        {
            case "AddBonusMin":      // мало монет
                AddBonusMin();
                break;

            case "AddBonusMax":      // много монет
                AddBonusMax();
                break;

            case "AddBonusX3":      // много монет
                AddBonusX3();
                break;

            case "AddLife":      // много монет
                AddLife();
                break;
        }
    }

    public void AddBonusReg() // Добавляет бонусные монеты (например, за действие)
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonusReg;
        UpdateCoins(coins);

        EffectClick();
        effectPSCoin.SetActive(true); // включаем эффект монеток
        ParticleSystem particleSystem = effectPSCoin.GetComponent<ParticleSystem>();
        if (particleSystem != null) // Если компонент Particle System найден
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Остановить и очистить частицы
            particleSystem.Play(); // Запустить заново
            //particleSystem.Emit(30); // Выпустить частицы
        }
        ScoreManager.SendCoinsChanged();
    }

    public void AddBonusMin() // Добавляет бонусные монеты за просмотр рекламы
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonusMin;
        UpdateCoins(coins);
        //effectPSCoinADS.GetComponent<ParticleSystem>().Play();
        //effectPSCoinADS.Play(); // включаем эффект монеток
        effectPSCoinADS.SetActive(true);
        ParticleSystem particleSystem = effectPSCoinADS.GetComponent<ParticleSystem>();
        if (particleSystem != null) // Если компонент Particle System найден
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Остановить и очистить частицы
            particleSystem.Play(); // Запустить заново
            //particleSystem.Emit(30); // Выпустить частицы
        }
        EffectClick(); // Визуальный эффект нажатия или другой отклик
        SendCoinsChanged(); // Сообщаем другим системам (UI, аналитика и т.п.), что количество монет изменилось
    }
    public void AddBonusMax() // Добавляет бонусные монеты за просмотр рекламы
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonusMax;
        UpdateCoins(coins);
        //effectPSCoinADS.GetComponent<ParticleSystem>().Play();
        //effectPSCoinADS.Play(); // включаем эффект монеток
        effectPSCoinADS.SetActive(true);
        ParticleSystem particleSystem = effectPSCoinADS.GetComponent<ParticleSystem>();
        if (particleSystem != null) // Если компонент Particle System найден
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Остановить и очистить частицы
            particleSystem.Play(); // Запустить заново
            //particleSystem.Emit(30); // Выпустить частицы
        }
        EffectClick(); // Визуальный эффект нажатия или другой отклик
        SendCoinsChanged(); // Сообщаем другим системам (UI, аналитика и т.п.), что количество монет изменилось
    }
    public void AddBonusX3()
    {
        if (revard)
        {
            revard.SetActive(true);
            int value = Cat.coinCounterLevel;
            int valueX3 = value * 3;
            Debug.Log("catCoinValue" + valueX3);
            PlayerPrefs.SetInt("valueX2", valueX3);
            textCatCoinValue.text = $"+{valueX3}";

            EffectClick();
        }
    }
    public void AddLife() // Добавляет бонусные монеты за просмотр рекламы
    {
        EffectClick(); // Визуальный эффект нажатия или другой отклик
    }
    public void CloseOnOpenX2()
    {
        if (revard)
        {
            int valuX2 = UnityEngine.PlayerPrefs.GetInt("valueX2");
            int coins = UnityEngine.PlayerPrefs.GetInt("coins") + valuX2;
            UpdateCoins(coins);
            PSCoin();
            EffectClick();
            revard.SetActive(false);

        }

    }
    public void PSCoin()
    {
        effectPSCoin.SetActive(true); // включаем эффект монеток
        ParticleSystem particleSystem = effectPSCoin.GetComponent<ParticleSystem>();
        if (particleSystem != null) // Если компонент Particle System найден
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Остановить и очистить частицы
            particleSystem.Play(); // Запустить заново
                                   //particleSystem.Emit(30); // Выпустить частицы
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
    void Update()
    {
        ///Debug.Log($"Current coin text: {score.text}");
    }
}
