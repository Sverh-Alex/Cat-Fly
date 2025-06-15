using UnityEngine;
using UnityEngine.UI;
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
        }
    }
    //public void ShowSkin(int skinId)
    //{
    //    if (skins == null || skins.Length == 0)
    //        return;

    //    // Ограничиваем индекс в пределах массива
    //    skinId = Mathf.Clamp(skinId, 0, skins.Length - 1);

    //    // Перебираем все скины и включаем только выбранный
    //    for (int i = 0; i < skins.Length; i++)
    //    {
    //        if (skins[i] != null)
    //            skins[i].SetActive(i == skinId);
    //    }
    //}

//    // Метод для смены скина и сохранения выбора
//    public void SetSkin(int skinId)
//    {
//        skinId = Mathf.Clamp(skinId, 0, skins.Length - 1);
//        PlayerPrefs.SetInt("skinMain", skinId);
//        PlayerPrefs.Save();

//        ShowSkin(skinId);
//    }
}
