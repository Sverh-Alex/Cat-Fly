using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class ShopSkins : MonoBehaviour
{
    public GameObject[] activSkins; // Ссылки на объекты скинов в магазине
    public int skinsId = 0;
    public int newSkinID = 0;
    void Start()
    {
        LoadSkin(); // Загрузка последнего выбранного скина при запуске
        ActivateSkin(); // Обновление видимости скинов

        // skinsId = PlayerPrefs.GetInt("skin");
        //Instantiate(skins[skinsId], position, Quaternion.identity);
    }
    public void LoadSkin()
    {
        skinsId = PlayerPrefs.GetInt("skin", 0); // Получаем сохраненный ID скина. Если нет - возвращаем 0
    }
    public void ActivateSkin()
    {
        if (newSkinID == skinsId) return;  // Если выбранный скин уже активен, то ничего не делаем

        if (skinsId >= 0 && skinsId < activSkins.Length)
            activSkins[skinsId].SetActive(false);  // Отключаем текущий активный скин

        if (newSkinID >= 0 && newSkinID < activSkins.Length)
            activSkins[newSkinID].SetActive(true);           // Включаем новый скин

        skinsId = newSkinID;                   // Запоминаем, какой скин сейчас активен
      
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
