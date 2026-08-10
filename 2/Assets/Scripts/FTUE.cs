using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using PlayerPrefs = RedefineYG.PlayerPrefs;
using UnityEngine.UI;

public class FTUEEntryPoint : MonoBehaviour
{
    // Ключ сохранения прохождения туториала:
    // 0 — туториал ещё не пройден;
    // 1 — туториал уже пройден.
    private const string FTUE_KEY = "FTUE_Shown";

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI debug;

    [Header("FTUE")]
    [Tooltip("Имя обычной сцены FTUE. Она должна быть добавлена в Build Settings.")]
    [SerializeField] private string ftueSceneName = "FTUE";

    [Header("Addressables")]
    [Tooltip("Addressable-ссылка на основную стартовую сцену.")]
    [SerializeField] private AssetReference startSceneName;

    [Header("UI")]
    [Tooltip("Экран загрузки или изображение, которое показывается во время загрузки.")]
    [SerializeField] private GameObject img;

    [Header("Settings")]
    [Tooltip("Если включено, ключ FTUE будет удалён при запуске.")]
    [SerializeField] private bool resetFTUEPrefs = false;

    // Хендл предварительной загрузки основной сцены.
    private AsyncOperationHandle<SceneInstance> startSceneHandle;

    // Показывает, была ли основная сцена успешно загружена.
    private bool startSceneLoaded;

    // Защита от повторного нажатия на кнопку Continue.
    private bool transitionStarted;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private Button btn;

    private void Start()
    {
        // При необходимости сбрасываем сохранение FTUE
        if (resetFTUEPrefs)
        {
            ResetFTUEPrefs();
        }

        // Показываем экран загрузки
        ShowLoadingScreen();

        // Возвращаем нормальную скорость игры
        Time.timeScale = 1f;

        // Проверяем, был ли FTUE уже пройден
        bool ftueWasShown = PlayerPrefs.GetInt(FTUE_KEY, 0) == 1;

        if (ftueWasShown)
        {
            // Сбрасываем Slider перед загрузкой
            loadingSlider.value = 0f;
            btn.interactable = false;
            Debug.Log("Кнопка выключена");

            // Если FTUE уже пройден, заранее загружаем Start
            StartCoroutine(PreloadStartScene());
        }
        else
        {
            // Если FTUE ещё не пройден, Start сейчас не нужен.
            // Не создаём параллельную загрузку.
        }
    }
    private IEnumerator PreloadStartScene()
    {
        // Начинаем загрузку Start-сцены,
        // но пока не активируем её
        startSceneHandle = startSceneName.LoadSceneAsync(
            LoadSceneMode.Single,
            false
        );

        // Показываем прогресс загрузки
        while (!startSceneHandle.IsDone)
        {
            // Получаем прогресс Addressables от 0 до 1
            loadingSlider.value =
                startSceneHandle.PercentComplete;

            // Ждём следующий кадр
            yield return null;
        }

        // После завершения устанавливаем 100%
        loadingSlider.value = 1f;
        btn.interactable = true;
        Debug.Log("Кнопка Включена");

        // Проверяем, завершилась ли загрузка успешно
        startSceneLoaded =
            startSceneHandle.Status == AsyncOperationStatus.Succeeded;
    }
    public void Continue()
    {
        // Не допускаем повторного запуска перехода.
        if (transitionStarted)
        {
            return;
        }

        transitionStarted = true;

        // Возвращаем нормальную скорость игры.
        Time.timeScale = 1f;

        // Проверяем, был ли FTUE уже пройден.
        bool ftueWasShown = PlayerPrefs.GetInt(FTUE_KEY, 0) == 1;

        if (ftueWasShown)
        {
            // Если туториал уже был пройден,
            // активируем заранее загруженную основную сцену.
            StartCoroutine(ActivateStartScene());
        }
        else
        {
            Debug.Log($"[FTUE] Загружаем обычную сцену: {ftueSceneName}");

            // Загружаем FTUE через обычный SceneManager.
            // Сцена должна быть добавлена в File > Build Settings.
            SceneManager.LoadScene(ftueSceneName);
        }
    }

    private IEnumerator ActivateStartScene()
    {
        // Проверяем, что сцена действительно была загружена.
        if (!startSceneLoaded ||
            !startSceneHandle.IsValid() ||
            startSceneHandle.Status != AsyncOperationStatus.Succeeded)
        {

            Debug.LogError(
                "[FTUE] Нельзя активировать основную сцену: " +
                "она не была успешно загружена."
            );

            // Разрешаем повторить нажатие после ошибки.
            transitionStarted = false;
            yield break;
        }

        Debug.Log("[FTUE] Активируем заранее загруженную основную сцену.");

        // Активируем сцену только после нажатия Continue.
        AsyncOperation activateOperation =
            startSceneHandle.Result.ActivateAsync();

        // Ждём завершения активации.
        yield return activateOperation;

        Debug.Log("[FTUE] Основная сцена успешно активирована.");
    }

    private void ShowLoadingScreen()
    {
        // Проверяем ссылку перед использованием,
        // чтобы избежать NullReferenceException.
        if (img != null)
        {
            img.SetActive(true);
        }
    }

    private void ResetFTUEPrefs()
    {
        // Удаляем сохранённый ключ прохождения FTUE.
        PlayerPrefs.DeleteKey(FTUE_KEY);
        PlayerPrefs.Save();
        Debug.Log("[FTUE] PlayerPrefs был сброшен.");
    }

}