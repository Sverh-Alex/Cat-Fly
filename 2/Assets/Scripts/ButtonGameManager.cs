using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ButtonGameManager : MonoBehaviour
{
    public GameObject shop;
    public GameObject catShop;
    public GameObject mainMenu;
    public GameObject bonusMenu;

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
        bonusMenu.SetActive(false); 
        shop.SetActive(true);
        ScoreManager.SendCoinsChanged();
    }
    public void MainMenu()
    {
        shop.SetActive(false);
        bonusMenu.SetActive(false);
    }
    public void BonusMenu()
    {
        shop.SetActive(false); 
        bonusMenu.SetActive(true);

    }
    public void CloseBonus()
    {
        bonusMenu.SetActive(false);
    }


}
