using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class FTUEController : MonoBehaviour
{
    [Header("Время")]
    [Tooltip("Задержка до паузы перед Шагом 1")]
    public float firstPauseDelay = 2f;

    [Tooltip("Интервал между шагами (после нажатия кнопки)")]
    public float delayBetweenSteps = 3f;

    [Header("UI — шаги")]
    public GameObject step1UI;
    public GameObject step2UI;
    public GameObject step3UI;

    [Header("Префабы для шагов")]
    public GameObject step1Prefab;
    public GameObject step2Prefab;
    public GameObject step3Prefab;

    [Header("Кнопка продолжения")]
    public Button continueButton;
    private GameObject continueButtonGO;

    private bool gameIsPaused = false;
    private int currentStep = 0;      // 0 = до первого шага

    private void Start()
    {
        // Игра в начале идёт без паузы
        Time.timeScale = 1f;
        gameIsPaused = false;

        if (continueButton != null)
        {
            continueButtonGO = continueButton.gameObject;
            continueButton.onClick.RemoveListener(OnContinueButtonClicked);
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        HideAllStepUI(); // прячем и шаги, и кнопку

        StartCoroutine(ShowFirstStepWithDelay());
    }

    private IEnumerator ShowFirstStepWithDelay()
    {
        yield return new WaitForSecondsRealtime(firstPauseDelay);

        SetPause(true);
        ShowStep(1); // UI шага + префаб + кнопка
    }

    // Нажатие на кнопку "Продолжить"
    public void OnContinueButtonClicked()
    {
        if (!gameIsPaused)
            return;

        // 1) Прячем кнопку
        if (continueButtonGO != null)
            continueButtonGO.SetActive(false);

        // 2) Прячем текущий UI шага
        switch (currentStep)
        {
            case 1:
                if (step1UI != null) step1UI.SetActive(false);
                break;
            case 2:
                if (step2UI != null) step2UI.SetActive(false);
                break;
            case 3:
                if (step3UI != null) step3UI.SetActive(false);
                break;
        }

        // 3) Снимаем паузу — игра продолжает идти
        SetPause(false);

        // 4) Ждём задержку и потом покажем следующий шаг
        StartCoroutine(StartNextStepAfterDelay());
    }

    private IEnumerator StartNextStepAfterDelay()
    {
        yield return new WaitForSecondsRealtime(delayBetweenSteps);

        int nextStep = currentStep + 1;

        if (nextStep <= 3)
        {
            SetPause(true);
            ShowStep(nextStep); // внутри снова включится UI шага + кнопка
        }
        else
        {
            HideAllStepUI();
            SetPause(false);

            // отмечаем, что FTUE пройден
            PlayerPrefs.SetInt("FTUE_Shown", 1);
            PlayerPrefs.Save();
            // дальше на этой сцене ты можешь делать катсцену, переходы и т.д.
        }
    }

    // Показ шага: UI шага + префаб + кнопка одновременно
    private void ShowStep(int step)
    {
        currentStep = step;

        HideAllStepUI(); // на всякий случай чистим старый UI

        // включаем кнопку вместе с новым шагом
        if (continueButtonGO != null)
            continueButtonGO.SetActive(true);

        switch (step)
        {
            case 1:
                if (step1UI != null)
                    step1UI.SetActive(true);
                if (step1Prefab != null)
                    SpawnPrefab(step1Prefab);
                break;

            case 2:
                if (step2UI != null)
                    step2UI.SetActive(true);
                if (step2Prefab != null)
                    SpawnPrefab(step2Prefab);
                break;

            case 3:
                if (step3UI != null)
                    step3UI.SetActive(true);
                if (step3Prefab != null)
                    SpawnPrefab(step3Prefab);
                break;
        }
    }

    private void SpawnPrefab(GameObject prefab)
    {
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width * 0.5f,
            Screen.height * 0.5f,
            10f
        ));
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    // Прячем все шаги и кнопку
    private void HideAllStepUI()
    {
        if (step1UI != null) step1UI.SetActive(false);
        if (step2UI != null) step2UI.SetActive(false);
        if (step3UI != null) step3UI.SetActive(false);

        if (continueButtonGO != null) continueButtonGO.SetActive(false);
    }

    private void SetPause(bool paused)
    {
        Time.timeScale = paused ? 0f : 1f;
        gameIsPaused = paused;
    }
}