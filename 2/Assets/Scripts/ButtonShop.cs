using TMPro;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShop : MonoBehaviour
{
    public string objectName; // Уникальное имя товара для сохранения доступа
    public int price; // Цена товара
    public int access;
    public int select;
    public GameObject block; // Объект, который блокирует покупку (например, затемнённая панель)
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
        UpdatePriceUI();
    }
    void UpdatePriceUI()
    {
        int coins = PlayerPrefs.GetInt("coins");
        objectPriceText.text = price.ToString();
        objectPriceText.color = coins < price ? notEnoughColor : normalColor;
    }
    void AccessUpdate()
    {
        access = PlayerPrefs.GetInt(objectName + "Access");
        objectPriceText.text = price.ToString();

        if (access == 1)
        {
            block.SetActive(false);
            objectPriceText.gameObject.SetActive(false);
        }
    }
    void SelectUpdate()
    {
        select = 1;
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

            }
        }
        UpdatePriceUI();
    }
}
