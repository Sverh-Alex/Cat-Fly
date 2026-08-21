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

    [Header("Tutorial settings")]
    public float firstPauseDelay = 2f; // Задержка перед первым popup
    public float delayBetweenSteps = 3f; // Задержка между popup
    public Button continueButton; // Кнопка продолжения туториала
    public StepData[] steps; // Массив шагов туториала
    private int currentStepIndex = -1; // Индекс текущего шага
    private bool isPausedByMenu; // Игровое меню удерживает паузу
    private bool isGamePaused; // Popup туториала удерживает паузу
    private bool isApplicationPaused; // Вкладка или приложение потеряли фокус
    private bool skipTimerFrame; // Пропуск первого кадра после возврата во вкладку
    private GameObject continueButtonGO; // GameObject кнопки продолжения
    private bool waitingForFirstStep; // Ожидание первого popup
    private bool waitingForNextStep; // Ожидание следующего popup
    private float timer; // Накопленное время текущей задержки
    private float targetDelay; // Требуемая задержка текущего ожидания

    private void Start()
    {
        Time.timeScale = 1f; // Запускаем игру с обычной скоростью

        if (continueButton != null) // Проверяем наличие кнопки
        {
            continueButtonGO = continueButton.gameObject; // Сохраняем GameObject кнопки

            continueButton.onClick.RemoveAllListeners(); // Удаляем старые обработчики

            continueButton.onClick.AddListener(OnContinueClicked); // Добавляем обработчик кнопки
        }

        HideAll(); // Скрываем все popup и кнопку

        waitingForFirstStep = true; // Запускаем ожидание первого popup

        waitingForNextStep = false; // Отключаем ожидание следующего popup

        timer = 0f; // Обнуляем таймер

        targetDelay = firstPauseDelay; // Устанавливаем задержку первого popup
    }

    private void Update()
    {
        if (isApplicationPaused) // Если вкладка сейчас неактивна
        {
            return; // Не изменяем таймер
        }

        if (skipTimerFrame) // Если это первый кадр после возврата во вкладку
        {
            skipTimerFrame = false; // Сбрасываем флаг пропуска кадра

            return; // Не используем возможный большой deltaTime
        }

        if (isPausedByMenu) // Если открыто игровое меню
        {
            return; // Не запускаем следующий popup
        }

        if (!waitingForFirstStep && !waitingForNextStep) // Если активного ожидания нет
        {
            return; // Завершаем Update
        }

        float deltaTime = Mathf.Min(
            Time.unscaledDeltaTime,
            0.2f
        ); // Ограничиваем возможный скачок времени

        timer += deltaTime; // Продолжаем считать время независимо от Time.timeScale

        if (timer < targetDelay) // Если задержка ещё не закончилась
        {
            return; // Продолжаем ожидание
        }

        timer = 0f; // Сбрасываем таймер после завершения задержки

        if (waitingForFirstStep) // Если ожидается первый popup
        {
            waitingForFirstStep = false; // Завершаем ожидание первого popup

            ShowStep(0); // Показываем первый шаг

            return; // Завершаем текущий кадр
        }

        if (waitingForNextStep) // Если ожидается следующий popup
        {
            waitingForNextStep = false; // Завершаем ожидание следующего popup

            int nextIndex = currentStepIndex + 1; // Рассчитываем индекс следующего шага

            if (nextIndex < steps.Length) // Проверяем наличие следующего шага
            {
                ShowStep(nextIndex); // Показываем следующий popup
            }
            else
            {
                HideAll(); // Скрываем все элементы туториала

                SetGamePause(false); // Снимаем паузу туториала
            }
        }
    }

    public void OnContinueClicked()
    {
        if (!isGamePaused) // Проверяем, активен ли popup туториала
        {
            return; // Игнорируем нажатие
        }

        HideCurrentStep(); // Скрываем текущий popup

        SetGamePause(false); // Снимаем паузу туториала

        timer = 0f; // Начинаем задержку следующего шага с нуля

        targetDelay = delayBetweenSteps; // Устанавливаем задержку до следующего popup

        waitingForNextStep = true; // Запускаем ожидание следующего шага
    }

    private void ShowStep(int index)
    {
        if (index < 0 || index >= steps.Length) // Проверяем корректность индекса
        {
            return; // Выходим при ошибочном индексе
        }

        currentStepIndex = index; // Сохраняем индекс текущего шага

        HideAll(); // Скрываем предыдущие элементы туториала

        if (continueButtonGO != null) // Проверяем наличие кнопки
        {
            continueButtonGO.SetActive(true); // Показываем кнопку продолжения
        }

        if (steps[index].ui != null) // Проверяем наличие UI текущего шага
        {
            steps[index].ui.SetActive(true); // Показываем UI текущего шага
        }

        if (steps[index].prefab != null) // Проверяем наличие prefab текущего шага
        {
            Instantiate(
                steps[index].prefab,
                Vector3.zero,
                Quaternion.identity
            ); // Создаём prefab текущего шага
        }

        SetGamePause(true); // Ставим игровой процесс на паузу
    }

    private void HideCurrentStep()
    {
        if (currentStepIndex < 0 || currentStepIndex >= steps.Length) // Проверяем корректность индекса
        {
            return; // Выходим при ошибочном индексе
        }

        if (steps[currentStepIndex].ui != null) // Проверяем наличие UI текущего шага
        {
            steps[currentStepIndex].ui.SetActive(false); // Скрываем UI текущего шага
        }

        if (continueButtonGO != null) // Проверяем наличие кнопки
        {
            continueButtonGO.SetActive(false); // Скрываем кнопку продолжения
        }
    }

    private void HideAll()
    {
        if (steps != null) // Проверяем наличие массива шагов
        {
            for (int i = 0; i < steps.Length; i++) // Перебираем все шаги
            {
                if (steps[i].ui != null) // Проверяем наличие UI шага
                {
                    steps[i].ui.SetActive(false); // Скрываем UI шага
                }
            }
        }

        if (continueButtonGO != null) // Проверяем наличие кнопки
        {
            continueButtonGO.SetActive(false); // Скрываем кнопку продолжения
        }
    }

    private void SetGamePause(bool paused)
    {
        isGamePaused = paused; // Сохраняем состояние паузы туториала

        ApplyTimeScale(); // Применяем итоговое состояние паузы
    }

    public void PauseByMenu()
    {
        isPausedByMenu = true; // Устанавливаем паузу игрового меню

        ApplyTimeScale(); // Применяем состояние паузы
    }

    public void ResumeByMenu()
    {
        isPausedByMenu = false; // Снимаем паузу игрового меню

        ApplyTimeScale(); // Восстанавливаем состояние паузы туториала
    }

    private void ApplyTimeScale()
    {
        if (isApplicationPaused) // Если вкладка неактивна
        {
            Time.timeScale = 0f; // Оставляем игровой процесс на паузе

            return; // Завершаем метод
        }

        if (isGamePaused || isPausedByMenu) // Если пауза нужна popup или меню
        {
            Time.timeScale = 0f; // Ставим игру на паузу
        }
        else
        {
            Time.timeScale = 1f; // Возвращаем обычную скорость игры
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        isApplicationPaused = !hasFocus; // Сохраняем состояние фокуса вкладки

        if (hasFocus) // Если вкладка снова стала активной
        {
            skipTimerFrame = true; // Пропускаем первый кадр после возвращения
        }

        ApplyTimeScale(); // Применяем итоговое состояние паузы
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        isApplicationPaused = pauseStatus; // Сохраняем состояние системной паузы

        if (!pauseStatus) // Если приложение снова стало активным
        {
            skipTimerFrame = true; // Пропускаем первый кадр после возвращения
        }

        ApplyTimeScale(); // Применяем итоговое состояние паузы
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f; // Возвращаем нормальную скорость при уничтожении объекта
    }
}