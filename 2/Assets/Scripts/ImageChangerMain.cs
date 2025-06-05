using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.InputManagerEntry;

public class ImageChangerMain : MonoBehaviour
{
    public Sprite[] imgMain; // ������ �������� ��� ������
    public int imageMainId = 0;
    [SerializeField] Image imgMainCat; // ��������� Image, � �� GameObject

    void Start()
    {
        LoadImageMain();
        UpdateImageMain();
    }

    private void LoadImageMain()
    {
        imageMainId = PlayerPrefs.GetInt("skin", 0);
    }

    public void UpdateImageMain()
    {
        if (imgMainCat != null && imgMain != null && imageMainId < imgMain.Length)
        {
            imgMainCat.sprite = imgMain[imageMainId];
        }

    }
    private void Update()
    {
        LoadImageMain();
        UpdateImageMain();
    }
}
