using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    public AudioSource[] soundSources;   // ������ ���������� ������ �������� (���� ����)
    public Button toggleButton;          // ������ ��� ������������ �����
    public GameObject soundOnIconObject;           // ������ ����� �������
    public GameObject soundOffIconObject;          // ������ ����� ��������
    private bool isSoundOn;

    void Start()
    {
        // ��������� ���������� ���������, �� ��������� ���� �������
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;

        ApplySoundState();

        // ��������� ���������� ������� �� ������
        toggleButton.onClick.AddListener(ToggleSound);
    }

    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplySoundState();
    }

    public void ApplySoundState()
    {
        // �������� ��� ��������� �������� �������
        if (soundSources != null)
        {
            foreach (var source in soundSources)
            {
                if (source != null)
                    source.mute = !isSoundOn;
            }
        }

        // ������ ������ ������
        if (soundOnIconObject != null && soundOffIconObject != null)
        {
            soundOnIconObject.SetActive(isSoundOn);
            soundOffIconObject.SetActive(!isSoundOn);
        }
    }
}
