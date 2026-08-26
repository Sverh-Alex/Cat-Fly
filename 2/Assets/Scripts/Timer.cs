using TMPro;
using UnityEngine;
using PlayerPrefs = RedefineYG.PlayerPrefs;
using System;

public class Timer : MonoBehaviour
{
    [Header("UI таймера")]
    [SerializeField] private TextMeshProUGUI timer; // Текст таймера
    [SerializeField] public float lifeTime = 60f; // Время на уровень

    [Header("Имена уровней для сохранения")]
    [SerializeField] private string levelName; // Имя текущего уровня, например LVL_1
    [SerializeField] private string nextLevelName; // Имя следующего уровня, например LVL_2

    [Header("UI экранов победы/поражения")]
    [SerializeField] private GameObject victoryMenu; // Общий экран победы
    [SerializeField] private GameObject victory3Lives; // Экран победы с тремя жизнями
    [SerializeField] private GameObject victory2Lives; // Экран победы с двумя жизнями
    [SerializeField] private GameObject victory1Life; // Экран победы с одной жизнью
    [SerializeField] private GameObject loseMenu; // Экран поражения

    [Header("Звук победы")]
    [SerializeField] private AudioSource victory; // Звук победы

    [Header("Ссылки на игровые объекты")]
    [SerializeField] private Cat catScript; // Ссылка на скрипт кота
    [SerializeField] private Pixel pixelScript; // Ссылка на эффект пикселизации
    public static event Action LevelCompleted; // Событие завершения уровня
    private const string OpenSuffix = "_open"; // Суффикс ключа открытия уровня
    private const string StarsSuffix = "_stars"; // Суффикс ключа звёзд
    private const string FtueShownKey = "FTUE_Shown"; // Ключ завершённого обучения
    private bool isLevelFinished; // Защита от повторного завершения уровня

    private void Start()
    {
        if (string.IsNullOrWhiteSpace(levelName)) // Проверяем имя текущего уровня
        {
            Debug.LogError(
                "[Timer] Поле levelName не заполнено"
            ); // Показываем ошибку настройки
        }

        if (string.IsNullOrWhiteSpace(nextLevelName)) // Проверяем имя следующего уровня
        {
            Debug.LogWarning(
                "[Timer] Поле nextLevelName не заполнено"
            ); // Предупреждаем об отсутствии следующего уровня
        }

        if (timer == null) // Проверяем UI таймера
        {
            Debug.LogError(
                "[Timer] Поле timer не назначено"
            ); // Показываем ошибку настройки
        }
        else
        {
            timer.text = Mathf.CeilToInt(
                lifeTime
            ).ToString(); // Показываем начальное время
        }

        if (catScript == null) // Проверяем скрипт кота
        {
            Debug.LogError(
                "[Timer] Поле catScript не назначено"
            ); // Показываем ошибку настройки
        }

        DeactivateAllVictoryUI(); // Скрываем интерфейс завершения уровня

        if (pixelScript != null) // Проверяем наличие пикселизации
        {
            pixelScript.DisablePixelation(); // Выключаем пикселизацию в начале уровня
        }
    }

    private void Update()
    {
        if (isLevelFinished) // Проверяем, завершён ли уровень
        {
            return; // Не продолжаем работу после завершения
        }

        if (Time.timeScale <= 0f) // Проверяем игровую паузу
        {
            return; // Не уменьшаем таймер во время паузы
        }

        lifeTime -= Time.deltaTime; // Уменьшаем оставшееся время
        lifeTime = Mathf.Max(lifeTime, 0f); // Не позволяем времени уйти в отрицательное значение

        if (timer != null) // Проверяем UI таймера
        {
            timer.text = Mathf.CeilToInt(
                lifeTime
            ).ToString(); // Обновляем текст таймера

            if (lifeTime < 25f) // Проверяем критическое значение времени
            {
                timer.color = Color.green; // Устанавливаем зелёный цвет
            }
            else if (lifeTime < 27f) // Проверяем предупреждающее значение времени
            {
                timer.color = Color.yellow; // Устанавливаем жёлтый цвет
            }
            else
            {
                timer.color = Color.white; // Возвращаем стандартный цвет
            }
        }

        if (lifeTime <= 0f) // Проверяем окончание времени
        {
            HandleTimeIsOver(); // Завершаем уровень
        }
    }

    private void HandleTimeIsOver()
    {
        if (isLevelFinished) // Защищаемся от повторного вызова
        {
            return; // Не выполняем логику второй раз
        }

        isLevelFinished = true; // Фиксируем завершение уровня

        if (catScript == null) // Проверяем ссылку на кота
        {
            Debug.LogError(
                "[Timer] Невозможно определить количество жизней"
            ); // Показываем ошибку

            return; // Прерываем обработку
        }

        int lives = Mathf.Clamp(
            catScript.GetLifeCounter(),
            0,
            3
        ); // Получаем и ограничиваем количество жизней

        Debug.Log(
            $"[Timer] Завершение уровня, жизней: {lives}"
        ); // Выводим результат прохождения

        ShowVictoryUI(lives); // Показываем соответствующий экран

        if (lives > 0) // Проверяем, была ли победа
        {
            SaveStars(lives); // Сохраняем лучший результат в звёздах

            HandleLevelCompletion(); // Открываем следующий уровень
        }
        else
        {
            if (loseMenu != null) // Проверяем экран поражения
            {
                loseMenu.SetActive(true); // Показываем поражение
            }

            Time.timeScale = 0f; // Останавливаем игру при поражении
        }
    }

    private void ShowVictoryUI(int lives)
    {
        if (victory != null) // Проверяем звук победы
        {
            victory.Play(); // Проигрываем звук победы
        }

        switch (lives) // Выбираем экран по количеству жизней
        {
            case 3:
                if (victory3Lives != null) // Проверяем экран трёх жизней
                {
                    victory3Lives.SetActive(true); // Показываем экран трёх жизней
                }
                break;

            case 2:
                if (victory2Lives != null) // Проверяем экран двух жизней
                {
                    victory2Lives.SetActive(true); // Показываем экран двух жизней
                }
                break;

            case 1:
                if (victory1Life != null) // Проверяем экран одной жизни
                {
                    victory1Life.SetActive(true); // Показываем экран одной жизни
                }
                break;

            default:
                Debug.LogWarning($"[Timer] Неожиданное количество жизней: {lives}"); // Предупреждаем о некорректном результате
                break;
        }
    }

    private void SaveStars(int newStars)
    {
        if (string.IsNullOrWhiteSpace(levelName)) // Проверяем имя уровня
        {
            Debug.LogError(
                "[Timer] Нельзя сохранить звёзды: levelName пустой"
            ); // Показываем ошибку

            return; // Не сохраняем результат
        }

        string starsKey = levelName + StarsSuffix; // Формируем ключ звёзд

        int oldStars = PlayerPrefs.GetInt(
            starsKey,
            0
        ); // Загружаем предыдущий лучший результат

        if (newStars <= oldStars) // Проверяем, улучшен ли результат
        {
            Debug.Log(
                $"[Timer] Старый результат {oldStars} звёзд лучше или равен {newStars}"
            ); // Сообщаем, что результат не изменён

            return; // Не перезаписываем лучший результат
        }

        PlayerPrefs.SetInt(starsKey, newStars); // Сохраняем новый лучший результат
        PlayerPrefs.Save(); // Немедленно записываем результат в хранилище

        Debug.Log($"[Timer] Сохранено: {starsKey} = {newStars}"); // Подтверждаем сохранение
    }

    private void HandleLevelCompletion()
    {
        if (catScript != null) // Проверяем наличие кота
        {
            catScript.gameObject.SetActive(false); // Отключаем персонажа
        }

        if (victoryMenu != null) // Проверяем общий экран победы
        {
            victoryMenu.SetActive(true); // Показываем экран победы
        }

        PlayerPrefs.SetInt(FtueShownKey, 1); // Сохраняем факт завершения обучения

        if (!string.IsNullOrWhiteSpace(levelName)) // Проверяем имя текущего уровня
        {
            string currentOpenKey = levelName + OpenSuffix; // Формируем ключ текущего уровня

            PlayerPrefs.SetInt(currentOpenKey, 1); // Помечаем текущий уровень открытым
        }

        if (!string.IsNullOrWhiteSpace(nextLevelName)) // Проверяем имя следующего уровня
        {
            string nextOpenKey = nextLevelName + OpenSuffix; // Формируем ключ следующего уровня

            PlayerPrefs.SetInt(nextOpenKey, 1); // Открываем следующий уровень

            Debug.Log($"[Timer] Открыт следующий уровень: {nextOpenKey}"); // Подтверждаем открытие уровня
        }
        PlayerPrefs.Save(); // Сохраняем все изменения прогресса

        LevelCompleted?.Invoke(); // Отправляем событие аналитики
        Debug.Log("[Timer] Отправлено событие LevelCompleted"); // Подтверждаем отправку события
        lifeTime = 0f; // Фиксируем время на нуле
        Time.timeScale = 1f; // Ставим игру на паузу после победы
    }

    private void DeactivateAllVictoryUI()
    {
        if (victoryMenu != null) // Проверяем общий экран победы
        {
            victoryMenu.SetActive(false); // Скрываем общий экран победы
        }

        if (victory3Lives != null) // Проверяем экран трёх жизней
        {
            victory3Lives.SetActive(false); // Скрываем экран трёх жизней
        }

        if (victory2Lives != null) // Проверяем экран двух жизней
        {
            victory2Lives.SetActive(false); // Скрываем экран двух жизней
        }

        if (victory1Life != null) // Проверяем экран одной жизни
        {
            victory1Life.SetActive(false); // Скрываем экран одной жизни
        }

        if (loseMenu != null) // Проверяем экран поражения
        {
            loseMenu.SetActive(false); // Скрываем экран поражения
        }
    }
}