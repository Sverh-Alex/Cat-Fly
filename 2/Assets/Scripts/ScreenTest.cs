using UnityEngine;

public class ScreenTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetOrientationBasedOnScreenSize();
    }

    void Update()
    {
        // ���� ������, ����� ���������� �������� ����������� ��� �������� ����������,
        // ���������������� ��������� ������
        SetOrientationBasedOnScreenSize();
    }

    void SetOrientationBasedOnScreenSize()
    {
        if (Screen.width < Screen.height)
        {
            // ���� ������ ������ ������ - ������������� ������ �������������� ����������
            Screen.orientation = ScreenOrientation.LandscapeLeft;

            // ��������� ����������� � ���������� ������
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;

            // �������� ����������� � ����������� ������
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
        }
        else
        {
            // ���� ������ ������ ��� ����� ������ - ��������� �����������
            Screen.orientation = ScreenOrientation.AutoRotation;

            // �������� ��� ����������� ������������
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
        }
    }
}
