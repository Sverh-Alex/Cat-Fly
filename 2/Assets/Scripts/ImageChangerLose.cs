using UnityEngine;
using UnityEngine.UI;

public class ImageChangerLose : MonoBehaviour
{
    public Sprite[] images; // массив спрайтов для скинов
    public int imageId = 0;
    [SerializeField] Image imgLose; // компонент Image, а не GameObject


    void Start()
    {
        LoadImageLose();
        UpdateImageLose();
    }

    private void LoadImageLose()
    {
        imageId = PlayerPrefs.GetInt("skin", 0);
    }

    public void UpdateImageLose()
    {
        if (imgLose != null && images != null && imageId < images.Length)
        {
            imgLose.sprite = images[imageId];
        }
    }
}
