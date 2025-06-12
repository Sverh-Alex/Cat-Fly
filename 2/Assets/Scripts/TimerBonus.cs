using UnityEngine;
using TMPro;
using System;

public class TimerBonus : MonoBehaviour
{
    public GameObject buttonBonus; // ������, ������� ���������� ����� ��������� �������
    public GameObject buttonTimerBonus; // ������, ������� ���������� ����� ��������� �������
    public TextMeshProUGUI textTimerBonus; // ����� ��� ����������� �������
    [SerializeField] float cdTime; // ����� ������� � �������� (��������, 300 ��� 5 �����)
    float remainingTime; // ������� �������� �������

    private const string TimerEndTimeKey = "TimerEndTime"; // ���� ��� ���������� ������� ���������

    private void Start()
    {
        buttonBonus.SetActive(false);
        buttonTimerBonus.SetActive(true);

        if (UnityEngine.PlayerPrefs.HasKey(TimerEndTimeKey))
        {
            string savedEndTimeStr = UnityEngine.PlayerPrefs.GetString(TimerEndTimeKey);
            DateTime endTime = DateTime.Parse(savedEndTimeStr);

            TimeSpan timeLeft = endTime - DateTime.Now;
            remainingTime = (float)timeLeft.TotalSeconds;

            if (remainingTime <= 0)
            {
                // ������ ��� �����
                remainingTime = 0;
                buttonBonus.SetActive(true);
                buttonTimerBonus.SetActive(false);
                UnityEngine.PlayerPrefs.DeleteKey(TimerEndTimeKey);
            }
        }
        else
        {
            // ������ �� �������, ���������� ������ ������
            remainingTime = 0;
            buttonBonus.SetActive(true);
            buttonTimerBonus.SetActive(false);
        }
    }
    public void OnButtonBonus()
    {
        // ��������� ������ ������
        remainingTime = cdTime;

        // ��������� ����� ��������� �������
        DateTime endTime = DateTime.Now.AddSeconds(remainingTime);
        UnityEngine.PlayerPrefs.SetString(TimerEndTimeKey, endTime.ToString());
        UnityEngine.PlayerPrefs.Save();

        buttonBonus.SetActive(false);
        buttonTimerBonus.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                buttonBonus.SetActive(true);
                buttonTimerBonus.SetActive(false);
                UnityEngine.PlayerPrefs.DeleteKey(TimerEndTimeKey);
            }

            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            textTimerBonus.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
