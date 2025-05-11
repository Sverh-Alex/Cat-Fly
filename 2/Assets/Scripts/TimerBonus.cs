using UnityEngine;
using TMPro;

public class TimerBonus : MonoBehaviour
{
    public GameObject buttonBonus;
    public GameObject buttonTimerBonus;
    public TextMeshProUGUI textTimerBonus;
    [SerializeField] float cdTime;
    float remainingTime;

    private void Start()
    {
        buttonBonus.SetActive(false);
        buttonTimerBonus.SetActive(true);
        remainingTime = cdTime;
    }
    public void OnButtonBonus()
    {
        buttonBonus.SetActive(false);
        buttonTimerBonus.SetActive(true);
        remainingTime = cdTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            textTimerBonus.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            remainingTime = 0;
            buttonBonus.SetActive(true);
            buttonTimerBonus.SetActive(false);
        }
    }
}
