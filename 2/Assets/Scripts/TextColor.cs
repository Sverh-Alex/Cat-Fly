using System.Diagnostics;
using TMPro;
using UnityEngine;

public class TextColor : MonoBehaviour
{
    public TextMeshProUGUI coinsText; // “екст, отображающий количество монет игрока
    private Color normalColor = Color.white;
    private Color notEnoughColor = Color.green;
    void Start()
    {
        coinsText.text = PlayerPrefs.GetInt("coins").ToString();
        ScoreManager.OnCoinsChanged += ChangeColor;
    }
    private void ChangeColor()
    {
        coinsText.color = notEnoughColor;
    }
    void UpdatePriceUI()
    {
        //int coins = PlayerPrefs.GetInt("coins");
        //objectPriceText.text = price.ToString();
        //objectPriceText.color = coins < price ? notEnoughColor : normalColor;
    }
}
