using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;


    void Start()
    {
        menuUI.SetActive(false);
    }
    public void OnClickMenu()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnPlay()
    {
        menuUI.SetActive(false);
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
