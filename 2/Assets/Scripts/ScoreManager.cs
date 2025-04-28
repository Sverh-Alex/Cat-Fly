using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    
    //private int scoreCounter;
    [SerializeField] private TextMeshProUGUI score;
    //[SerializeField] private TextMeshProUGUI scorePrefs;
    private Cat catScript;

    void Start()
    {

        score.text = PlayerPrefs.GetInt("coins").ToString();

        //scorePrefs.text = PlayerPrefs.GetInt("prefsCounter").ToString();
    }

    public void addToScore()
    {
        //scoreCounter++;
        
        int coins = PlayerPrefs.GetInt("coins");
        PlayerPrefs.SetInt("coins", coins + 1); // сохраняем счет игрока
        score.text = (coins + 1).ToString();


    }
}
