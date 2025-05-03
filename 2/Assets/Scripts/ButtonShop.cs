using TMPro;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.UI;

public class ButtonShop : MonoBehaviour
{
    public string objectName;
    public int price;
    public int access;
    public int select;
    public GameObject block;
    public TextMeshProUGUI objectPriceText;
    public TextMeshProUGUI coinsText;

    void Start()
    {
        PlayerPrefs.DeleteKey(objectName + "Access"); // для теста сбрасываем сохранение покупки
        //PlayerPrefs.SetInt("coins", 10);
        coinsText.text = PlayerPrefs.GetInt("coins").ToString();
        AccessUpdate();
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
    }
}
