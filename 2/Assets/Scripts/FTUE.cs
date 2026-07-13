using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class FTUEEntryPoint : MonoBehaviour
{
    private const string FTUE_KEY = "FTUE_Shown";

    [Header("Имена сцен")]
    [Tooltip("Сцена с туториалом (FTUE)")]
    public string ftueSceneName = "FTUE";
    [SerializeField] private GameObject img;

    [Tooltip("Стартовая сцена, если туториал уже пройден")]
    public string startSceneName = "Start";


    private void Start()
    {

        // 0 = не показан, 1 = уже проходили
        if (PlayerPrefs.GetInt(FTUE_KEY, 0) == 1)
        {
            // Туториал уже проходили → сразу идём в Start
            SceneManager.LoadScene(startSceneName);
        }
        else
        {
            img.SetActive(true);
            Time.timeScale = 0f;
        }
        PlayerPrefs.Save();

    }
    public void Continue()
    {
        {
            // Туториал ещё не проходили → идём в сцену FTUE
            Time.timeScale = 1f;
            SceneManager.LoadScene(ftueSceneName);
        }
    }
}