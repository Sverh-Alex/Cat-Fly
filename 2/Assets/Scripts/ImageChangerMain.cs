using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.InputManagerEntry;

public class ImageChangerMain : MonoBehaviour
{
    public GameObject[] skins; // ������ GameObject ������

    private int currentSkinId = 0;

    void Start()
    {
        // ��������� ���������� ����, �� ��������� 0
        currentSkinId = PlayerPrefs.GetInt("skin", 0);

        ShowSkin(currentSkinId);
    }

    public void ShowSkin(int skinId)
    {
        if (skins == null || skins.Length == 0)
            return;

        // ������������ ������ � �������� �������
        skinId = Mathf.Clamp(skinId, 0, skins.Length - 1);

        // ���������� ��� ����� � �������� ������ ���������
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i] != null)
                skins[i].SetActive(i == skinId);
        }
    }

    // ����� ��� ����� ����� � ���������� ������
    public void SetSkin(int skinId)
    {
        currentSkinId = Mathf.Clamp(skinId, 0, skins.Length - 1);
        PlayerPrefs.SetInt("skin", currentSkinId);
        PlayerPrefs.Save();

        ShowSkin(currentSkinId);
    }
}
