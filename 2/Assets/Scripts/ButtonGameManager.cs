using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class ButtonGameManager : MonoBehaviour
{
    public GameObject shop;
    public GameObject catShop;
    public GameObject mainMenu;

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
    public void CloseShop()
    {
        shop.SetActive(false);
    }
    public void CatShop()
    {
        shop.SetActive(true);
    }
    public void MainMenu()
    {
        shop.SetActive(false);
    }

}
