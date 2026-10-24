using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class UrlVideoSceneLoader : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField]
    private string videoUrl =
        "https://storage.yandexcloud.net/cat-fly/Kitten.mp4";

    [SerializeField] private bool playOnStart = true;

    [Header("Next Scene")]
    [SerializeField] private string nextSceneAddress = "MainMenu";

    [SerializeField] private LoadSceneMode loadMode = LoadSceneMode.Single;

    private AsyncOperationHandle<SceneInstance> sceneHandle;
    private bool sceneLoadStarted;

    private void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer == null)
        {
            Debug.LogError(
                $"[UrlVideoSceneLoader] VideoPlayer не найден на объекте {gameObject.name}."
            );

            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (videoPlayer == null) return;

        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.errorReceived += OnVideoError;
    }

    private void OnDisable()
    {
        if (videoPlayer == null) return;

        videoPlayer.prepareCompleted -= OnVideoPrepared;
        videoPlayer.loopPointReached -= OnVideoFinished;
        videoPlayer.errorReceived -= OnVideoError;
    }

    private void Start()
    {
        if (videoPlayer == null) return;

        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoUrl;

        if (playOnStart)
        {
            videoPlayer.Prepare();
        }
    }

    public void PlayVideo()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("[UrlVideoSceneLoader] VideoPlayer не назначен.");
            return;
        }

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoUrl;

        videoPlayer.Prepare();
    }

    private void OnVideoPrepared(VideoPlayer player)
    {
        player.Play();
    }

    private void OnVideoFinished(VideoPlayer player)
    {
        LoadNextScene();
    }

    private void OnVideoError(VideoPlayer player, string message)
    {
        Debug.LogError($"[UrlVideoSceneLoader] Ошибка видео: {message}");

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (sceneLoadStarted) return;

        sceneLoadStarted = true;

        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        sceneHandle = Addressables.LoadSceneAsync(
            nextSceneAddress,
            loadMode
        );

        yield return sceneHandle;

        if (sceneHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError(
                $"[UrlVideoSceneLoader] Не удалось загрузить сцену '{nextSceneAddress}': {sceneHandle.OperationException}"
            );
        }
    }
}