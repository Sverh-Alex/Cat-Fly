using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] public string sceneName = "GameScene";
    public void OnButtonClick()
    {
        SceneManager.LoadScene(sceneName);
    }

}
