using UnityEditor;
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
    public GameObject menuInGame;
    public GameObject loseMenu;

    void Start()
    {
        if (shop)
        {
            shop.SetActive(false);
        }
        if (bonusMenu)
        {
            bonusMenu.SetActive(false);
        }
        if (menuInGame)
        {
            menuInGame.SetActive(false);
        }
        
    }
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
    public void LVL_1()
    {
        SceneManager.LoadScene("LVL_1");
        Time.timeScale = 1;
    }
    public void LVL_2()
    {
        SceneManager.LoadScene("LVL_2");
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
    public void OnClickMenu()
    {
        menuInGame.SetActive(true);
        Time.timeScale = 0;
    }
    public void OnContinue()
    {
        Time.timeScale = 1;
        menuInGame.SetActive(false);
    }
    public void OnAlive()
    {
        Time.timeScale = 1;
        if (loseMenu)
        {
            loseMenu.SetActive(false);
        }
        ScoreManager.SendContinue();
    }
    public void OnPlay()
    {
        menuInGame.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnMenu()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1;
    }
    public void OnRestart()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
    }

}
