using UnityEngine;

public class ButtonClose : MonoBehaviour
{
    public GameObject image;
    
    public void CloseTab()
    {
        image.SetActive(false);
    }
    public void OpenTab()
    {
        image.SetActive(true);
    }
}
