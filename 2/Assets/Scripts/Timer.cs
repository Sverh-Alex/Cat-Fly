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
    [SerializeField] private GameObject victoryMenu; // ��������� ��� 3 ������
    [SerializeField] private GameObject victory3Lives; // ��������� ��� 3 ������
    [SerializeField] private GameObject victory2Lives; // ��������� ��� 2 ������
    [SerializeField] private GameObject victory1Life;  // ��������� ��� 1 �����
    [SerializeField] private GameObject loseMenu;  // ��������� ��� 1 �����
    [SerializeField] AudioSource victory;


    void Start()
    {
        timer.text = lifeTime.ToString();
        // ������� ������ PlayerChanger
        GameObject playerChangerObj = GameObject.Find("PlayerChanger");
        if (playerChangerObj != null)
        {
            PlayerChanger playerChanger = playerChangerObj.GetComponent<PlayerChanger>();
            if (playerChanger != null)
            {
                // �������� ��������� ����
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
                                Debug.LogError("��������� Cat �� ������ �� �������� ������� 'Cat'");
                            }
                        }
                        else
                        {
                            Debug.LogError("�������� ������ � ������ 'Cat' �� ������ � ��������� �����");
                        }
                    }
                    else
                    {
                        Debug.LogError("��������� ���� ����� null!");
                    }
                }
                else
                {
                    Debug.LogError("������ ������ ���� ��� ������ ���������� ����� �����������!");
                }
            }
            else
            {
                Debug.LogError("��������� PlayerChanger �� ������ �� ������� PlayerChanger!");
            }
        }
        else
        {
            Debug.LogError("������ PlayerChanger �� ������ � �����!");
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

        // ��������� ����� �������
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
                    // �������� ��������� ����
                    if (playerChanger.skins != null && playerChanger.skins.Length > 0 && playerChanger.skinsId < playerChanger.skins.Length)
                    {
                        GameObject selectedSkin = playerChanger.skins[playerChanger.skinsId];
                        if (selectedSkin != null)
                        {
                            selectedSkin.SetActive(false);

                        }
                        else
                        {
                            Debug.LogError("�� ���� �������� ���� ����� ��������");
                        }
                    }
                }
            }

            if (catScript == null) return; // ������ �� ���������� ����
            Debug.LogError("�� ���� ����� ����� ����");
            victoryMenu.SetActive(true);


            int lives = catScript.GetLifeCounter();
            Debug.Log($"[Timer] �������� ������: {lives} � ������� {catScript.gameObject.name}");

            PlayerPrefs.SetFloat(nextlevelName + "open", 1);

            lifeTime = 0;


            // ��������� ����������
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
                    // ���� ������ ��� ��� ������ 3, ����� �������� �����-�� ��������� ��������� ��� ������
                    Debug.LogWarning("Unexpected life count: " + lives);
                    break;
            }
        }
    }

}
