using System.Collections.Generic;
using UnityEngine;

public class PlayerChanger : MonoBehaviour
{
    public GameObject[] skins; // ������ �� ������� ������
    public int skinsId = 0;
    void Start()
    {
        LoadSkin(); // �������� ���������� ���������� ����� ��� �������
        UpdateSkin(); // ���������� ��������� ������

        // skinsId = PlayerPrefs.GetInt("skin");
        //Instantiate(skins[skinsId], position, Quaternion.identity);
    }
    public void LoadSkin()
    {
        skinsId = PlayerPrefs.GetInt("skin", 0); // �������� ����������� ID �����. ���� ��� - ���������� 0
    }
    public void UpdateSkin()
    {
        for (int i = 0; i <= skins.Length; i++)
        {
            if (skins[i] !=null)
            {
                skins[i].SetActive(i == skinsId); // ���������� ����, ���� ��� ID ������������� ����������
            }
        }
    }

}
