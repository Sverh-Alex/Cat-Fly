using UnityEngine;

public class DataSaver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject select;
    public string objectName;
    public int id;

    public void SaveInt()
    {
        PlayerPrefs.SetInt("skin", id);
        select.SetActive(false);
    }
}
