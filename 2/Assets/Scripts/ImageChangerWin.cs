using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputManagerEntry;

public class ImageChangerWin : MonoBehaviour
{
    public Sprite[] images; // ������ �������� ��� ������
    public int imageId = 0;
    [SerializeField] Image win3stars; // ��������� Image, � �� GameObject
    [SerializeField] Image win2stars; // ��������� Image, � �� GameObject
    [SerializeField] Image win1stars; // ��������� Image, � �� GameObject

    void Start()
    {
        LoadImageWin();
        UpdateImageWin();
    }

    private void LoadImageWin()
    {
        imageId = PlayerPrefs.GetInt("skin", 0);
    }

    public void UpdateImageWin()
    {
        if (win3stars != null && images != null && imageId < images.Length)
        {
            win3stars.sprite = images[imageId];
        }
        if (win2stars != null && images != null && imageId < images.Length)
        {
            win2stars.sprite = images[imageId];
        }
        if (win1stars != null && images != null && imageId < images.Length)
        {
            win1stars.sprite = images[imageId];
        }
    }
}
