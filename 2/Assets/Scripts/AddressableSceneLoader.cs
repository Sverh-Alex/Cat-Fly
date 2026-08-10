using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AddressableSceneLoader : MonoBehaviour
{
    [Header("Сцена для загрузки")]
    [SerializeField] private AssetReference sceneToLoad;

    // Handle операции загрузки сцены
    private AsyncOperationHandle<SceneInstance> loadHandle;

    // Состояние загрузки
    private bool isLoaded;
    private bool isLoading;
    private bool isActivating;
    [SerializeField] private Slider loadingSlider;

    public void LoadScene()
    {
        loadingSlider.value = 0f;
        // Не допускаем повторную загрузку
        if (isLoading || isLoaded)
        {
            Debug.LogWarning(
                "[Loader] Сцена уже загружается или загружена."
            );

            return;
        }

        // Проверяем ссылку на сцену
        if (sceneToLoad == null)
        {
            Debug.LogError(
                "[Loader] Scene To Load не назначена в Inspector."
            );

            return;
        }

        // Проверяем корректность Addressables-ссылки
        if (!sceneToLoad.RuntimeKeyIsValid())
        {
            Debug.LogError(
                "[Loader] У сцены недействительный RuntimeKey."
            );

            return;
        }

        // Запускаем корутину загрузки
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        isLoading = true;

        Debug.Log("[Loader] Начинаем загрузку сцены.");

        // Загружаем сцену в режиме Single,
        // но не активируем её сразу
        loadHandle = sceneToLoad.LoadSceneAsync(
            LoadSceneMode.Single,
            false
        );
        
        while (!loadHandle.IsDone)
        {
            // Получаем прогресс Addressables от 0 до 1
            loadingSlider.value =
                loadHandle.PercentComplete;

            // Ждём следующий кадр
            yield return null;
        }
        // Ждём окончания загрузки
        yield return loadHandle;

        // Загрузка завершилась
        isLoading = false;

        // После завершения устанавливаем 100%
        loadingSlider.value = 1f;

        // Проверяем результат
        if (loadHandle.Status ==
            AsyncOperationStatus.Succeeded)
        {
            isLoaded = true;

            Debug.Log(
                "[Loader] Сцена загружена, " +
                "но ещё не активирована."
            );
        }
        else
        {
            isLoaded = false;

            string errorMessage =
                loadHandle.OperationException != null
                    ? loadHandle.OperationException.Message
                    : "Неизвестная ошибка.";

            Debug.LogError(
                $"[Loader] Ошибка загрузки сцены: {errorMessage}"
            );
        }
    }

    public void SwitchToLoadedScene()
    {
        // Не допускаем активацию до окончания загрузки
        if (!isLoaded ||
            !loadHandle.IsValid() ||
            loadHandle.Status !=
            AsyncOperationStatus.Succeeded)
        {
            Debug.LogWarning(
                "[Loader] Сцена ещё не загружена."
            );

            return;
        }

        // Не допускаем повторную активацию
        if (isActivating)
        {
            return;
        }

        // Возвращаем нормальную скорость времени
        Time.timeScale = 1f;

        // Запускаем активацию
        StartCoroutine(ActivateSceneCoroutine());
    }

    private IEnumerator ActivateSceneCoroutine()
    {
        isActivating = true;

        Debug.Log("[Loader] Активируем сцену.");

        // Активируем ранее загруженную сцену
        AsyncOperation activateOperation =
            loadHandle.Result.ActivateAsync();

        // Ждём окончания активации
        yield return activateOperation;

        isActivating = false;

        Debug.Log("[Loader] Сцена успешно активирована.");
    }

    public void UnloadScene()
    {
        // Проверяем наличие загруженной сцены
        if (!isLoaded ||
            !loadHandle.IsValid())
        {
            Debug.LogWarning(
                "[Loader] Нет загруженной сцены для выгрузки."
            );

            return;
        }

        Debug.Log("[Loader] Начинаем выгрузку сцены.");

        // Запускаем выгрузку Addressables-сцены
        StartCoroutine(UnloadSceneCoroutine());
    }

    private IEnumerator UnloadSceneCoroutine()
    {
        AsyncOperationHandle<SceneInstance> unloadHandle =
            Addressables.UnloadSceneAsync(loadHandle);

        // Ждём окончания выгрузки
        yield return unloadHandle;

        // Сбрасываем состояние
        isLoaded = false;
        isLoading = false;
        isActivating = false;
        loadHandle = default;

        Debug.Log("[Loader] Сцена выгружена.");
    }
}