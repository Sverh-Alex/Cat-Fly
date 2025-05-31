using System;
using JetBrains.Annotations;
using TMPro;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShop : MonoBehaviour
{

    public string objectName; // ���������� ��� ������ ��� ���������� �������
    public int price; // ���� ������
    public int access;
    public int select;
    public GameObject block; // ������, ������� ��������� �������
    public TextMeshProUGUI objectPriceText; // �����, ������������ ���� ������
    public TextMeshProUGUI coinsText; // �����, ������������ ���������� ����� ������
    private Color normalColor = Color.white;
    private Color notEnoughColor = Color.red;



    void Start()
    {
        PlayerPrefs.DeleteKey(objectName + "Access"); // ��� ����� ���������� ���������� �������
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
                ScoreManager.SendCoinsChanged(); // ��������� ���� ����������� � ��������� �����
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
