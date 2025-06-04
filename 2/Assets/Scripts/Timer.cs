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
    [SerializeField] private GameObject victory3Lives; // ��������� ��� 3 ������
    [SerializeField] private GameObject victory2Lives; // ��������� ��� 2 ������
    [SerializeField] private GameObject victory1Life;  // ��������� ��� 1 �����
    [SerializeField] private GameObject loseMenu;  // ��������� ��� 1 �����
    [SerializeField] AudioSource victory;

    void Start()
    {
        timer.text = lifeTime.ToString();
        // ������� ���� � ����� �������������
        catScript = FindObjectOfType<Cat>();

        if (catScript == null)
        {
            Debug.LogError("Cat not found in scene!");
        }

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

        // ��������� ����� �������
        if (lifeTime < 27) timer.color = Color.yellow;
        if (lifeTime < 25) timer.color = Color.green;

        if (lifeTime <= 0)
        {
            if (catScript == null) return; // ������ �� ���������� ����

            int lives = catScript.GetLifeCounter();
            PlayerPrefs.SetFloat(nextlevelName + "open", 1);

            // ��������� ���������� ����
            catScript.gameObject.SetActive(false);
            lifeTime = 0;

            // ��������� ����������
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
        }
    }

}
