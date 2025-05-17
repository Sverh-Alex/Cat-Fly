using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static event Action OnCoinsChanged; // �������, ������� ���������� ����������� �� ��������� ���������� �����

    [SerializeField] private TextMeshProUGUI score; // UI ����� ��� ����������� �����
    [SerializeField] private int addBonus = 0; // ����� �� ������� ��������
    [SerializeField] private int addBonusADS = 0; // ����� �� �������� �������
    [SerializeField] private GameObject effectPSClick; // ������ ��� �����
    [SerializeField] private GameObject effectPSCoin; // ������ �������

    public static void SendCoinsChanged()
    {
        OnCoinsChanged.Invoke();

    }
    void Start()
    {
        effectPSCoin.SetActive(false); // ���������� ������ ������� ��������

        int coins = PlayerPrefs.GetInt("coins"); // �������� ������� ���������� ����� �� ����������
        score.text = coins.ToString();
                
    }
    private void Test()
    {
        Debug.Log("������� ���������");
       
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
    }

    public void AddBonus() // ��������� �������� ������ (��������, �� ��������)
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonus;
        UpdateCoins(coins);

        Instantiate(effectPSClick); // ������ ������ �����
        effectPSCoin.SetActive(true); // �������� ������ �������
    }

    public void AddBonusADS() // ��������� �������� ������ �� �������� �������
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonusADS;
        UpdateCoins(coins);

        Instantiate(effectPSClick); // ������ ������ �����
        effectPSCoin.SetActive(true); // �������� ������ �������
    }
}
