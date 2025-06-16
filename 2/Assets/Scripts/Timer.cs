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
    [SerializeField] private GameObject victoryMenu; // ��������� ��� 3 ������
    [SerializeField] private GameObject victory3Lives; // ��������� ��� 3 ������
    [SerializeField] private GameObject victory2Lives; // ��������� ��� 2 ������
    [SerializeField] private GameObject victory1Life;  // ��������� ��� 1 �����
    [SerializeField] private GameObject loseMenu;  // ��������� ��� 0 �����
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
            Debug.LogError("PlayerChanger �� ������ � �����!");
            return;
        }

        int selectedSkinId = PlayerPrefs.GetInt("skin", 0);
        if (selectedSkinId < 0 || selectedSkinId >= playerChanger.skins.Length)
        {
            Debug.LogError($"������������ ������ �����: {selectedSkinId}");
            return;
        }

        GameObject selectedSkin = playerChanger.skins[selectedSkinId];
        if (selectedSkin == null)
        {
            Debug.LogError("��������� ���� �� �������� � ����������!");
            return;
        }

        catScript = selectedSkin.GetComponentInChildren<Cat>(true); // ���� ���� � ���������� ��������
        if (catScript == null)
        {
            Debug.LogError("Cat ��������� �� ������ � ��������� �����!");
            return;
        }

        Debug.Log($"������� ������ Cat � ����� {selectedSkin.name}");
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

        // ��������� ����� �������
        if (lifeTime < 27) timer.color = Color.yellow;
        if (lifeTime < 25) timer.color = Color.green;

        if (lifeTime <= 0)
        {
            int lives = catScript.GetLifeCounter();
            if (catScript == null)
            {
                Debug.LogError("������ �� Cat ��������!");
                return;
            }
            Debug.Log($"��������� ���� ��� {lives} ������");
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
                    // ���� ������ ��� ��� ������ 3, ����� �������� �����-�� ��������� ��������� ��� ������
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
        Debug.Log($"������� ���������� ������: {lives}");

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
