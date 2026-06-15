// Рабочий вариант загрузки сцены через Addressables
// LoadSceneMode.Single — старая сцена выгружается
// activateOnLoad: false — сцена загружается неактивной, активируется вручную

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class Addressables : MonoBehaviour
{
    [Header("Сцена для загрузки")]
    [SerializeField] private AssetReference sceneToLoad;

    // Handle операции загрузки + флаги состояния
    private AsyncOperationHandle<SceneInstance> loadHandle;
    private bool isLoaded;      // Сцена загружена
    private bool isLoading;     // Загрузка идёт
    private bool isActivating;  // Активация идёт

    // Загрузить сцену (по кнопке "Load")
    public void LoadScene()
    {
        // Защита от повторов
        if (isLoading || isLoaded)
            return;

        if (sceneToLoad == null)
        {
            Debug.LogError("Scene To Load не назначена в инспекторе");
            return;
        }

        isLoading = true;

        // Загружаем сцену:
        // - LoadSceneMode.Single: заменяет текущую (старая выгружается)
        // - false: activateOnLoad = false (сцена неактивная)
        loadHandle = UnityEngine.AddressableAssets.Addressables.LoadSceneAsync(
            sceneToLoad,
            LoadSceneMode.Single,
            false
        );

        // Подписка на завершение загрузки
        loadHandle.Completed += OnSceneLoadCompleted;
    }

    // Callback: завершение загрузки
    private void OnSceneLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        isLoading = false;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Не удалось загрузить сцену");
            return;
        }

        isLoaded = true;
        Debug.Log("Сцена загружена, нажмите SwitchToLoadedScene() для активации.");
    }

    // Активировать сцену (по кнопке "Switch")
    public void SwitchToLoadedScene()
    {
        if (!isLoaded || !loadHandle.IsValid())
        {
            Debug.LogWarning("Сцена ещё не загружена");
            return;
        }

        if (isActivating)
            return;

        isActivating = true;

        // Активируем сцену асинхронно
        var activateOp = loadHandle.Result.ActivateAsync();
        activateOp.completed += op =>
        {
            SceneManager.SetActiveScene(loadHandle.Result.Scene);
            isActivating = false;
            Debug.Log("Сцена активирована.");
        };

        // Восстанавливаем время (если было timeScale = 0)
        Time.timeScale = 1;
    }

    // Выгрузить сцену (по кнопке "Unload")
    public void UnloadScene()
    {
        if (!isLoaded || !loadHandle.IsValid())
        {
            Debug.LogWarning("Нет загруженной сцены");
            return;
        }

        // Выгружаем сцену
        UnityEngine.AddressableAssets.Addressables.UnloadSceneAsync(loadHandle);

        // Сбрасываем флаги и handle
        isLoaded = false;
        isLoading = false;
        isActivating = false;
        loadHandle = default;

        Debug.Log("Сцена выгружена.");
    }
}