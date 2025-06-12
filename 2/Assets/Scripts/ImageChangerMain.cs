using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.InputManagerEntry;

public class ImageChangerMain : MonoBehaviour
{
    public GameObject[] skins; // Массив GameObject скинов

    private int currentSkinId = 0;

    void Start()
    {
        // Загружаем сохранённый скин, по умолчанию 0
        currentSkinId = PlayerPrefs.GetInt("skin", 0);

        ShowSkin(currentSkinId);
    }

    public void ShowSkin(int skinId)
    {
        if (skins == null || skins.Length == 0)
            return;

        // Ограничиваем индекс в пределах массива
        skinId = Mathf.Clamp(skinId, 0, skins.Length - 1);

        // Перебираем все скины и включаем только выбранный
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i] != null)
                skins[i].SetActive(i == skinId);
        }
    }

    // Метод для смены скина и сохранения выбора
    public void SetSkin(int skinId)
    {
        currentSkinId = Mathf.Clamp(skinId, 0, skins.Length - 1);
        PlayerPrefs.SetInt("skin", currentSkinId);
        PlayerPrefs.Save();

        ShowSkin(currentSkinId);
    }
}
