using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class FTUEController : MonoBehaviour
{
    [System.Serializable]
    public class StepData
    {
        public GameObject ui; // UI текущего шага
        public GameObject prefab; // Prefab текущего шага
    }

    public float firstPauseDelay = 2f; // Задержка перед первым popup
    public float delayBetweenSteps = 3f; // Задержка между popup
    public Button continueButton; // Кнопка продолжения туториала
    public StepData[] steps; // Последовательность шагов туториала

    private int currentStepIndex = -1; // Индекс текущего шага
    private bool isPausedByMenu; // Меню удерживает паузу
    private bool isGamePaused; // Popup туториала удерживает паузу
    private GameObject continueButtonGO; // GameObject кнопки продолжения
    private bool waitingForFirstStep; // Ожидание первого шага
    private bool waitingForNextStep; // Ожидание следующего шага
    private float timer; // Текущее время ожидания
    private float targetDelay; // Требуемая задержка

    private void Start()
    {
        Time.timeScale = 1f; // Запускаем игру на нормальной скорости

        if (continueButton != null) // Проверяем наличие кнопки
        {
            continueButtonGO = continueButton.gameObject; // Сохраняем GameObject кнопки
            continueButton.onClick.RemoveAllListeners(); // Удаляем старые обработчики
            continueButton.onClick.AddListener(OnContinueClicked); // Добавляем обработчик продолжения
        }

        HideAll(); // Скрываем все popup

        waitingForFirstStep = true; // Запускаем ожидание первого шага
        waitingForNextStep = false; // Отключаем ожидание следующего шага
        timer = 0f; // Сбрасываем таймер
        targetDelay = firstPauseDelay; // Назначаем задержку первого шага
    }

    private void Update()
    {
        if (isPausedByMenu) // Если меню открыто
        {
            return; // Не запускаем следующий popup
        }

        if (!waitingForFirstStep && !waitingForNextStep) // Если ждать нечего
        {
            return; // Выходим из Update
        }

        timer += Time.unscaledDeltaTime; // Считаем реальное время при любой паузе

        if (timer < targetDelay) // Если задержка ещё не закончилась
        {
            return; // Продолжаем ожидание
        }

        timer = 0f; // Сбрасываем таймер

        if (waitingForFirstStep) // Если ожидается первый шаг
        {
            waitingForFirstStep = false; // Отключаем ожидание первого шага
            ShowStep(0); // Показываем первый шаг
            return; // Завершаем текущий кадр
        }

        if (waitingForNextStep) // Если ожидается следующий шаг
        {
            waitingForNextStep = false; // Отключаем ожидание следующего шага
            int nextIndex = currentStepIndex + 1; // Вычисляем индекс следующего шага

            if (nextIndex < steps.Length) // Проверяем наличие следующего шага
            {
                ShowStep(nextIndex); // Показываем следующий шаг
            }
            else
            {
                HideAll(); // Скрываем все popup
                SetGamePause(false); // Снимаем паузу туториала
            }
        }
    }

    public void OnContinueClicked()
    {
        if (!isGamePaused) // Если popup не активен
        {
            return; // Ничего не делаем
        }

        HideCurrentStep(); // Скрываем текущий popup
        SetGamePause(false); // Снимаем паузу только туториала
        timer = 0f; // Сбрасываем таймер
        targetDelay = delayBetweenSteps; // Назначаем задержку до следующего popup
        waitingForNextStep = true; // Запускаем ожидание следующего шага
    }

    private void ShowStep(int index)
    {
        if (index < 0 || index >= steps.Length) // Проверяем индекс
        {
            return; // Выходим при ошибочном индексе
        }

        currentStepIndex = index; // Сохраняем индекс текущего шага
        HideAll(); // Скрываем предыдущие элементы

        if (continueButtonGO != null) // Проверяем кнопку
        {
            continueButtonGO.SetActive(true); // Показываем кнопку продолжения
        }

        if (steps[index].ui != null) // Проверяем UI шага
        {
            steps[index].ui.SetActive(true); // Показываем UI шага
        }

        if (steps[index].prefab != null) // Проверяем prefab шага
        {
            Instantiate(steps[index].prefab, Vector3.zero, Quaternion.identity); // Создаём prefab
        }

        SetGamePause(true); // Ставим игру на паузу
    }

    private void HideCurrentStep()
    {
        if (currentStepIndex < 0 || currentStepIndex >= steps.Length) // Проверяем индекс
        {
            return; // Выходим при ошибочном индексе
        }

        if (steps[currentStepIndex].ui != null) // Проверяем UI текущего шага
        {
            steps[currentStepIndex].ui.SetActive(false); // Скрываем UI текущего шага
        }

        if (continueButtonGO != null) // Проверяем кнопку
        {
            continueButtonGO.SetActive(false); // Скрываем кнопку
        }
    }

    private void HideAll()
    {
        for (int i = 0; i < steps.Length; i++) // Перебираем все шаги
        {
            if (steps[i].ui != null) // Проверяем UI шага
            {
                steps[i].ui.SetActive(false); // Скрываем UI шага
            }
        }

        if (continueButtonGO != null) // Проверяем кнопку
        {
            continueButtonGO.SetActive(false); // Скрываем кнопку
        }
    }

    private void SetGamePause(bool paused)
    {
        isGamePaused = paused; // Сохраняем состояние popup туториала
        Time.timeScale = isGamePaused || isPausedByMenu ? 0f : 1f; // Учитываем паузу popup и меню
    }

    public void PauseByMenu()
    {
        isPausedByMenu = true; // Меню ставит игру на паузу
        SetGamePause(isGamePaused); // Popup также учитывается
    }

    public void ResumeByMenu()
    {
        isPausedByMenu = false; // Меню снимает только свою паузу
        SetGamePause(isGamePaused); // Popup может оставить игру на паузе
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f; // Возвращаем время при уничтожении объекта
    }
}