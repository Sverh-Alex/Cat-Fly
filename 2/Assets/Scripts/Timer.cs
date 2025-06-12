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
    private Cat catScript;
    [SerializeField] private GameObject victoryMenu; // интерфейс при 3 жизнях
    [SerializeField] private GameObject victory3Lives; // интерфейс при 3 жизнях
    [SerializeField] private GameObject victory2Lives; // интерфейс при 2 жизнях
    [SerializeField] private GameObject victory1Life;  // интерфейс при 1 жизни
    [SerializeField] private GameObject loseMenu;  // интерфейс при 1 жизни
    [SerializeField] AudioSource victory;


    void Start()
    {
        timer.text = lifeTime.ToString();
        // Находим объект PlayerChanger
        GameObject playerChangerObj = GameObject.Find("PlayerChanger");
        if (playerChangerObj != null)
        {
            PlayerChanger playerChanger = playerChangerObj.GetComponent<PlayerChanger>();
            if (playerChanger != null)
            {
                // Получаем выбранный скин
                if (playerChanger.skins != null && playerChanger.skins.Length > 0 && playerChanger.skinsId < playerChanger.skins.Length)
                {
                    GameObject selectedSkin = playerChanger.skins[playerChanger.skinsId];
                    if (selectedSkin != null)
                    {
                        Transform catChild = selectedSkin.transform.Find("Cat");
                        if (catChild != null)
                        {
                            catScript = catChild.GetComponent<Cat>();
                            if (catScript == null)
                            {
                                Debug.LogError("Компонент Cat не найден на дочернем объекте 'Cat'");
                            }
                        }
                        else
                        {
                            Debug.LogError("Дочерний объект с именем 'Cat' не найден в выбранном скине");
                        }
                    }
                    else
                    {
                        Debug.LogError("Выбранный скин равен null!");
                    }
                }
                else
                {
                    Debug.LogError("Массив скинов пуст или индекс выбранного скина некорректен!");
                }
            }
            else
            {
                Debug.LogError("Компонент PlayerChanger не найден на объекте PlayerChanger!");
            }
        }
        else
        {
            Debug.LogError("Объект PlayerChanger не найден в сцене!");
        }
        victoryMenu.SetActive(false);
        victory3Lives.SetActive(false);
        victory2Lives.SetActive(false);
        victory1Life.SetActive(false);
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
            GameObject playerChangerObj = GameObject.Find("PlayerChanger");
            if (playerChangerObj != null)
            {
                PlayerChanger playerChanger = playerChangerObj.GetComponent<PlayerChanger>();
                if (playerChanger != null)
                {
                    // Получаем выбранный скин
                    if (playerChanger.skins != null && playerChanger.skins.Length > 0 && playerChanger.skinsId < playerChanger.skins.Length)
                    {
                        GameObject selectedSkin = playerChanger.skins[playerChanger.skinsId];
                        if (selectedSkin != null)
                        {
                            selectedSkin.SetActive(false);

                        }
                        else
                        {
                            Debug.LogError("Не могу отклчить Кота после выигрыша");
                        }
                    }
                }
            }

            if (catScript == null) return; // Защита от отсутствия кота
            Debug.LogError("Не могу найти жизни кота");
            victoryMenu.SetActive(true);


            int lives = catScript.GetLifeCounter();
            Debug.Log($"[Timer] Получено жизней: {lives} у объекта {catScript.gameObject.name}");

            PlayerPrefs.SetFloat(nextlevelName + "open", 1);

            lifeTime = 0;


            // Активация интерфейса
            switch (lives)
            {
                case 3:
                    victory3Lives.SetActive(true);
                    victory.Play();
                    UnityEngine.PlayerPrefs.SetInt(levelName + "stars", 3);
                    break;
                case 2:
                    victory2Lives.SetActive(true);
                    victory.Play();
                    UnityEngine.PlayerPrefs.SetInt(levelName + "stars", 2);
                    break;
                case 1:
                    victory1Life.SetActive(true);
                    victory.Play();
                    UnityEngine.PlayerPrefs.SetInt(levelName + "stars", 1);
                    break;
                default:
                    // Если жизней нет или больше 3, можно показать какой-то дефолтный интерфейс или ничего
                    Debug.LogWarning("Unexpected life count: " + lives);
                    break;
            }
        }
    }

}
