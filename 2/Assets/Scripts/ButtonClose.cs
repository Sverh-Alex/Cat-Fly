using UnityEngine;

public class ButtonClose : MonoBehaviour
{
    public GameObject image;
    
    public void CloseTab()
    {
        image.SetActive(false);
        //Time.timeScale = 1;
    }
    public void OpenTab()
    {
        image.SetActive(true);
        //Time.timeScale = 0;
    }
}
