using UnityEngine;
using UnityEngine.UI;

public class ButtonToggleManager : MonoBehaviour
{
    public Button[] buttons; // ��� ������ �� �����
    public int id;
    public int selectId = 0;

    void Start()
    {
        // ��������� ������ ����������� ������ �� ������ (���� ����)
        selectId = PlayerPrefs.GetInt("SelectButton", 0);
        UpdateButtons();
    }

    // ���� ����� ���������� ��� ������� �� ������
    public void OnButtonClick(int buttonIndex)
    {
        selectId = buttonIndex;  // ����������, ����� ������ ���������
        PlayerPrefs.SetInt("SelectButton", selectId);  // ��������� � ������
        PlayerPrefs.Save();

        UpdateButtons();  // ��������� ��������� ������
    }

    // �������� ��� ������, ����� �����������
    void UpdateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(i != selectId);
        }
    }
}
