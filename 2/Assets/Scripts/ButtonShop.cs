using System;
using JetBrains.Annotations;
using TMPro;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShop : MonoBehaviour
{

    public string objectName; // Уникальное имя товара для сохранения доступа
    public int price; // Цена товара
    public int access;
    public int select;
    public GameObject block; // Объект, который блокирует покупку
    public TextMeshProUGUI objectPriceText; // Текст, отображающий цену товара
    public TextMeshProUGUI coinsText; // Текст, отображающий количество монет игрока
    private Color normalColor = Color.white;
    private Color notEnoughColor = Color.red;



    void Start()
    {
        PlayerPrefs.DeleteKey(objectName + "Access"); // для теста сбрасываем сохранение покупки
        //PlayerPrefs.SetInt("coins", 10);
        coinsText.text = PlayerPrefs.GetInt("coins").ToString();
        AccessUpdate();
        ScoreManager.OnCoinsChanged += ChangeColor;

    }

    void AccessUpdate()
    {
        access = PlayerPrefs.GetInt(objectName + "Access");

        if (objectPriceText != null)
        {
            objectPriceText.text = price.ToString();
        }

        if (access == 1)
        {
            if (block != null)
                block.SetActive(false);

            if (objectPriceText != null)
                objectPriceText.gameObject.SetActive(false);
        }
    }
    public void OnBuy()
    {
        int coins = PlayerPrefs.GetInt("coins");

        if (access == 0)
        {
            if (coins >= price)
            {
                PlayerPrefs.SetInt(objectName + "Access", 1);
                PlayerPrefs.SetInt("coins", coins - price);
                coinsText.text = PlayerPrefs.GetInt("coins").ToString();
                AccessUpdate();
                ScoreManager.SendCoinsChanged(); // Оповещаем всех подписчиков о изменении монет
            }
        }
    }
    private void ChangeColor()
    {
        int coins = PlayerPrefs.GetInt("coins");
        if (objectPriceText == null)
            return;

        if (coins >= price)
        {
            objectPriceText.color = normalColor;
        }
        else
        {
            objectPriceText.color = notEnoughColor;
        }
    }
}
