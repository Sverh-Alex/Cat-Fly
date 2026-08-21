using System.Collections;  // Подключает Coroutine
using UnityEngine;  // Подключает Unity API
using UnityEngine.AddressableAssets;  // Подключает Addressables
using UnityEngine.ResourceManagement.AsyncOperations;  // Подключает AsyncOperationHandle
using UnityEngine.ResourceManagement.ResourceProviders;  // Подключает SceneInstance
using UnityEngine.SceneManagement;  // Подключает работу со сценами
using PlayerPrefs = RedefineYG.PlayerPrefs;  // Использует PlayerPrefs из RedefineYG
using UnityEngine.UI;  // Подключает Slider и Button

public class FTUEStart : MonoBehaviour
{
    [Header("Сцена туториала")]
    [SerializeField] private AssetReference ftueSceneName;  // Ссылка на Addressable-сцену туториала

    [Header("FTUE")]
    [SerializeField] private bool resetFTUEPrefs = false;  // Сбрасывает сохранение FTUE при запуске
    [SerializeField] private Slider loadingSlider;  // Индикатор загрузки
    [SerializeField] private Button btn;  // Кнопка продолжения
    [SerializeField] private GameObject imgFtue;  // Окно FTUE

    private AnimationPulse buttonAnimation;  // Анимация кнопки
    private const string FTUE_KEY = "FTUE_Shown";  // Ключ завершения FTUE

    private AsyncOperationHandle<SceneInstance> ftueSceneHandle;  // Handle загруженной сцены туториала
    private bool ftueSceneLoaded;  // Показывает успешную загрузку туториала
    private bool transitionStarted;  // Защищает от повторного нажатия

    [Header("Первая покупка")]
    [SerializeField] private bool resetBuyPrefs = false;  // Сбрасывает сохранение покупки при запуске
    [SerializeField] private GameObject imgBuy_1;  // Первое изображение покупки
    [SerializeField] private GameObject imgBuy_2;  // Второе изображение покупки

    private const string SKIN_KEY = "Cat 2_Access";  // Ключ первой покупки

    [Header("Первый бонус")]
    [SerializeField] private bool resetFirstBonusPrefs = false;  // Сбрасывает сохранение бонуса при запуске
    [SerializeField] private GameObject imgBuy_3;  // Первое изображение бонуса
    [SerializeField] private GameObject imgBuy_4;  // Второе изображение бонуса

    private const string FIRSTBONUS_KEY = "FirstBonus";  // Ключ первого бонуса

    private Scene startScene;  // Сцена, в которой находится этот объект

    private void Awake()
    {
        startScene = gameObject.scene;  // Запоминает стартовую сцену
        Time.timeScale = 1f;  // Устанавливает нормальную скорость игрового времени
    }

    private void Start()
    {
        Time.timeScale = 1f;  // Устанавливает нормальную скорость игрового времени

        if (imgFtue != null)  // Проверяет окно FTUE
        {
            imgFtue.SetActive(false);  // Скрывает окно FTUE
        }

        if (btn != null)  // Проверяет кнопку
        {
            buttonAnimation = btn.GetComponent<AnimationPulse>();  // Получает AnimationPulse с кнопки

            if (buttonAnimation != null)  // Проверяет наличие AnimationPulse
            {
                buttonAnimation.enabled = false;  // Отключает анимацию до загрузки
            }
        }

        if (resetFTUEPrefs)  // Проверяет необходимость сброса FTUE
        {
            ResetFTUEPrefs();  // Сбрасывает сохранение FTUE
        }

        if (resetBuyPrefs)  // Проверяет необходимость сброса покупки
        {
            ResetBuyPrefs();  // Сбрасывает сохранение покупки
        }

        if (resetFirstBonusPrefs)  // Проверяет необходимость сброса бонуса
        {
            ResetFirstBonusPrefs();  // Сбрасывает сохранение бонуса
        }

        SetupFirstBonus();  // Настраивает изображения первого бонуса
        SetupFirstBuy();  // Настраивает изображения первой покупки

        bool ftueWasShown =
            PlayerPrefs.GetInt(FTUE_KEY, 0) == 1;  // Проверяет завершение FTUE

        if (ftueWasShown)  // Проверяет, был ли FTUE завершён
        {
            Debug.Log("[FTUE] FTUE уже пройден. Туториал не загружается.");  // Выводит сообщение

            if (imgFtue != null)  // Проверяет окно FTUE
            {
                imgFtue.SetActive(false);  // Оставляет окно скрытым
            }

            if (btn != null)  // Проверяет кнопку
            {
                btn.interactable = false;  // Отключает кнопку FTUE
            }

            return;  // Завершает запуск без загрузки туториала
        }

        Debug.Log("[FTUE] FTUE не пройден. Начинается загрузка туториала.");  // Выводит сообщение

        if (imgFtue != null)  // Проверяет окно FTUE
        {
            imgFtue.SetActive(true);  // Показывает окно FTUE
        }

        if (loadingSlider != null)  // Проверяет Slider
        {
            loadingSlider.value = 0f;  // Сбрасывает прогресс
        }

        if (btn != null)  // Проверяет кнопку
        {
            btn.interactable = false;  // Блокирует кнопку до завершения загрузки
        }

        StartCoroutine(PreloadTutorialScene());  // Запускает предварительную загрузку туториала
    }

    private IEnumerator PreloadTutorialScene()
    {
        if (ftueSceneName == null ||
            !ftueSceneName.RuntimeKeyIsValid())  // Проверяет Addressable-ссылку
        {
            Debug.LogError("[FTUE] Сцена туториала не назначена или имеет недействительный ключ.");  // Выводит ошибку
            yield break;  // Завершает Coroutine
        }

        ftueSceneHandle = ftueSceneName.LoadSceneAsync(
            LoadSceneMode.Additive,  // Добавляет туториал к стартовой сцене
            false  // Откладывает активацию туториала
        );

        while (!ftueSceneHandle.IsDone)  // Ждёт завершения загрузки
        {
            if (loadingSlider != null)  // Проверяет Slider
            {
                loadingSlider.value = ftueSceneHandle.PercentComplete;  // Обновляет прогресс
            }

            yield return null;  // Ждёт следующий кадр
        }

        if (ftueSceneHandle.Status != AsyncOperationStatus.Succeeded)  // Проверяет результат загрузки
        {
            Debug.LogError(
                $"[FTUE] Ошибка загрузки туториала: {ftueSceneHandle.OperationException}"  // Выводит ошибку
            );

            yield break;  // Завершает Coroutine
        }

        ftueSceneLoaded = true;  // Запоминает успешную загрузку

        if (loadingSlider != null)  // Проверяет Slider
        {
            loadingSlider.value = 1f;  // Показывает завершение загрузки
        }

        if (btn != null)  // Проверяет кнопку
        {
            btn.interactable = true;  // Разрешает нажатие
        }

        if (buttonAnimation != null)  // Проверяет AnimationPulse
        {
            buttonAnimation.enabled = true;  // Включает анимацию кнопки
        }

        Debug.Log("[FTUE] Туториал загружен и ожидает активации.");  // Выводит сообщение
    }

    public void Continue()
    {
        if (transitionStarted)  // Проверяет повторное нажатие
        {
            return;  // Игнорирует повторный вызов
        }

        if (!ftueSceneLoaded)  // Проверяет готовность туториала
        {
            Debug.LogWarning("[FTUE] Туториал ещё не загружен.");  // Выводит предупреждение
            return;  // Не запускает активацию
        }

        transitionStarted = true;  // Блокирует повторные нажатия
        StartCoroutine(ActivateTutorialAndUnloadStartScene());  // Запускает переход
    }

    private IEnumerator ActivateTutorialAndUnloadStartScene()
    {
        if (!ftueSceneHandle.IsValid() ||
            ftueSceneHandle.Status != AsyncOperationStatus.Succeeded)  // Проверяет handle
        {
            Debug.LogError("[FTUE] Нельзя активировать туториал.");  // Выводит ошибку
            transitionStarted = false;  // Разрешает повторную попытку
            yield break;  // Завершает Coroutine
        }

        if (btn != null)  // Проверяет кнопку
        {
            btn.interactable = false;  // Блокирует кнопку во время перехода
        }

        AsyncOperation activateOperation =
            ftueSceneHandle.Result.ActivateAsync();  // Активирует туториал

        yield return activateOperation;  // Ждёт завершения активации

        Scene tutorialScene = ftueSceneHandle.Result.Scene;  // Получает сцену туториала

        if (!tutorialScene.IsValid() ||
            !tutorialScene.isLoaded)  // Проверяет загруженную сцену
        {
            Debug.LogError("[FTUE] Сцена туториала недействительна.");  // Выводит ошибку
            transitionStarted = false;  // Разрешает повторную попытку
            yield break;  // Завершает Coroutine
        }

        SceneManager.SetActiveScene(tutorialScene);  // Делает туториал активной сценой

        if (imgFtue != null)  // Проверяет окно FTUE
        {
            imgFtue.SetActive(false);  // Скрывает окно FTUE
        }

        if (startScene.IsValid() &&
            startScene.isLoaded &&
            startScene != tutorialScene)  // Проверяет стартовую сцену
        {
            AsyncOperation unloadOperation =
                SceneManager.UnloadSceneAsync(startScene);  // Выгружает стартовую сцену

            if (unloadOperation == null)  // Проверяет операцию выгрузки
            {
                Debug.LogError("[FTUE] Не удалось начать выгрузку стартовой сцены.");  // Выводит ошибку
                yield break;  // Завершает Coroutine
            }

            yield return unloadOperation;  // Ждёт завершения выгрузки
        }

        Debug.Log("[FTUE] Туториал активирован, стартовая сцена выгружена.");  // Выводит результат
    }

    private void SetupFirstBonus()
    {
        bool firstBonusClicked =
            PlayerPrefs.GetInt(FIRSTBONUS_KEY, 0) == 1;  // Проверяет состояние бонуса

        if (firstBonusClicked)  // Проверяет обработанный бонус
        {
            HideFirstBonusImages();  // Скрывает изображения бонуса
        }
        else
        {
            if (imgBuy_3 != null)  // Проверяет третье изображение
            {
                imgBuy_3.SetActive(true);  // Показывает третье изображение
            }

            if (imgBuy_4 != null)  // Проверяет четвёртое изображение
            {
                imgBuy_4.SetActive(true);  // Показывает четвёртое изображение
            }
        }
    }

    private void SetupFirstBuy()
    {
        bool firstBuyClicked =
            PlayerPrefs.GetInt(SKIN_KEY, 0) == 1;  // Проверяет состояние покупки

        if (firstBuyClicked)  // Проверяет выполненную покупку
        {
            if (imgBuy_1 != null)  // Проверяет первое изображение
            {
                imgBuy_1.SetActive(false);  // Скрывает первое изображение
            }

            if (imgBuy_2 != null)  // Проверяет второе изображение
            {
                imgBuy_2.SetActive(false);  // Скрывает второе изображение
            }
        }
        else
        {
            if (imgBuy_1 != null)  // Проверяет первое изображение
            {
                imgBuy_1.SetActive(true);  // Показывает первое изображение
            }

            if (imgBuy_2 != null)  // Проверяет второе изображение
            {
                imgBuy_2.SetActive(true);  // Показывает второе изображение
            }
        }
    }

    private void HideFirstBonusImages()
    {
        if (imgBuy_3 != null)  // Проверяет третье изображение
        {
            imgBuy_3.SetActive(false);  // Скрывает третье изображение
        }

        if (imgBuy_4 != null)  // Проверяет четвёртое изображение
        {
            imgBuy_4.SetActive(false);  // Скрывает четвёртое изображение
        }
    }

    public void HidenBuy()
    {
        Debug.Log("FTUE для первой покупки показан");  // Выводит сообщение

        if (imgBuy_1 != null)  // Проверяет первое изображение
        {
            imgBuy_1.SetActive(false);  // Скрывает первое изображение
        }

        if (imgBuy_2 != null)  // Проверяет второе изображение
        {
            imgBuy_2.SetActive(false);  // Скрывает второе изображение
        }

        PlayerPrefs.SetInt(SKIN_KEY, 1);  // Сохраняет факт покупки
        PlayerPrefs.Save();  // Сохраняет изменения
    }

    public void HidenFirstBonus()
    {
        HideFirstBonusImages();  // Скрывает изображения первого бонуса
        PlayerPrefs.SetInt(FIRSTBONUS_KEY, 1);  // Сохраняет факт обработки бонуса
        PlayerPrefs.Save();  // Сохраняет изменения
        Debug.Log("Первый бонус скрыт и сохранён.");  // Выводит сообщение
    }

    private void ResetFTUEPrefs()
    {
        PlayerPrefs.DeleteKey(FTUE_KEY);  // Удаляет ключ FTUE
        PlayerPrefs.Save();  // Сохраняет изменения
        Debug.Log("[FTUE] Сохранение FTUE сброшено.");  // Выводит сообщение
    }

    private void ResetBuyPrefs()
    {
        PlayerPrefs.DeleteKey(SKIN_KEY);  // Удаляет ключ покупки
        PlayerPrefs.Save();  // Сохраняет изменения
        Debug.Log("[FTUE] Сохранение покупки сброшено.");  // Выводит сообщение
    }

    private void ResetFirstBonusPrefs()
    {
        PlayerPrefs.DeleteKey(FIRSTBONUS_KEY);  // Удаляет ключ бонуса
        PlayerPrefs.Save();  // Сохраняет изменения
        Debug.Log("[FTUE] Сохранение первого бонуса сброшено.");  // Выводит сообщение
    }

    private void OnEnable()
    {
        ButtonShop.UnlockSkin += HidenBuy;  // Подписывается на событие покупки
    }

    private void OnDisable()
    {
        ButtonShop.UnlockSkin -= HidenBuy;  // Отписывается от события покупки
    }
}