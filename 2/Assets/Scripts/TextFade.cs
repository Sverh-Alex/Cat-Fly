using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TextFade : MonoBehaviour
{
    [SerializeField] private Graphic[] uiElements = new Graphic[10]; // ������ �� 10 UI ���������
    [SerializeField] private float fadeDuration = 2f;                // ����� ���������
    [SerializeField] private float delayBeforeFade = 3f;             // �������� ����� ������� ���������

    void Start()
    {
        // ������������� ���� ��������� ������ ��������������
        foreach (var element in uiElements)
        {
            if (element != null)
                element.canvasRenderer.SetAlpha(1f);
        }

        // ��������� ��������� � ���������
        Invoke(nameof(StartFadeOut), delayBeforeFade);
    }

    private void StartFadeOut()
    {
        foreach (var element in uiElements)
        {
            if (element != null)
                element.CrossFadeAlpha(0f, fadeDuration, false);
        }

        // ������� ������ ����� ��������� ��������� (���� �����)
        Destroy(gameObject, fadeDuration);
    }
}
