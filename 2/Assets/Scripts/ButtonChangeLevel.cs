using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonChangeLevel : MonoBehaviour
{
    public string scene;
    public void ChangeScene()
    {
        SceneManager.LoadScene(scene);
    }
}
