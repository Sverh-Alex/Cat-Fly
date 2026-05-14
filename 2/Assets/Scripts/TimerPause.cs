using UnityEngine;

public class TimerPause : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Time.timeScale = 0f;
            }
    public void Continue()
    {
        Time.timeScale = 1f;
    }
}
