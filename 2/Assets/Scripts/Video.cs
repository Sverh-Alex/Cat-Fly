using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.IO;  // Для Path.Combine

public class VideoEndSceneLoader : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Awake()  // Раньше OnEnable
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer не найден на " + gameObject.name);
            enabled = false;
            return;
        }
    }

    void OnEnable()
    {
        if (videoPlayer != null)
            videoPlayer.prepareCompleted += OnPrepareCompleted;
    }

    void OnDisable()
    {
        if (videoPlayer != null)
            videoPlayer.prepareCompleted -= OnPrepareCompleted;
    }

    void Start()
    {
        if (videoPlayer == null) return;

        // StreamingAssets путь для WebGL
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, "Kitten.mp4");
        videoPlayer.playOnAwake = false;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Prepare();
    }

    void OnPrepareCompleted(VideoPlayer vp)
    {
        vp.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
