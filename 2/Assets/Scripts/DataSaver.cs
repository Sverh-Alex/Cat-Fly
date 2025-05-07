using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public GameObject select;
    public string objectName;
    public int id;
    
    public void SaveInt()
    {
        PlayerPrefs.SetInt("skin", id);
    }

}
