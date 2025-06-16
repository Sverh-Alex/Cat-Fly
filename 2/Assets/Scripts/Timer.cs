using System.Security.Cryptography;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public string levelName;
    public string nextlevelName;
    public float lifeTime = 60;
    [SerializeField] public int colorYellow = 0;
    [SerializeField] public int colorRed = 0;
    [SerializeField] private GameObject victoryMenu; // интерфейс при 3 жизнях
    [SerializeField] private GameObject victory3Lives; // интерфейс при 3 жизнях
    [SerializeField] private GameObject victory2Lives; // интерфейс при 2 жизнях
    [SerializeField] private GameObject victory1Life;  // интерфейс при 1 жизни
    [SerializeField] private GameObject loseMenu;  // интерфейс при 0 жизни
    [SerializeField] AudioSource victory;
    public Cat catScript;


    void Start()
    {
        timer.text = lifeTime.ToString();
        InitializeCatScript();
        DeactivateAllVictoryUI();
    }

    private void InitializeCatScript()
    {
        PlayerChanger playerChanger = FindObjectOfType<PlayerChanger>();
        if (playerChanger == null)
        {
            Debug.LogError("PlayerChanger не найден в сцене!");
            return;
        }

        int selectedSkinId = PlayerPrefs.GetInt("skin", 0);
        if (selectedSkinId < 0 || selectedSkinId >= playerChanger.skins.Length)
        {
            Debug.LogError($"Некорректный индекс скина: {selectedSkinId}");
            return;
        }

        GameObject selectedSkin = playerChanger.skins[selectedSkinId];
        if (selectedSkin == null)
        {
            Debug.LogError("Выбранный скин не назначен в инспекторе!");
            return;
        }

        catScript = selectedSkin.GetComponentInChildren<Cat>(true); // Ищем даже в неактивных объектах
        if (catScript == null)
        {
            Debug.LogError("Cat компонент не найден в выбранном скине!");
            return;
        }

        Debug.Log($"Успешно найден Cat в скине {selectedSkin.name}");
    }

    public static void Pause()
    {
        Time.timeScale = 0;
    }
    public static void Continue()
    {
        Time.timeScale = 1;


    }
    void Update()
    {
        lifeTime -= Time.deltaTime;
        timer.text = Mathf.Round(lifeTime).ToString();

        // Изменение цвета таймера
        if (lifeTime < 27) timer.color = Color.yellow;
        if (lifeTime < 25) timer.color = Color.green;

        if (lifeTime <= 0)
        {
            int lives = catScript.GetLifeCounter();
            if (catScript == null)
            {
                Debug.LogError("Ссылка на Cat потеряна!");
                return;
            }
            Debug.Log($"Показываю меню для {lives} жизней");
            //DeactivateAllVictoryUI();
            switch (lives)
            {
                case 3:
                    victory3Lives.SetActive(true);
                    victory.Play();
                    PlayerPrefs.SetInt(levelName + "stars", 3);
                    break;
                case 2:
                    victory2Lives.SetActive(true);
                    victory.Play();
                    PlayerPrefs.SetInt(levelName + "stars", 2);
                    break;
                case 1:
                    victory1Life.SetActive(true);
                    victory.Play();
                    PlayerPrefs.SetInt(levelName + "stars", 1);
                    break;
                default:
                    // Если жизней нет или больше 3, можно показать какой-то дефолтный интерфейс или ничего
                    Debug.LogWarning("Unexpected life count: " + lives);
                    break;
            }
            HandleLevelCompletion();
        }
    }
    private void HandleLevelCompletion()
    {
        catScript.gameObject.SetActive(false);
        victoryMenu.SetActive(true);

        int lives = catScript.GetLifeCounter();
        Debug.Log($"Текущее количество жизней: {lives}");

        PlayerPrefs.SetFloat(nextlevelName + "open", 1);
        lifeTime = 0;
    }

    private void DeactivateAllVictoryUI()
    {
        victoryMenu.SetActive(false);
        victory3Lives.SetActive(false);
        victory2Lives.SetActive(false);
        victory1Life.SetActive(false);
    }
}
