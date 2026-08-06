using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = RedefineYG.PlayerPrefs;
//using static UnityEngine.InputManagerEntry;

public class ImageChangerMain : MonoBehaviour
{
    public Sprite[] skins; // Массив GameObject скинов
    [SerializeField] Image image; // изображение кота на главном экране
    private int skinId = 0;

    void Start()
    {
        // Загружаем сохранённый скин, по умолчанию 0
        skinId = PlayerPrefs.GetInt("skin", 0);
        
    }

    public void Update()
    {
        skinId = PlayerPrefs.GetInt("skin");
        if (image != null && skins != null && skinId < skins.Length)
        {
            image.sprite = skins[skinId];
            Debug.Log($"установлена картинка скина{skinId}");
        }
    }
}
