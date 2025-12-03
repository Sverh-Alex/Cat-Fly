using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AutoSizeImage : MonoBehaviour
{
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            Image img = GetComponent<Image>();
            RectTransform rt = GetComponent<RectTransform>();
            if (img.sprite != null)
            {
                rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.sizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height);
            }
        }
    }
}
