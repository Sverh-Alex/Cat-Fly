using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.InputManagerEntry;

public class ImageChangerMain : MonoBehaviour
{
    public Sprite[] skins; // ������ GameObject ������
    [SerializeField] Image image; // ����������� ���� �� ������� ������
    private int skinId = 0;

    void Start()
    {
        // ��������� ���������� ����, �� ��������� 0
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

    //    // ������������ ������ � �������� �������
    //    skinId = Mathf.Clamp(skinId, 0, skins.Length - 1);

    //    // ���������� ��� ����� � �������� ������ ���������
    //    for (int i = 0; i < skins.Length; i++)
    //    {
    //        if (skins[i] != null)
    //            skins[i].SetActive(i == skinId);
    //    }
    //}

//    // ����� ��� ����� ����� � ���������� ������
//    public void SetSkin(int skinId)
//    {
//        skinId = Mathf.Clamp(skinId, 0, skins.Length - 1);
//        PlayerPrefs.SetInt("skinMain", skinId);
//        PlayerPrefs.Save();

//        ShowSkin(skinId);
//    }
}
