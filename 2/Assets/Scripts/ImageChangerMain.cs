using System;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class ImageChangerMain : MonoBehaviour
{
    public static event Action<int> OnSkinChanged;

    [SerializeField] private Sprite[] skins;
    [SerializeField] private Image image;
    private int skinId = 0;

    void Start()
    {
        skinId = PlayerPrefs.GetInt("skin", 0);
        ApplySkin();

        // Подписываемся на событие смены скина
        OnSkinChanged += HandleSkinChanged;
    }

    private void OnDestroy()
    {
        OnSkinChanged -= HandleSkinChanged;
    }

    private void ApplySkin()
    {
        if (image != null && skins != null && skinId >= 0 && skinId < skins.Length)
        {
            image.sprite = skins[skinId];
            Debug.Log($"Установлена картинка скина {skinId}");
        }
        else
        {
            Debug.LogWarning($"Скин {skinId} не найден или не назначен");
        }
    }

    private void HandleSkinChanged(int newSkinId)
    {
        skinId = newSkinId;
        ApplySkin();
    }

    // Вызывай этот метод при смене скина из любого другого места
    public static void SetSkin(int newSkinId)
    {
        PlayerPrefs.SetInt("skin", newSkinId);
        PlayerPrefs.Save();
        OnSkinChanged?.Invoke(newSkinId);
    }
}