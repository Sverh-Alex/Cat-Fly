using System.Collections.Generic;
using UnityEngine;

public class PlayerChanger : MonoBehaviour
{
    public GameObject[] skins; // Ссылки на объекты скинов
    public int skinsId = 0;
    void Start()
    {
        LoadSkin(); // Загрузка последнего выбранного скина при запуске
        UpdateSkin(); // Обновление видимости скинов

        // skinsId = PlayerPrefs.GetInt("skin");
        //Instantiate(skins[skinsId], position, Quaternion.identity);
    }
    public void LoadSkin()
    {
        skinsId = PlayerPrefs.GetInt("skin", 0); // Получаем сохраненный ID скина. Если нет - возвращаем 0
    }
    public void UpdateSkin()
    {
        for (int i = 0; i <= skins.Length; i++)
        {
            if (skins[i] !=null)
            {
                skins[i].SetActive(i == skinsId); // Активируем скин, если его ID соответствует выбранному
            }
        }
    }

}
