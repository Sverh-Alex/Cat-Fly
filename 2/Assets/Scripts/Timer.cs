using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using PlayerPrefs = RedefineYG.PlayerPrefs;
using System;

public class Timer : MonoBehaviour
{
    [Header("UI таймера")]
    [SerializeField] private TextMeshProUGUI timer;   // Текст таймера
    public float lifeTime = 60f;    // Время на уровень

    [Header("Имена уровней для сохранения прогресса")]
    [SerializeField] private AssetReference levelName;        // Текущее название уровня (для stars)
    [SerializeField] private AssetReference nextlevelName;    // Следующий уровень (для open)
    public static event Action LevelCompleted;


    [Header("UI экранов победы/поражения")]
    [SerializeField] private GameObject victoryMenu;   // Общий экран победы
    [SerializeField] private GameObject victory3Lives; // Экран при 3 жизнях
    [SerializeField] private GameObject victory2Lives; // Экран при 2 жизнях
    [SerializeField] private GameObject victory1Life;  // Экран при 1 жизни
    [SerializeField] private GameObject loseMenu;      // Экран при 0 жизней (если понадобится)

    [Header("Звук победы")]
    [SerializeField] private AudioSource victory;      // Аудио для победы

    [Header("Ссылка на скрипт кота")]
    [SerializeField] private Cat catScript;           // Сюда перетаскиваем Cat из инспектора

    private bool isLevelFinished = false;             // Флаг, чтобы не выполнять логику конца уровня много раз

    private void Start()
    {
        // Проверяем, что ссылка на текст таймера назначена
        if (timer == null)
        {
            Debug.LogError("[Timer] Поле 'timer' не назначено в инспекторе!");
        }
        else
        {
            // Показываем стартовое время в UI
            timer.text = lifeTime.ToString("0");
        }

        // Проверяем, что назначен Cat
        if (catScript == null)
        {
            Debug.LogError("[Timer] Поле 'catScript' не назначено в инспекторе! Перетащи сюда объект с компонентом Cat.");
        }

        // Отключаем все UI-плашки победы/поражения в начале
        DeactivateAllVictoryUI();
    }

    /// <summary>
    /// Пауза игры (останавливаем время).
    /// </summary>
    public static void Pause()
    {
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Продолжение игры (возвращаем время).
    /// </summary>
    public static void Continue()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        // Если уровень уже завершён, ничего не делаем
        if (isLevelFinished)
            return;

        // Обновляем таймер
        lifeTime -= Time.deltaTime;
        if (timer != null)
        {
            timer.text = Mathf.Round(lifeTime).ToString();
        }

        // Изменение цвета таймера (пороги можешь под себя настроить)
        if (timer != null)
        {
            if (lifeTime < 27f) timer.color = Color.yellow;
            if (lifeTime < 25f) timer.color = Color.green;
        }

        // Когда время кончилось — считаем жизни и показываем соответствующее меню
        if (lifeTime <= 0f)
        {
            HandleTimeIsOver();
        }
    }

    /// <summary>
    /// Обработка ситуации, когда время вышло.
    /// </summary>
    private void HandleTimeIsOver()
    {
        // Чтобы не заходить сюда несколько кадров подряд
        if (isLevelFinished)
            return;

        isLevelFinished = true;

        if (catScript == null)
        {
            Debug.LogError("[Timer] CatScript == null при завершении уровня! Проверь, назначен ли он в инспекторе.");
            return;
        }

        int lives = catScript.GetLifeCounter();
        Debug.Log($"[Timer] Показываю меню для {lives} жизней");

        switch (lives)
        {
            case 3:
                if (victory3Lives != null) victory3Lives.SetActive(true);
                if (victory != null) victory.Play();
                PlayerPrefs.SetInt(levelName + "stars", 3);
                break;

            case 2:
                if (victory2Lives != null) victory2Lives.SetActive(true);
                if (victory != null) victory.Play();
                PlayerPrefs.SetInt(levelName + "stars", 2);
                break;

            case 1:
                if (victory1Life != null) victory1Life.SetActive(true);
                if (victory != null) victory.Play();
                PlayerPrefs.SetInt(levelName + "stars", 1);
                break;

            default:
                Debug.LogWarning("[Timer] Unexpected life count: " + lives);
                // Пример: если хочешь показывать loseMenu при 0 жизней:
                // if (lives <= 0 && loseMenu != null) loseMenu.SetActive(true);
                break;
        }

        HandleLevelCompletion();
    }

    /// Обработка завершения уровня: выключаем персонажа, показываем общий экран победы и открываем следующий уровень.
    private void HandleLevelCompletion()
    {
        if (catScript != null)
        {
            catScript.gameObject.SetActive(false);
        }

        if (victoryMenu != null)
        {
            victoryMenu.SetActive(true);
        }

        int lives = catScript != null ? catScript.GetLifeCounter() : 0;
        Debug.Log($"[Timer] Текущее количество жизней: {lives}");

        // Открываем следующий уровень (флаг в PlayerPrefs)
        PlayerPrefs.SetFloat(nextlevelName + "open", 1f);
        Debug.Log("Перед LevelCompleted.Invoke()");
        LevelCompleted?.Invoke(); // Отправляем событие для GA

        // Фиксируем время на нуле
        lifeTime = 0f;
        
    }

    /// Выключаем все UI элементы победы/поражения в начале уровня.
    private void DeactivateAllVictoryUI()
    {
        if (victoryMenu != null) victoryMenu.SetActive(false);
        if (victory3Lives != null) victory3Lives.SetActive(false);
        if (victory2Lives != null) victory2Lives.SetActive(false);
        if (victory1Life != null) victory1Life.SetActive(false);
        if (loseMenu != null) loseMenu.SetActive(false);
    }
}
