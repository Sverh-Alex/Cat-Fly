using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField] private GameObject revard; // ���� ��������� �2 �������
    [SerializeField] private TextMeshProUGUI textCatCoinValue; // ����� ����� ���������� �� �������


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
        revard.SetActive(false);
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

        EffectClick();
        effectPSCoin.SetActive(true); // �������� ������ �������
        ScoreManager.SendCoinsChanged();
    }

    public void AddBonusADS() // ��������� �������� ������ �� �������� �������
    {
        int coins = PlayerPrefs.GetInt("coins") + addBonusADS;
        UpdateCoins(coins);

        effectPSCoin.SetActive(true); // �������� ������ �������

        EffectClick();

        SendCoinsChanged();
    }
    public void OnOpenX2()
    {
        if (revard)
        {
            revard.SetActive(true);
            int value = Cat.coinCounterLevel;
            int valueX2 = value * 2;
            Debug.Log("catCoinValue" + valueX2);
            PlayerPrefs.SetInt("valueX2", valueX2);
            textCatCoinValue.text = $"+{valueX2}";

            EffectClick();
        }
    }
    public void CloseOnOpenX2()
    {
        if (revard)
        {
            revard.SetActive(false);
            int valuX2 = PlayerPrefs.GetInt("valueX2");
            int coins = PlayerPrefs.GetInt("coins") + valuX2;
            UpdateCoins(coins);
            effectPSCoin.SetActive(true); // �������� ������ �������

            EffectClick();
        }

    }
    public void EffectClick()
    {
        // �������� ������� ���� � ������� �����������
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10f; // ���������� �� ������ �� ���������, �� ������� ������ ������ (������� ��� ���� �����)
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0f; // ���� � ���� 2D, ����� ������ ��� �� ������ ����

        // ������ ������ � ������� ����
        Instantiate(effectPSClick, worldPos, Quaternion.identity);
    }
   
}
