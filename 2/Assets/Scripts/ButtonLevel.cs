using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{
    public string levelName;
    public string nextlevelName;
    public int lavelStars;
    [SerializeField] public GameObject block; // Объект, который блокирует выбор
    [SerializeField] public GameObject stars3; // Объект, который показывет 3 звезды
    [SerializeField] public GameObject stars2; // Объект, который показывет 2 звезды
    [SerializeField] public GameObject stars1; // Объект, который показывет 1 звезду
    public float isLevelOpen; // флаг открытия уровня
    [SerializeField] private Button myButton;

    void Start()
    {
        //PlayerPrefs.DeleteKey(levelName + "stars"); // для теста сбрасываем сохранение звезд
        //PlayerPrefs.DeleteKey(levelName + "open"); // для теста сбрасываем сохранение открытого уровня
        //PlayerPrefs.SetFloat(nextlevelName + "open", 0); // для теста закрываю блок следующего уровня
        //PlayerPrefs.GetFloat(nextlevelName + "open"); // для теста закрываю блок следующего уровня

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
            Debug.Log(levelName + " включил Block при старте");
            myButton.interactable = false;
        }

        isLevelOpen = UnityEngine.PlayerPrefs.GetFloat(levelName + "open");
        if(isLevelOpen == 1)
        {
            if (block)
            {
                block.SetActive(false);
                Debug.Log("выключил Block " + levelName + " 1");
                myButton.interactable = true;
            }
            
        }
        if (isLevelOpen == 0)
        {
            if (block)
            {
                block.SetActive(true);
                Debug.Log("включил Block " + levelName + " 0");
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
            Debug.Log(levelName + " 3 звезды");
        }
        if (lavelStars == 2)
        {
            stars2.SetActive(true);
            Debug.Log(levelName + " 2 звезды");
        }
        if (lavelStars == 1)
        {
            stars1.SetActive(true);
            Debug.Log(levelName + " 1 звезда");
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
