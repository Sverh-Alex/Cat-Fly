using UnityEngine;
using UnityEngine.Video;
using UnityEngine.AddressableAssets;
using System.IO;

public class VideoEndSceneLoader : MonoBehaviour
{
    private VideoPlayer videoPlayer; // Ссылка на VideoPlayer

    [SerializeField] private AddressableSceneLoader sceneLoader; // Ссылка на компонент загрузчика

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>(); // Получаем VideoPlayer на этом объекте

        if (videoPlayer == null) // Проверяем наличие VideoPlayer
        {
            Debug.LogError(
                $"[VideoEndSceneLoader] VideoPlayer не найден на объекте {gameObject.name}"
            ); // Выводим ошибку

            enabled = false; // Отключаем этот скрипт

            return; // Завершаем Awake
        }

        if (sceneLoader == null) // Проверяем ссылку на загрузчик
        {
            Debug.LogError(
                "[VideoEndSceneLoader] AddressableSceneLoader не назначен в Inspector"
            ); // Выводим ошибку

            enabled = false; // Отключаем этот скрипт

            return; // Завершаем Awake
        }
    }

    private void OnEnable()
    {
        if (videoPlayer != null) // Проверяем VideoPlayer
        {
            videoPlayer.prepareCompleted += OnPrepareCompleted; // Подписываемся на завершение подготовки видео

            videoPlayer.loopPointReached += OnVideoFinished; // Подписываемся на завершение видео
        }
    }

    private void OnDisable()
    {
        if (videoPlayer != null) // Проверяем VideoPlayer
        {
            videoPlayer.prepareCompleted -= OnPrepareCompleted; // Отписываемся от подготовки видео

            videoPlayer.loopPointReached -= OnVideoFinished; // Отписываемся от завершения видео
        }
    }

    private void Start()
    {
        if (videoPlayer == null) // Проверяем VideoPlayer
        {
            return; // Завершаем Start
        }

        videoPlayer.source = VideoSource.Url; // Указываем источник видео через URL

        videoPlayer.url = Path.Combine(
            Application.streamingAssetsPath,
            "Kitten.mp4"
        ); // Формируем путь к видео

        videoPlayer.playOnAwake = false; // Запрещаем автоматическое воспроизведение

        videoPlayer.Prepare(); // Запускаем подготовку видео
    }

    private void OnPrepareCompleted(VideoPlayer vp)
    {
        vp.Play(); // Запускаем видео после подготовки
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (sceneLoader == null) // Проверяем наличие загрузчика
        {
            Debug.LogError(
                "[VideoEndSceneLoader] Невозможно загрузить сцену: AddressableSceneLoader отсутствует"
            ); // Выводим ошибку

            return; // Завершаем метод
        }

        sceneLoader.LoadAndSwitchScene(); // Запускаем загрузку сцены из AddressableSceneLoader
    }
}