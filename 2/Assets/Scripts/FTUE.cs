using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using PlayerPrefs = RedefineYG.PlayerPrefs;

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
    private bool LVLStarted;

    private void Start()
    {
        // При необходимости сбрасываем сохранение FTUE.
        if (resetFTUEPrefs)
        {
            ResetFTUEPrefs();
        }

        // Показываем экран загрузки.
        ShowLoadingScreen();

        // Возвращаем обычную скорость времени.
        Time.timeScale = 1f;

        // Начинаем заранее загружать Addressable-сцену.
        // Важно: сцена загружается, но пока не активируется.
        StartCoroutine(PreloadStartScene());
    }

    private IEnumerator PreloadStartScene()
    {
        // Проверяем, назначена ли ссылка на сцену.
        if (startSceneName == null)
        {
            WriteDebug("Ошибка: startSceneName не назначена.");
            Debug.LogError("[FTUE] Addressable-ссылка startSceneName не назначена.");
            yield break;
        }

        // Проверяем корректность Addressables-ссылки.
        if (!startSceneName.RuntimeKeyIsValid())
        {
            WriteDebug("Ошибка: недействительный ключ startSceneName.");
            Debug.LogError("[FTUE] У startSceneName недействительный RuntimeKey.");
            yield break;
        }

        WriteDebug("Начинаем загрузку основной сцены...");
        Debug.Log("[FTUE] Начинаем предварительную загрузку основной сцены.");

        // Загружаем сцену в режиме Single,
        // но false запрещает её немедленную активацию.
        startSceneHandle = startSceneName.LoadSceneAsync(
            LoadSceneMode.Single,
            false
        );

        // Ждём окончания загрузки Addressables-операции.
        yield return startSceneHandle;

        // Проверяем результат загрузки.
        if (startSceneHandle.Status == AsyncOperationStatus.Succeeded)
        {
            startSceneLoaded = true;

            WriteDebug("Основная сцена загружена и ожидает активации.");
            Debug.Log("[FTUE] Основная сцена успешно загружена и ожидает активации.");
        }
        else
        {
            startSceneLoaded = false;

            string errorMessage =
                startSceneHandle.OperationException != null
                    ? startSceneHandle.OperationException.Message
                    : "Неизвестная ошибка загрузки.";

            WriteDebug($"Ошибка загрузки основной сцены:\n{errorMessage}");

            Debug.LogError(
                $"[FTUE] Не удалось загрузить основную сцену: {errorMessage}"
            );
        }
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

        WriteDebug($"FTUE_KEY = {PlayerPrefs.GetInt(FTUE_KEY, 0)}");

        if (ftueWasShown)
        {
            // Если туториал уже был пройден,
            // активируем заранее загруженную основную сцену.
            StartCoroutine(ActivateStartScene());
        }
        else
        {
            // Если туториал ещё не был пройден,
            // сначала сохраняем этот факт.
            //PlayerPrefs.SetInt(FTUE_KEY, 1);
            //PlayerPrefs.Save();

            WriteDebug($"Загружаем сцену FTUE: {ftueSceneName}");
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
            WriteDebug("Основная сцена ещё не готова.");

            Debug.LogError(
                "[FTUE] Нельзя активировать основную сцену: " +
                "она не была успешно загружена."
            );

            // Разрешаем повторить нажатие после ошибки.
            transitionStarted = false;
            yield break;
        }

        WriteDebug("Активируем основную сцену...");
        Debug.Log("[FTUE] Активируем заранее загруженную основную сцену.");

        // Активируем сцену только после нажатия Continue.
        AsyncOperation activateOperation =
            startSceneHandle.Result.ActivateAsync();

        // Ждём завершения активации.
        yield return activateOperation;

        WriteDebug("Основная сцена активирована.");
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

        WriteDebug("FTUE PlayerPrefs сброшен.");
        Debug.Log("[FTUE] PlayerPrefs был сброшен.");
    }

    private void WriteDebug(string message)
    {
        // Выводим сообщение в обычную консоль.
        Debug.Log(message);

        // Дополнительно показываем сообщение на экране,
        // если TextMeshProUGUI назначен в Inspector.
        if (debug != null)
        {
            debug.text = message;
        }
    }
    public void LVL()
    {
        if(LVLStarted)
        {
            return;
        }
        LVLStarted = true;

        SceneManager.LoadScene("Start");
    }
}