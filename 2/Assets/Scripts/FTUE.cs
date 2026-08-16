using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using PlayerPrefs = RedefineYG.PlayerPrefs;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class FTUEEntryPoint : MonoBehaviour
{
    [SerializeField] private AssetReference ftueSceneName; //ссылка на основную стартовую сцену
    [SerializeField] private bool resetFTUEPrefs = false; //Если включено, ключ FTUE будет удалён при запуске
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private Button btn;
    private AnimationPulseReusable buttonAnimation;
    [SerializeField] private GameObject imgFtue;

    private const string FTUE_KEY = "FTUE_Shown";

    private AsyncOperationHandle<SceneInstance> ftueSceneHandle; // Хендл предварительной загрузки основной сцены.
    private bool ftueSceneLoaded; // Показывает, была ли основная сцена успешно загружена.
    private bool transitionStarted; // Защита от повторного нажатия на кнопку Continue.

    private const string SKIN_KEY = "Cat 2_Access";
    [SerializeField] private GameObject imgBuy_1;
    [SerializeField] private GameObject imgBuy_2;
    
    [SerializeField] private bool resetFIRSTBONUSPrefs = false; //Если включено, ключ FTUE будет удалён при запуске
    private const string FIRSTBONUS_KEY = "FirstBonus";
    [SerializeField] private GameObject imgBuy_3;
    [SerializeField] private GameObject imgBuy_4;


    private void Start()
    {
        Time.timeScale = 1f;
        imgFtue.SetActive(false);
        
        if (resetFTUEPrefs) // При необходимости сбрасываем сохранение FTUE
        {
            ResetFTUEPrefs();
        }

        if (resetFIRSTBONUSPrefs) // При необходимости сбрасываем сохранение FTUE
        {
            ResetFIRSTBONUSandBUYPrefs();
        }

        bool firstBonusClicked = PlayerPrefs.GetInt(FIRSTBONUS_KEY, 0) == 1; // Проверяем, был ли первый бонус
        if (firstBonusClicked)
        {
            HidenFirstBonus();
        }
        else
        {
            imgBuy_3.SetActive(true);
            imgBuy_4.SetActive(true);
            Debug.Log("Картинки показаны");
            
        }

        bool firstBuyClicked = PlayerPrefs.GetInt(SKIN_KEY, 0) == 1; // Проверяем, был ли первый бонус
        Debug.Log($"{firstBuyClicked} firstBuyClicked");
        if (firstBuyClicked)
        {
            imgBuy_1.SetActive(false);
            imgBuy_2.SetActive(false);
        }
        else
        {
            imgBuy_1.SetActive(true);
            imgBuy_2.SetActive(true);
        }

        bool ftueWasShown = PlayerPrefs.GetInt(FTUE_KEY, 0) == 1; // Проверяем, был ли FTUE уже пройден
        if (ftueWasShown)
        {
            btn.interactable = true;
            Time.timeScale = 1f;
            return;
        }
        else
        {
            Time.timeScale = 1f;
            //imgFtue.SetActive(true);
            imgFtue.SetActive(true);
            loadingSlider.value = 0f; // Сбрасываем Slider перед загрузкой
            btn.interactable = false;
            Debug.Log("Кнопка выключена");
            StartCoroutine(PreloadStartScene()); // Если FTUE не пройден, заранее загружаем уровень
        }
        buttonAnimation =
        btn.GetComponent<AnimationPulseReusable>();
        buttonAnimation.enabled = false;


    }
    private IEnumerator PreloadStartScene()
    {
        ftueSceneHandle = ftueSceneName.LoadSceneAsync(LoadSceneMode.Single, false);

        while (!ftueSceneHandle.IsDone)
        {
            loadingSlider.value = ftueSceneHandle.PercentComplete;
            yield return null;
        }

        loadingSlider.value = 1f;

        ftueSceneLoaded = ftueSceneHandle.Status == AsyncOperationStatus.Succeeded;

        if (ftueSceneLoaded)
        {
            btn.interactable = true;
            buttonAnimation.enabled = true;
            Debug.Log("Кнопка включена");
        }
        else
        {
            Debug.LogError($"[FTUE] Ошибка загрузки сцены: {ftueSceneHandle.OperationException}");
            // Можно показать сообщение игроку или перезагрузить
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
        StartCoroutine(ActivateStartScene());
        Debug.Log("[FTUE] Пытается активироваться.");
        // Возвращаем нормальную скорость игры.
        Time.timeScale = 1f;

    }

    private IEnumerator ActivateStartScene()
    {
        // Проверяем, что сцена действительно была загружена.
        if (!ftueSceneLoaded ||
            !ftueSceneHandle.IsValid() ||
            ftueSceneHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("[FTUE] Нельзя активировать основную сцену: она не была успешно загружена.");

            // Разрешаем повторить нажатие после ошибки.
            transitionStarted = false;
            yield break;
        }

        Debug.Log("[FTUE] Активируем заранее загруженную основную сцену.");

        // Активируем сцену только после нажатия Continue.
        AsyncOperation activateOperation =
            ftueSceneHandle.Result.ActivateAsync();

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
    private void ResetFIRSTBONUSandBUYPrefs()
    {
        PlayerPrefs.DeleteKey(FIRSTBONUS_KEY);
        PlayerPrefs.DeleteKey(SKIN_KEY);
        PlayerPrefs.Save();
        Debug.Log("FIRSTBONUS_KEY был сброшен.");
    }
    private void OnEnable()
    {
        ButtonShop.UnlockSkin += HidenBuy;
    }
    private void OnDisable()
    {
        ButtonShop.UnlockSkin -= HidenBuy;
    }
    public void HidenBuy()
    {
        Debug.Log($"FTUE для первой покупки показан");
        imgBuy_1.SetActive(false);
        imgBuy_2.SetActive(false);
        PlayerPrefs.GetInt(SKIN_KEY, 1);
        PlayerPrefs.Save();
    }
    
    
    public void HidenFirstBonus()
    {
        imgBuy_3.SetActive(false); // Скрываем изображения первого бонуса.
        imgBuy_4.SetActive(false);
        PlayerPrefs.SetInt(FIRSTBONUS_KEY, 1); // Сохраняем значение 1, потому что бонус уже был показан или обработан.
        PlayerPrefs.Save();
        Debug.Log("Первый бонус скрыт и сохранён.");
    }
}