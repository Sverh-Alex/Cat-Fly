using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class ShopSkins : MonoBehaviour
{
    public GameObject[] activSkins; // ������ �� ������� ������ � ��������
    public int skinsId = 0;
    public int newSkinID = 0;
    void Start()
    {
        LoadSkin(); // �������� ���������� ���������� ����� ��� �������
        ActivateSkin(); // ���������� ��������� ������

        // skinsId = PlayerPrefs.GetInt("skin");
        //Instantiate(skins[skinsId], position, Quaternion.identity);
    }
    public void LoadSkin()
    {
        skinsId = PlayerPrefs.GetInt("skin", 0); // �������� ����������� ID �����. ���� ��� - ���������� 0
    }
    public void ActivateSkin()
    {
        if (newSkinID == skinsId) return;  // ���� ��������� ���� ��� �������, �� ������ �� ������

        if (skinsId >= 0 && skinsId < activSkins.Length)
            activSkins[skinsId].SetActive(false);  // ��������� ������� �������� ����

        if (newSkinID >= 0 && newSkinID < activSkins.Length)
            activSkins[newSkinID].SetActive(true);           // �������� ����� ����

        skinsId = newSkinID;                   // ����������, ����� ���� ������ �������
      
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
