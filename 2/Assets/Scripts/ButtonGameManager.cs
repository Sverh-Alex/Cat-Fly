using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class ButtonGameManager : MonoBehaviour
{
    public GameObject shop;
    public GameObject catShop;
    public GameObject mainMenu;
    public GameObject bonusMenu;
    public GameObject menuInGame;
    public GameObject loseMenu;
    public GameObject levels;
    [SerializeField] private GameObject effectPSClick; // эффект при клике
    [SerializeField] private GameObject[] objectOff; // объекты которые отключаются


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
        if (levels)
        {
            levels.SetActive(false);
        }

    }
    public void onGameStart()
    {
        SceneManager.LoadScene("GameScene");
        EffectClick();
    }
    public void onRestartScene()
    {
        SceneManager.LoadScene("Start");
        EffectClick();
    }
    public void OnLevels()
    {
        levels.SetActive(true);
        EffectClick();
    }
    public void onGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
        EffectClick();
    }
    public void LVL_1()
    {
        SceneManager.LoadScene("LVL_1");
        Time.timeScale = 1;
        EffectClick();
    }
    public void LVL_2()
    {
        SceneManager.LoadScene("LVL_2");
        EffectClick();
    }
    public void CloseShop()
    {
        if (shop)
        {
            shop.SetActive(false);
        }
        if (levels)
        {
            levels.SetActive(false);
        }
        EffectClick();
    }

    public void CatShop()
    {
        
        bonusMenu.SetActive(false);
        levels.SetActive(false);
        shop.SetActive(true);
        ScoreManager.SendCoinsChanged();
        EffectClick();

    }
    public void MainMenu()
    {
         
        shop.SetActive(false);
        bonusMenu.SetActive(false);
        EffectClick();
    }
    public void BonusMenu()
    {
        shop.SetActive(false); 
        bonusMenu.SetActive(true);
        EffectClick();
    }
    public void CloseBonus()
    {
        bonusMenu.SetActive(false);
        EffectClick();
    }
    public void OnClickMenu()
    {
        menuInGame.SetActive(true);
        DisableAll();
        // Включаем или выключаем звуковые эффекты
        Time.timeScale = 0;
    }
    public void DisableAll()
    {
        foreach(GameObject i in objectOff)
        {
            if (i != null)
            {
               i.SetActive(false);
            }
        }
    }
    public void ActiveAll()
    {
        foreach (GameObject i in objectOff)
        {
            if (i != null)
            {
                i.SetActive(true);
            }
        }
    }
    public void OnContinue()
    {
        Time.timeScale = 1;
        menuInGame.SetActive(false);
        ActiveAll();
        EffectClick();

    }
    public void OnAlive()
    {
        Time.timeScale = 1;
        if (loseMenu)
        {
            loseMenu.SetActive(false);
        }
        ScoreManager.SendContinue();
        EffectClick();
    }
    public void OnPlay()
    {
        menuInGame.SetActive(false);
        Time.timeScale = 1;
        EffectClick();
    }
    public void OnMenu()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1;
        EffectClick();
    }
    public void OnRestart()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
        EffectClick();
    }
    public void EffectClick()
    {
        // Получаем позицию мыши в мировых координатах
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10f; // расстояние от камеры до плоскости, на которой создаём объект
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0f; // чтобы объект был на нужном слое

        // Создаём эффект в позиции мыши
        Instantiate(effectPSClick, worldPos, Quaternion.identity);
    }
}
