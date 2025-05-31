using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static event Action OnCoinsChanged; // �������, ������� ���������� ����������� �� ��������� ���������� �����
    public static event Action OnAlive; // �������, ������� ���������� ����������� � ���������� ����
    public static event Action OnTutorWeb; // �������, � ��������� ����������
    public static event Action OnTutorApp; // �������, � ���������� ��������

    [SerializeField] private TextMeshProUGUI score; // UI ����� ��� ����������� �����
    [SerializeField] public int addBonus = 0; // ����� �� ������� ��������
    [SerializeField] private int addBonusADS = 0; // ����� �� �������� �������
    [SerializeField] private GameObject effectPSClick; // ������ ��� �����
    [SerializeField] private GameObject effectPSCoin; // ������ �������
    [SerializeField] private TextMeshProUGUI isAddBonusText; // UI ����� ��� ����������� �����

    public static void SendCoinsChanged()
    {
        OnCoinsChanged?.Invoke();
    
    }
    public static void SendContinue()
    {
        OnAlive.Invoke();

    }
    public static void SendTutorialWeb()
    {
        OnTutorWeb.Invoke();

    }
    public static void SendTutorialApp()
    {
        OnTutorApp.Invoke();

    }
    void Start()
    {
        effectPSCoin.SetActive(false); // ���������� ������ ������� ��������

        int coins = PlayerPrefs.GetInt("coins"); // �������� ������� ���������� ����� �� ����������
        score.text = coins.ToString();
        isAddBonusText.text = $"+{addBonus}";

    }

    private void UpdateCoins(int newCoins) // ��������������� ����� ���������� ����� � ������ �������
    {
        PlayerPrefs.SetInt("coins", newCoins); // ��������� ����� �������� �����
        score.text = newCoins.ToString(); // ��������� UI
       // OnCoinsChanged?.Invoke(newCoins); // �������� ������� ��� �����������
    }

    public void AddToScore() // ��������� 1 ������ � ��������� UI � �������
    {
        int coins = PlayerPrefs.GetInt("coins") + 1;
        UpdateCoins(coins);
        ScoreManager.SendCoinsChanged();
    }

    public void AddBonus() // ��������� �������� ������ (��������, �� ��������)
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonus;
        UpdateCoins(coins);

        Instantiate(effectPSClick); // ������ ������ �����
        effectPSCoin.SetActive(true); // �������� ������ �������
        ScoreManager.SendCoinsChanged();
    }

    public void AddBonusADS() // ��������� �������� ������ �� �������� �������
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonusADS;
        UpdateCoins(coins);

        Instantiate(effectPSClick); // ������ ������ �����
        effectPSCoin.SetActive(true); // �������� ������ �������
        SendCoinsChanged();
    }
}
