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
    [SerializeField] private bool resetFTUEPrfs = false;
    // Ключ в PlayerPrefs: 0 = FTUE ещё не показывали, 1 = уже проходили
    private const string FTUE_KEY = "FTUE_Shown";
    [SerializeField] private TextMeshProUGUI debug;

    [Header("Scenes")]
    public string ftueSceneName = "FTUE";
    [SerializeField] private AssetReference startSceneName; // Основная сцена игры
    //[SerializeField] private AssetReference ftueSceneName;  // Сцена туториала
    [SerializeField] private GameObject img;                 // Экран загрузки / заглушка

    private AsyncOperationHandle<SceneInstance> startSceneHandle; // Хендл предзагрузки сцены
    private bool startSceneLoaded;                                // Флаг: сцена успешно загружена
    

    private void ResetFTUEPrfs()
    {
        PlayerPrefs.DeleteKey(FTUE_KEY);
        PlayerPrefs.Save();
        Debug.Log("FTUE PlayerPrefs был сброшен");
    }
    private void Start()
    {
        if (resetFTUEPrfs)
        {
            ResetFTUEPrfs();
        }
        // Показываем загрузочный экран сразу
        if (img != null)
            img.SetActive(true);

        // На всякий случай возвращаем нормальную скорость игры
        Time.timeScale = 1f;

        // Начинаем заранее загружать стартовую сцену, но не активируем её
        StartCoroutine(PreloadStartScene());
    }

    private IEnumerator PreloadStartScene()
    {
        // Загружаем сцену в фоне, но не переключаемся на неё сразу
        startSceneHandle = startSceneName.LoadSceneAsync(LoadSceneMode.Single, false);

        // Ждём, пока загрузка закончится
        yield return startSceneHandle;

        // Запоминаем, успешно ли всё загрузилось
        startSceneLoaded = startSceneHandle.Status == AsyncOperationStatus.Succeeded;
    }

    public void Continue()
    {
        // Если FTUE уже проходили
        if (PlayerPrefs.GetInt(FTUE_KEY, 0) == 1)
        {
            // Если стартовая сцена уже загружена — активируем её
            if (startSceneLoaded)
            {
                startSceneHandle.Result.ActivateAsync();
            }
            else
            {
                // Если по какой-то причине не успела загрузиться — грузим обычным способом
                startSceneName.LoadSceneAsync(LoadSceneMode.Single);
            }
        }
        else
        {
            // Если FTUE ещё не проходили — помечаем это в сохранении
            //PlayerPrefs.SetInt(FTUE_KEY, 1);
            //PlayerPrefs.Save();

            // Загружаем сцену FTUE
            SceneManager.LoadScene(ftueSceneName);
            Debug.Log($"{ftueSceneName} загружена");
            debug.text = ftueSceneName.ToString();
        }
    }
}