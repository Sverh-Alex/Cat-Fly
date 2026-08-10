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
    [SerializeField] private string ftueSceneName = "FTUE"; //сцена FTUE Ключ сохранения: 0 — туториал не пройден; 1 — туториал пройден.
    [SerializeField] private AssetReference startSceneName; //ссылка на основную стартовую сцену
    [SerializeField] private bool resetFTUEPrefs = false; //Если включено, ключ FTUE будет удалён при запуске
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private Button btn;
    [SerializeField] private GameObject imgFtue;

    private const string FTUE_KEY = "FTUE_Shown";
    private AsyncOperationHandle<SceneInstance> startSceneHandle; // Хендл предварительной загрузки основной сцены.
    private bool startSceneLoaded; // Показывает, была ли основная сцена успешно загружена.
    private bool transitionStarted; // Защита от повторного нажатия на кнопку Continue.

    private void Start()
    {
        Time.timeScale = 1f;
        imgFtue.SetActive(false);
        
        if (resetFTUEPrefs) // При необходимости сбрасываем сохранение FTUE
        {
            ResetFTUEPrefs();
        }

        bool ftueWasShown = PlayerPrefs.GetInt(FTUE_KEY, 0) == 1; // Проверяем, был ли FTUE уже пройден
        Debug.Log(ftueWasShown);

        if (ftueWasShown)
        {
            return;
        }
        else
        {
            //imgFtue.SetActive(true);
            imgFtue.SetActive(true);
            loadingSlider.value = 0f; // Сбрасываем Slider перед загрузкой
            btn.interactable = false;
            Debug.Log("Кнопка выключена");
            StartCoroutine(PreloadStartScene()); // Если FTUE не пройден, заранее загружаем уровень
        }
    }
    private IEnumerator PreloadStartScene()
    {
        // Начинаем загрузку сцены, но пока не активируем её
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
        StartCoroutine(ActivateStartScene());
        Debug.Log("[FTUE] Пытается активироваться.");
        // Возвращаем нормальную скорость игры.
        Time.timeScale = 1f;

    }

    private IEnumerator ActivateStartScene()
    {
        // Проверяем, что сцена действительно была загружена.
        if (!startSceneLoaded ||
            !startSceneHandle.IsValid() ||
            startSceneHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("[FTUE] Нельзя активировать основную сцену: она не была успешно загружена.");

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


    private void ResetFTUEPrefs()
    {
        // Удаляем сохранённый ключ прохождения FTUE.
        PlayerPrefs.DeleteKey(FTUE_KEY);
        PlayerPrefs.Save();
        Debug.Log("[FTUE] PlayerPrefs был сброшен.");
    }
}