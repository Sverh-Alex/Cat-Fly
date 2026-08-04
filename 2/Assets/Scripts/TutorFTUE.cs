using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class FTUEController : MonoBehaviour
{
    [System.Serializable]
    public class StepData
    {
        public GameObject ui;
        public GameObject prefab;
    }

    public float firstPauseDelay = 2f;
    public float delayBetweenSteps = 3f;
    public Button continueButton;
    public StepData[] steps;

    private int currentStepIndex = -1;     // Текущий шаг, -1 = шаг ещё не показан
    private bool isPausedByMenu;           // Туториал временно остановлен меню
    private bool isGamePaused;             // Игра сейчас на паузе из-за туториала
    private GameObject continueButtonGO;   // Ссылка на объект кнопки

    private bool waitingForFirstStep;      // Ждём первый шаг
    private bool waitingForNextStep;       // Ждём следующий шаг
    private float timer;                  // Текущий отсчёт времени
    private float targetDelay;            // Нужная задержка

    private void Start()
    {
        Time.timeScale = 1f;

        if (continueButton != null)
        {
            continueButtonGO = continueButton.gameObject;
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinueClicked);
        }

        HideAll();

        // Запускаем ожидание первого шага
        waitingForFirstStep = true;
        waitingForNextStep = false;
        timer = 0f;
        targetDelay = firstPauseDelay;
    }

    private void Update()
    {
        // Если туториал остановлен через меню — ничего не считаем
        if (isPausedByMenu)
            return;

        // Если сейчас не ждём никаких шагов — выходим
        if (!waitingForFirstStep && !waitingForNextStep)
            return;

        // Считаем время
        timer += Time.deltaTime;

        // Если время ещё не вышло — ждём дальше
        if (timer < targetDelay)
            return;

        // Сбрасываем счётчик
        timer = 0f;

        // Показ первого шага
        if (waitingForFirstStep)
        {
            waitingForFirstStep = false;
            ShowStep(0);
            return;
        }

        // Показ следующего шага
        if (waitingForNextStep)
        {
            waitingForNextStep = false;

            int nextIndex = currentStepIndex + 1;

            if (nextIndex < steps.Length)
            {
                ShowStep(nextIndex);
            }
            else
            {
                HideAll();
                SetGamePause(false);

                PlayerPrefs.SetInt("FTUE_Shown", 1);
                PlayerPrefs.Save();
            }
        }
    }

    public void OnContinueClicked()
    {
        if (!isGamePaused)
            return;

        // Прячем текущий шаг
        HideCurrentStep();

        // Снимаем паузу
        SetGamePause(false);

        // Запускаем ожидание до следующего шага
        timer = 0f;
        targetDelay = delayBetweenSteps;
        waitingForNextStep = true;
    }

    private void ShowStep(int index)
    {
        currentStepIndex = index;

        // Прячем всё перед показом нового шага
        HideAll();

        // Показываем кнопку продолжения
        if (continueButtonGO != null)
            continueButtonGO.SetActive(true);

        // Проверяем границы массива
        if (index < 0 || index >= steps.Length)
            return;

        // Показываем UI шага
        if (steps[index].ui != null)
            steps[index].ui.SetActive(true);

        // Спавним префаб шага
        if (steps[index].prefab != null)
            Instantiate(steps[index].prefab, Vector3.zero, Quaternion.identity);

        // Ставим игру на паузу
        SetGamePause(true);
    }

    private void HideCurrentStep()
    {
        if (currentStepIndex < 0 || currentStepIndex >= steps.Length)
            return;

        if (steps[currentStepIndex].ui != null)
            steps[currentStepIndex].ui.SetActive(false);

        if (continueButtonGO != null)
            continueButtonGO.SetActive(false);
    }

    private void HideAll()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            if (steps[i].ui != null)
                steps[i].ui.SetActive(false);
        }

        if (continueButtonGO != null)
            continueButtonGO.SetActive(false);
    }

    private void SetGamePause(bool paused)
    {
        Time.timeScale = paused ? 0f : 1f;
        isGamePaused = paused;
    }

    public void PauseByMenu()
    {
        // Меню открылось — туториал перестаёт считать время
        isPausedByMenu = true;
    }

    public void ResumeByMenu()
    {
        // Меню закрылось — продолжаем считать дальше
        isPausedByMenu = false;
    }
}