using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{
    public string levelName;
    public string nextlevelName;
    public int lavelStars;
    [SerializeField] public GameObject block; // ������, ������� ��������� �����
    [SerializeField] public GameObject stars3; // ������, ������� ��������� 3 ������
    [SerializeField] public GameObject stars2; // ������, ������� ��������� 2 ������
    [SerializeField] public GameObject stars1; // ������, ������� ��������� 1 ������
    public float isLevelOpen; // ���� �������� ������
    [SerializeField] private Button myButton;

    void Start()
    {
        //PlayerPrefs.DeleteKey(levelName + "stars"); // ��� ����� ���������� ���������� �����
        //PlayerPrefs.DeleteKey(levelName + "open"); // ��� ����� ���������� ���������� ��������� ������
        //PlayerPrefs.SetFloat(nextlevelName + "open", 0); // ��� ����� �������� ���� ���������� ������
        //PlayerPrefs.GetFloat(nextlevelName + "open"); // ��� ����� �������� ���� ���������� ������

        PlayerPrefs.GetFloat(levelName);
        PlayerPrefs.GetInt(levelName + "stars");
        LevelUpdate();
        StarsUpdate();
    }
    
    void LevelUpdate()
    {
        if (block)
        {
            block.SetActive(true);
            Debug.Log(levelName + " ������� Block ��� ������");
            myButton.interactable = false;
        }

        isLevelOpen = UnityEngine.PlayerPrefs.GetFloat(levelName + "open");
        if(isLevelOpen == 1)
        {
            if (block)
            {
                block.SetActive(false);
                Debug.Log("�������� Block " + levelName + " 1");
                myButton.interactable = true;
            }
            
        }
        if (isLevelOpen == 0)
        {
            if (block)
            {
                block.SetActive(true);
                Debug.Log("������� Block " + levelName + " 0");
                myButton.interactable = false;
            }

        }

    }
    void StarsUpdate()
    {
        stars1.SetActive(false);
        stars2.SetActive(false);
        stars3.SetActive(false);

        lavelStars = UnityEngine.PlayerPrefs.GetInt(levelName + "stars");
        if (lavelStars == 3)
        {
            stars3.SetActive(true);
            Debug.Log(levelName + " 3 ������");
        }
        if (lavelStars == 2)
        {
            stars2.SetActive(true);
            Debug.Log(levelName + " 2 ������");
        }
        if (lavelStars == 1)
        {
            stars1.SetActive(true);
            Debug.Log(levelName + " 1 ������");
        }
        if (lavelStars == 0)
        {
            stars1.SetActive(false);
            stars2.SetActive(false);
            stars3.SetActive(false);
        }
    }
    public void Update()
    {

    }

}
