using System.Collections; // Подключает IEnumerator и coroutine
using UnityEngine; // Подключает основные классы Unity
using UnityEngine.AddressableAssets; // Подключает AssetReference
using UnityEngine.ResourceManagement.AsyncOperations; // Подключает AsyncOperationHandle
using UnityEngine.ResourceManagement.ResourceProviders; // Подключает SceneInstance
using UnityEngine.SceneManagement; // Подключает LoadSceneMode
using UnityEngine.UI; // Подключает Slider и Button

public class AddressableSceneLoader : MonoBehaviour
{
    [Header("Сцена для загрузки")]
    [SerializeField] private AssetReference sceneToLoad; // Addressable-ссылка на сцену

    [Header("UI загрузки")]
    [SerializeField] private Slider loadingSlider; // Индикатор загрузки
    [SerializeField] private Button btn; // Кнопка Continue автоматического сценария

    private AsyncOperationHandle<SceneInstance> loadHandle; // Handle загрузки сцены
    private bool isReadyToActivate; // Сцена загружена и готова к активации
    private bool isLoading; // Сцена сейчас загружается
    private bool isActivating; // Сцена сейчас активируется

    private void OnEnable()
    {
        Timer.LevelCompleted += LoadScene; // Подписываемся на автоматическую загрузку
    }

    private void OnDisable()
    {
        Timer.LevelCompleted -= LoadScene; // Отписываемся от автоматической загрузки
    }

    public void LoadScene()
    {
        if (isLoading || isReadyToActivate || isActivating) // Проверяем активные операции
        {
            Debug.LogWarning(
                "[Loader] Сцена уже загружается или готова."
            ); // Выводим предупреждение

            return; // Прерываем повторный запуск
        }

        if (!ValidateSceneReference()) // Проверяем Addressable-ссылку
        {
            return; // Прерываем загрузку
        }

        PrepareLoadingUI(); // Подготавливаем UI загрузки

        isLoading = true; // Блокируем повторную загрузку

        StartCoroutine(
            LoadSceneCoroutine()
        ); // Запускаем предварительную загрузку
    }

    public void LoadAndSwitchScene()
    {
        if (isLoading || isReadyToActivate || isActivating) // Проверяем активные операции
        {
            Debug.LogWarning(
                "[Loader] Сцена уже загружается или активируется."
            ); // Выводим предупреждение

            return; // Прерываем повторный запуск
        }

        if (!ValidateSceneReference()) // Проверяем Addressable-ссылку
        {
            return; // Прерываем загрузку
        }

        PrepareLoadingUI(); // Подготавливаем UI загрузки

        isLoading = true; // Блокируем повторный запуск

        StartCoroutine(
            LoadAndActivateSceneCoroutine()
        ); // Запускаем загрузку и активацию
    }

    public void SwitchToLoadedScene()
    {
        if (!isReadyToActivate) // Проверяем готовность сцены
        {
            Debug.LogWarning(
                "[Loader] Сцена ещё не готова к активации."
            ); // Выводим предупреждение

            return; // Прерываем активацию
        }

        if (!loadHandle.IsValid()) // Проверяем Handle
        {
            Debug.LogError(
                "[Loader] Handle сцены недействителен."
            ); // Выводим ошибку

            return; // Прерываем активацию
        }

        if (isActivating) // Проверяем повторную активацию
        {
            return; // Ничего не делаем
        }

        isActivating = true; // Блокируем повторный запуск
        Time.timeScale = 1f; // Восстанавливаем игровое время

        StartCoroutine(
            ActivateSceneCoroutine()
        ); // Запускаем активацию
    }

    private IEnumerator LoadSceneCoroutine()
    {
        Debug.Log(
            "[Loader] Начинаем предварительную загрузку сцены."
        ); // Выводим сообщение

        loadHandle = sceneToLoad.LoadSceneAsync(
            LoadSceneMode.Single,
            false
        ); // Загружаем сцену без активации

        while (!loadHandle.IsDone) // Ждём окончания загрузки
        {
            UpdateLoadingProgress(); // Обновляем Slider

            yield return null; // Ждём следующий кадр
        }

        isLoading = false; // Завершаем загрузку

        if (!IsSceneLoadSucceeded()) // Проверяем результат
        {
            HandleLoadError(); // Обрабатываем ошибку

            yield break; // Завершаем coroutine
        }

        CompletePreload(); // Завершаем предварительную загрузку
    }

    private IEnumerator LoadAndActivateSceneCoroutine()
    {
        Debug.Log(
            "[Loader] Начинаем загрузку сцены для выбора уровня."
        ); // Выводим сообщение

        loadHandle = sceneToLoad.LoadSceneAsync(
            LoadSceneMode.Single,
            false
        ); // Загружаем сцену без активации

        while (!loadHandle.IsDone) // Ждём окончания загрузки
        {
            UpdateLoadingProgress(); // Обновляем Slider

            yield return null; // Ждём следующий кадр
        }

        isLoading = false; // Завершаем загрузку

        if (!IsSceneLoadSucceeded()) // Проверяем результат
        {
            HandleLoadError(); // Обрабатываем ошибку

            yield break; // Завершаем coroutine
        }

        if (loadingSlider != null) // Проверяем Slider
        {
            loadingSlider.value = 1f; // Показываем 100 процентов
        }

        isActivating = true; // Блокируем повторную активацию
        Time.timeScale = 1f; // Восстанавливаем игровое время

        AsyncOperation activateOperation =
            loadHandle.Result.ActivateAsync(); // Активируем загруженную сцену

        yield return activateOperation; // Ждём окончания активации

        isActivating = false; // Сбрасываем состояние активации
        isReadyToActivate = false; // Сбрасываем состояние готовности

        Debug.Log(
            "[Loader] Сцена успешно загружена и активирована."
        ); // Выводим сообщение
    }

    private IEnumerator ActivateSceneCoroutine()
    {
        Debug.Log(
            "[Loader] Активируем заранее загруженную сцену."
        ); // Выводим сообщение

        AsyncOperation activateOperation =
            loadHandle.Result.ActivateAsync(); // Активируем сцену

        yield return activateOperation; // Ждём активации

        isActivating = false; // Сбрасываем состояние активации
        isReadyToActivate = false; // Сбрасываем состояние готовности

        Debug.Log(
            "[Loader] Сцена успешно активирована."
        ); // Выводим сообщение
    }

    private void UpdateLoadingProgress()
    {
        float progress = loadHandle.PercentComplete; // Получаем прогресс загрузки

        if (loadingSlider != null) // Проверяем Slider
        {
            loadingSlider.value = progress; // Обновляем Slider
        }
    }

    private void PrepareLoadingUI()
    {
        if (loadingSlider != null) // Проверяем Slider
        {
            loadingSlider.value = 0f; // Сбрасываем Slider
        }

        if (btn != null) // Проверяем кнопку Continue
        {
            btn.interactable = false; // Блокируем кнопку
        }
    }

    private void CompletePreload()
    {
        if (loadingSlider != null) // Проверяем Slider
        {
            loadingSlider.value = 1f; // Устанавливаем 100 процентов
        }

        isReadyToActivate = true; // Разрешаем активацию сцены

        if (btn != null) // Проверяем кнопку Continue
        {
            btn.interactable = true; // Разрешаем кнопку Continue
        }

        Debug.Log(
            "[Loader] Сцена загружена, но ещё не активирована."
        ); // Выводим сообщение
    }

    private bool ValidateSceneReference()
    {
        if (sceneToLoad == null) // Проверяем AssetReference
        {
            Debug.LogError(
                "[Loader] Scene To Load не назначена."
            ); // Выводим ошибку

            return false; // Возвращаем отрицательный результат
        }

        if (!sceneToLoad.RuntimeKeyIsValid()) // Проверяем RuntimeKey
        {
            Debug.LogError(
                "[Loader] RuntimeKey недействителен."
            ); // Выводим ошибку

            return false; // Возвращаем отрицательный результат
        }

        return true; // Ссылка корректна
    }

    private bool IsSceneLoadSucceeded()
    {
        if (!loadHandle.IsValid()) // Проверяем Handle
        {
            return false; // Handle недействителен
        }

        return loadHandle.Status ==
               AsyncOperationStatus.Succeeded; // Возвращаем результат загрузки
    }

    private void HandleLoadError()
    {
        isLoading = false; // Сбрасываем состояние загрузки
        isReadyToActivate = false; // Сбрасываем состояние готовности
        isActivating = false; // Сбрасываем состояние активации

        if (loadingSlider != null) // Проверяем Slider
        {
            loadingSlider.value = 0f; // Сбрасываем Slider
        }

        if (btn != null) // Проверяем кнопку
        {
            btn.interactable = false; // Отключаем кнопку
        }

        Debug.LogError(
            $"[Loader] Ошибка загрузки сцены: " +
            $"{loadHandle.OperationException}"
        ); // Выводим ошибку
    }

    public void UnloadScene()
    {
        if (!loadHandle.IsValid()) // Проверяем Handle
        {
            Debug.LogWarning(
                "[Loader] Handle сцены недействителен."
            ); // Выводим предупреждение

            return; // Прерываем выгрузку
        }

        if (isLoading || isActivating) // Проверяем активные операции
        {
            Debug.LogWarning(
                "[Loader] Нельзя выгрузить сцену во время операции."
            ); // Выводим предупреждение

            return; // Прерываем выгрузку
        }

        if (!isReadyToActivate) // Проверяем наличие предварительно загруженной сцены
        {
            Debug.LogWarning(
                "[Loader] Нет загруженной сцены для выгрузки."
            ); // Выводим предупреждение

            return; // Прерываем выгрузку
        }

        StartCoroutine(
            UnloadSceneCoroutine()
        ); // Запускаем выгрузку
    }

    private IEnumerator UnloadSceneCoroutine()
    {
        Debug.Log(
            "[Loader] Начинаем выгрузку сцены."
        ); // Выводим сообщение

        AsyncOperationHandle<SceneInstance> unloadHandle =
            Addressables.UnloadSceneAsync(
                loadHandle
            ); // Выгружаем Addressable-сцену

        yield return unloadHandle; // Ждём завершения выгрузки

        if (unloadHandle.Status ==
            AsyncOperationStatus.Succeeded) // Проверяем результат
        {
            isReadyToActivate = false; // Сбрасываем готовность сцены
            isLoading = false; // Сбрасываем состояние загрузки
            isActivating = false; // Сбрасываем состояние активации
            loadHandle = default; // Очищаем Handle

            Debug.Log(
                "[Loader] Сцена выгружена."
            ); // Выводим сообщение
        }
        else
        {
            Debug.LogError(
                $"[Loader] Ошибка выгрузки сцены: " +
                $"{unloadHandle.OperationException}"
            ); // Выводим ошибку
        }
    }

    private void OnDestroy()
    {
        if (loadHandle.IsValid() &&
            isReadyToActivate) // Проверяем Handle предварительно загруженной сцены
        {
            Debug.LogWarning(
                "[Loader] Объект уничтожается с загруженной сценой."
            ); // Выводим предупреждение
        }
    }
}