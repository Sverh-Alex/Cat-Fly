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
        // Если хотите, чтобы ориентация менялась динамически при повороте устройства,
        // раскомментируйте следующую строку
        SetOrientationBasedOnScreenSize();
    }

    void SetOrientationBasedOnScreenSize()
    {
        if (Screen.width < Screen.height)
        {
            // Если ширина меньше высоты - принудительно ставим горизонтальную ориентацию
            Screen.orientation = ScreenOrientation.LandscapeLeft;

            // Отключаем автоповорот в портретные режимы
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;

            // Включаем автоповорот в ландшафтные режимы
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
        }
        else
        {
            // Если ширина больше или равна высоте - разрешаем автоповорот
            Screen.orientation = ScreenOrientation.AutoRotation;

            // Включаем все направления автоповорота
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
        }
    }
}
