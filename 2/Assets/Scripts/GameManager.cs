using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void onGameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void onRestartScene()
    {
        SceneManager.LoadScene("Start");
    }
    public void onGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }
    public void LVL1()
    {
        SceneManager.LoadScene("Start");
    }
}
