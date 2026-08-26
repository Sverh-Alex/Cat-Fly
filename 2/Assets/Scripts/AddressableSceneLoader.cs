using System.Collections; // Подключает IEnumerator и coroutine
using UnityEngine; // Подключает базовые классы Unity
using UnityEngine.AddressableAssets; // Подключает AssetReference и Addressables
using UnityEngine.ResourceManagement.AsyncOperations; // Подключает AsyncOperationHandle
using UnityEngine.ResourceManagement.ResourceProviders; // Подключает SceneInstance
using UnityEngine.SceneManagement; // Подключает LoadSceneMode и AsyncOperation
using UnityEngine.UI; // Подключает Slider и Button

public class AddressableSceneLoader : MonoBehaviour
{
    [Header("Сцена для загрузки")]
    [SerializeField] private AssetReference sceneToLoad; // Единственная сцена, которую загружает этот компонент

    [Header("АвтоПредзагрузка (включи для загрузки след уровня)")]
    [SerializeField] private bool preloadWhenLevelCompleted; // Включается только у Loader следующего уровня

    [Header("Отмена предзагрузки (оставь пустым если не Restart)")]
    [SerializeField] private AddressableSceneLoader nextLevelLoader; // Ссылка на Loader следующего уровня, а не на сцену

    [Header("UI загрузки")]
    [SerializeField] private Slider loadingSlider; // Индикатор загрузки
    [SerializeField] private Button continueButton; // Кнопка Continue на победном попапе

    private AsyncOperationHandle<SceneInstance> loadHandle; // Handle загрузки сцены
    private bool isLoading; // Выполняется загрузка данных сцены
    private bool isReadyToActivate; // Сцена загружена до 90 процентов и ждёт активации
    private bool isActivating; // Выполняется активация или немедленный переход

    private void OnEnable()
    {
        if (preloadWhenLevelCompleted) // Проверяем настройку автоматической предзагрузки
        {
            Timer.LevelCompleted += LoadScene; // Подписываем Loader следующего уровня на победу
        }
    }

    private void OnDisable()
    {
        if (preloadWhenLevelCompleted) // Проверяем, создавалась ли подписка
        {
            Timer.LevelCompleted -= LoadScene; // Удаляем подписку на победу
        }
    }

    public void LoadScene()
    {
        if (isLoading || isReadyToActivate || isActivating) // Проверяем активные операции
        {
            Debug.LogWarning(
                "[Loader] Сцена уже загружается или ожидает активации."
            ); // Сообщаем о повторном запуске

            return; // Не запускаем вторую загрузку
        }

        if (!ValidateSceneReference()) // Проверяем Addressable-ссылку
        {
            return; // Не продолжаем с неверной ссылкой
        }

        PrepareLoadingUI(); // Сбрасываем UI перед предзагрузкой
        isLoading = true; // Блокируем повторные вызовы

        StartCoroutine(
            PreloadSceneCoroutine()
        ); // Начинаем фоновую загрузку без активации
    }

    public void SwitchToLoadedScene()
    {
        if (!isReadyToActivate) // Проверяем завершение предзагрузки
        {
            Debug.LogWarning(
                "[Loader] Сцена ещё не готова к активации."
            ); // Сообщаем о раннем нажатии Continue

            return; // Не активируем сцену раньше времени
        }

        if (!loadHandle.IsValid()) // Проверяем корректность Handle
        {
            Debug.LogError(
                "[Loader] Handle загруженной сцены недействителен."
            ); // Сообщаем об ошибке

            ResetState(); // Очищаем некорректное состояние

            return; // Не продолжаем активацию
        }

        if (isActivating) // Проверяем повторное нажатие Continue
        {
            return; // Не запускаем вторую активацию
        }

        StartCoroutine(
            ActivateLoadedSceneCoroutine()
        ); // Активируем подготовленную сцену
    }

    public void LoadAndSwitchScene()
    {
        if (isLoading || isReadyToActivate || isActivating) // Проверяем операции этого Loader
        {
            Debug.LogWarning(
                "[Loader] Этот Loader уже выполняет операцию."
            ); // Сообщаем о повторном вызове

            return; // Не запускаем вторую загрузку
        }

        if (!ValidateSceneReference()) // Проверяем ссылку на текущую сцену
        {
            return; // Не продолжаем с неверной ссылкой
        }

        StartCoroutine(
            LoadAndSwitchSceneCoroutine()
        ); // Начинаем немедленную загрузку и активацию
    }

    public void UnloadScene()
    {
        StartCoroutine(
            CancelPreloadedSceneCoroutine()
        ); // Отменяем предзагрузку сцены вручную
    }

    private IEnumerator PreloadSceneCoroutine()
    {
        Debug.Log(
            "[Loader] Начинаем предварительную загрузку сцены."
        ); // Выводим сообщение о старте предзагрузки

        loadHandle = Addressables.LoadSceneAsync(
            sceneToLoad,
            LoadSceneMode.Single,
            false
        ); // Загружаем сцену без её активации

        while (loadHandle.PercentComplete < 0.9f &&
               !loadHandle.IsDone) // Ждём данные сцены до точки ожидания активации
        {
            UpdateLoadingProgress(); // Обновляем Slider

            yield return null; // Ждём следующий кадр
        }

        if (!loadHandle.IsValid() ||
            loadHandle.Status == AsyncOperationStatus.Failed) // Проверяем ошибку операции
        {
            HandleLoadError(); // Обрабатываем ошибку

            yield break; // Завершаем coroutine
        }

        isLoading = false; // Данные сцены успешно подготовлены
        isReadyToActivate = true; // Разрешаем вызов ActivateAsync

        if (loadingSlider != null) // Проверяем наличие Slider
        {
            loadingSlider.value = 1f; // Показываем 100 процентов
        }

        if (continueButton != null) // Проверяем наличие кнопки Continue
        {
            continueButton.interactable = true; // Разрешаем перейти дальше
        }

        Debug.Log(
            "[Loader] Сцена загружена и ожидает активации."
        ); // Выводим сообщение о готовности
    }

    private IEnumerator ActivateLoadedSceneCoroutine()
    {
        isActivating = true; // Блокируем повторное нажатие Continue
        Time.timeScale = 1f; // Восстанавливаем нормальный ход времени

        Debug.Log(
            "[Loader] Активируем предварительно загруженную сцену."
        ); // Выводим сообщение о старте активации

        AsyncOperation activateOperation =
            loadHandle.Result.ActivateAsync(); // Запускаем активацию сцены

        yield return activateOperation; // Ждём смену сцены
    }

    private IEnumerator LoadAndSwitchSceneCoroutine()
    {
        isActivating = true; // Блокируем повторный вызов Restart

        if (nextLevelLoader != null) // Проверяем наличие Loader следующего уровня
        {
            yield return nextLevelLoader.CancelPreloadedSceneCoroutine(); // Отменяем предзагрузку следующего уровня
        }

        PrepareLoadingUI(); // Сбрасываем UI перед загрузкой
        isLoading = true; // Помечаем начало загрузки

        Debug.Log(
            "[Loader] Загружаем сцену с немедленной активацией."
        ); // Выводим сообщение о старте Restart

        loadHandle = Addressables.LoadSceneAsync(
            sceneToLoad,
            LoadSceneMode.Single,
            true
        ); // Загружаем и сразу активируем текущую сцену

        while (!loadHandle.IsDone) // Ждём полное завершение операции
        {
            UpdateLoadingProgress(); // Обновляем Slider

            yield return null; // Ждём следующий кадр
        }

        if (!IsSceneLoadSucceeded()) // Проверяем результат загрузки
        {
            HandleLoadError(); // Обрабатываем ошибку

            yield break; // Завершаем coroutine
        }

        if (loadingSlider != null) // Проверяем наличие Slider
        {
            loadingSlider.value = 1f; // Показываем успешное завершение
        }

        Debug.Log(
            "[Loader] Сцена успешно загружена и активирована."
        ); // Выводим сообщение об успешном Restart
    }

    private IEnumerator CancelPreloadedSceneCoroutine()
    {
        while (isLoading) // Ждём завершения начатой предзагрузки
        {
            yield return null; // Не выгружаем сцену до готовности операции
        }

        if (!isReadyToActivate) // Проверяем наличие сцены, ожидающей активации
        {
            yield break; // Нечего отменять
        }

        if (!loadHandle.IsValid()) // Проверяем корректность Handle
        {
            ResetState(); // Очищаем ошибочное состояние

            yield break; // Не выгружаем недействительную операцию
        }

        Debug.Log(
            "[Loader] Отменяем предварительно загруженную сцену."
        ); // Выводим сообщение о начале выгрузки

        AsyncOperationHandle<SceneInstance> unloadHandle =
            Addressables.UnloadSceneAsync(
                loadHandle,
                true
            ); // Выгружаем сцену и освобождаем её Handle

        yield return unloadHandle; // Ждём завершения выгрузки

        if (unloadHandle.Status == AsyncOperationStatus.Succeeded) // Проверяем результат выгрузки
        {
            ResetState(); // Сбрасываем состояние Loader

            Debug.Log(
                "[Loader] Предзагрузка сцены отменена."
            ); // Выводим сообщение об успешной отмене
        }
        else
        {
            Debug.LogError(
                $"[Loader] Ошибка выгрузки сцены: " +
                $"{unloadHandle.OperationException}"
            ); // Выводим текст ошибки
        }
    }

    private void UpdateLoadingProgress()
    {
        if (loadingSlider == null) // Проверяем наличие Slider
        {
            return; // Не обновляем отсутствующий UI
        }

        float progress = Mathf.Clamp01(
            loadHandle.PercentComplete / 0.9f
        ); // Преобразуем технический прогресс 0-0.9 в UI-прогресс 0-1

        loadingSlider.value = progress; // Обновляем индикатор загрузки
    }

    private void PrepareLoadingUI()
    {
        if (loadingSlider != null) // Проверяем наличие Slider
        {
            loadingSlider.value = 0f; // Сбрасываем индикатор
        }

        if (continueButton != null) // Проверяем наличие Continue
        {
            continueButton.interactable = false; // Блокируем Continue до готовности сцены
        }
    }

    private bool ValidateSceneReference()
    {
        if (sceneToLoad == null) // Проверяем назначение сцены в Inspector
        {
            Debug.LogError(
                "[Loader] Scene To Load не назначена."
            ); // Выводим ошибку настройки

            return false; // Возвращаем отрицательный результат
        }

        if (!sceneToLoad.RuntimeKeyIsValid()) // Проверяем Addressables RuntimeKey
        {
            Debug.LogError(
                "[Loader] RuntimeKey сцены недействителен."
            ); // Выводим ошибку ключа

            return false; // Возвращаем отрицательный результат
        }

        return true; // Ссылка корректна
    }

    private bool IsSceneLoadSucceeded()
    {
        if (!loadHandle.IsValid()) // Проверяем корректность Handle
        {
            return false; // Возвращаем ошибку проверки
        }

        return loadHandle.Status ==
               AsyncOperationStatus.Succeeded; // Возвращаем результат операции
    }

    private void HandleLoadError()
    {
        Debug.LogError(
            $"[Loader] Ошибка загрузки сцены: " +
            $"{loadHandle.OperationException}"
        ); // Выводим текст ошибки

        ResetState(); // Возвращаем Loader в безопасное состояние
    }

    private void ResetState()
    {
        isLoading = false; // Сбрасываем состояние загрузки
        isReadyToActivate = false; // Сбрасываем готовность к активации
        isActivating = false; // Сбрасываем состояние перехода
        loadHandle = default; // Очищаем сохранённый Handle

        if (loadingSlider != null) // Проверяем наличие Slider
        {
            loadingSlider.value = 0f; // Сбрасываем отображаемый прогресс
        }

        if (continueButton != null) // Проверяем наличие Continue
        {
            continueButton.interactable = false; // Блокируем кнопку Continue
        }
    }
}