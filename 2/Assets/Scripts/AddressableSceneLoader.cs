using System.Collections;
using Coffee.UIEffects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AddressableSceneLoader : MonoBehaviour
{
    [Header("Сцена для загрузки")]
    [SerializeField] private AssetReference sceneToLoad; // Addressable-ссылка на сцену

    [SerializeField] private Slider loadingSlider; // Индикатор загрузки
    [SerializeField] private UIEffect loadingButton; // Индикатор загрузки
    [SerializeField] private Button btn;
    [SerializeField] private float startLocation = 0f;  // Начальное значение location (1 = справа, 0 = слева)

    private AsyncOperationHandle<SceneInstance> loadHandle; // Handle загрузки сцены
    private bool isReadyToActivate; // Сцена загружена и готова к активации
    private bool isLoading; // Сцена сейчас загружается
    private bool isActivating; // Сцена сейчас активируется

    [SerializeField] private UIScreenPixelation screenPixelation; // Управление UI-пикселизацией
    [SerializeField] private AnimationPulse animationPulse; // управление пульсацией

    private void OnEnable()
    {
        Timer.LevelCompleted += LoadScene; // Подписываемся на завершение уровня
    }

    private void OnDisable()
    {
        Timer.LevelCompleted -= LoadScene; // Отписываемся от события
    }

    public void LoadScene()
    {
        if (isLoading || isReadyToActivate || isActivating) // Проверяем активные операции
        {
            Debug.LogWarning("[Loader] Сцена уже загружается или готова."); // Сообщаем о повторном вызове
            return; // Прерываем повторную загрузку
        }

        if (sceneToLoad == null) // Проверяем ссылку на сцену
        {
            Debug.LogError("[Loader] Scene To Load не назначена."); // Показываем ошибку
            return; // Прерываем загрузку
        }

        if (!sceneToLoad.RuntimeKeyIsValid()) // Проверяем Addressables-ключ
        {
            Debug.LogError("[Loader] У сцены недействительный RuntimeKey."); // Показываем ошибку
            return; // Прерываем загрузку
        }

        if (loadingSlider != null) // Проверяем наличие Slider
        {
            loadingSlider.value = 0f; // Сбрасываем прогресс
        }

        StartCoroutine(LoadSceneCoroutine()); // Запускаем coroutine загрузки
        loadingButton.samplingScale = 0;
        screenPixelation.SetPixelation(5);
        animationPulse.enabled = true;
    }

    private IEnumerator LoadSceneCoroutine()
    {
        isLoading = true; // Помечаем начало загрузки
        isReadyToActivate = false; // Сбрасываем состояние готовности

        Debug.Log("[Loader] Начинаем загрузку сцены."); // Выводим сообщение

        loadHandle = sceneToLoad.LoadSceneAsync( // Запускаем Addressables-загрузку
            LoadSceneMode.Single, // Загружаем сцену вместо текущей
            false // Откладываем активацию сцены
        );

        while (!loadHandle.IsDone) // Ждём окончания загрузки
        {
            float progress = loadHandle.PercentComplete; // Получаем прогресс Addressables

            if (loadingSlider != null) // Проверяем Slider
            {
                loadingSlider.value = progress; // Обновляем Slider
            }

            if (loadingButton != null) // Проверяем UIEffect
            {
                loadingButton.samplingScale =
                    Mathf.Lerp(0f, 5f, progress); // Уменьшаем от 0 до 1
            }

            if (screenPixelation != null) // Проверяем контроллер пикселизации
            {
                screenPixelation.SetPixelation(
                    Mathf.Lerp(0f, 1f, progress)
                ); // Уменьшаем Pixelation от 1 до 0
                Debug.Log("screenPixelation изменили с 0 до 5");
            }
            yield return null; // Ждём следующий кадр
        }

        isLoading = false; // Помечаем завершение загрузки

        if (loadHandle.Status != AsyncOperationStatus.Succeeded) // Проверяем результат
        {
            
            isReadyToActivate = false; // Сцена не готова к активации

            if (loadingSlider != null) // Проверяем Slider
            {
                loadingSlider.value = 0f; // Сбрасываем прогресс при ошибке
            }

            if (loadingButton != null) // Проверяем UIEffect
            {
                loadingButton.samplingScale = 5f; // Возвращаем RGB Shift при ошибке
            }
            Debug.LogError( // Выводим причину ошибки
                $"[Loader] Ошибка загрузки сцены: " +
                $"{loadHandle.OperationException}"
            );

            yield break; // Завершаем coroutine
        }

        if (loadingSlider != null) // Проверяем Slider
        {
            loadingSlider.value = 1f; // Показываем 100 процентов
        }

        if (loadingButton != null) // Проверяем UIEffect
        {
            loadingButton.samplingScale = 5f; // Полностью отключаем RGB Shift
        }
        isReadyToActivate = true; // Разрешаем активацию сцены
        btn.interactable = true;

        Debug.Log( // Выводим сообщение
            "[Loader] Сцена загружена, но ещё не активирована."
        );
    }

    public void SwitchToLoadedScene()
    {
        if (!isReadyToActivate) // Проверяем готовность сцены
        {
            Debug.LogWarning("[Loader] Сцена ещё не готова."); // Выводим предупреждение
            return; // Прерываем активацию
        }

        if (!loadHandle.IsValid()) // Проверяем корректность handle
        {
            Debug.LogError("[Loader] Handle сцены недействителен."); // Выводим ошибку
            return; // Прерываем активацию
        }

        if (isActivating) // Проверяем повторную активацию
        {
            return; // Не запускаем вторую активацию
        }

        isActivating = true; // Сразу блокируем повторный вызов
        Time.timeScale = 1f; // Восстанавливаем игровое время
        StartCoroutine(ActivateSceneCoroutine()); // Запускаем активацию
    }

    private IEnumerator ActivateSceneCoroutine()
    {
        Debug.Log("[Loader] Активируем сцену."); // Выводим сообщение

        AsyncOperation activateOperation = // Получаем операцию активации
            loadHandle.Result.ActivateAsync();

        yield return activateOperation; // Ждём окончания активации

        isActivating = false; // Сбрасываем состояние активации
        isReadyToActivate = false; // Сцена больше не ожидает активации

        Debug.Log("[Loader] Сцена успешно активирована."); // Выводим сообщение
    }

    public void UnloadScene()
    {
        if (!loadHandle.IsValid()) // Проверяем handle
        {
            Debug.LogWarning("[Loader] Handle сцены недействителен."); // Выводим предупреждение
            return; // Прерываем выгрузку
        }

        if (!isReadyToActivate) // Неактивированную сцену выгружать этим методом нельзя
        {
            Debug.LogWarning("[Loader] Сцена ещё не активирована."); // Выводим предупреждение
            return; // Прерываем выгрузку
        }

        StartCoroutine(UnloadSceneCoroutine()); // Запускаем выгрузку
    }

    private IEnumerator UnloadSceneCoroutine()
    {
        Debug.Log("[Loader] Начинаем выгрузку сцены."); // Выводим сообщение

        AsyncOperationHandle<SceneInstance> unloadHandle = // Создаём handle выгрузки
            Addressables.UnloadSceneAsync(loadHandle); // Выгружаем Addressable-сцену

        yield return unloadHandle; // Ждём завершения выгрузки

        if (unloadHandle.Status == AsyncOperationStatus.Succeeded) // Проверяем результат
        {
            isReadyToActivate = false; // Сбрасываем состояние загрузки
            isLoading = false; // Сбрасываем состояние загрузки
            isActivating = false; // Сбрасываем состояние активации
            loadHandle = default; // Очищаем handle

            Debug.Log("[Loader] Сцена выгружена."); // Выводим сообщение
        }
        else
        {
            Debug.LogError( // Выводим ошибку
                $"[Loader] Ошибка выгрузки сцены: " +
                $"{unloadHandle.OperationException}"
            );
        }
    }

    private void OnDestroy()
    {
        if (loadHandle.IsValid() && isReadyToActivate) // Проверяем активную Addressable-сцену
        {
            Debug.LogWarning( // Предупреждаем о незавершённой очистке
                "[Loader] Объект уничтожается с загруженной сценой."
            );
        }
    }
}