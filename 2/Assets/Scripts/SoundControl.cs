using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    public AudioSource[] soundSources;   // Массив источников звуков эффектов (если есть)
    public Button toggleButton;          // Кнопка для переключения звука
    public GameObject soundOnIconObject;           // Иконка звука включен
    public GameObject soundOffIconObject;          // Иконка звука выключен
    private bool isSoundOn;

    void Start()
    {
        // Загружаем сохранённое состояние, по умолчанию звук включён
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;

        ApplySoundState();

        // Назначаем обработчик нажатия на кнопку
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
        // Включаем или выключаем звуковые эффекты
        if (soundSources != null)
        {
            foreach (var source in soundSources)
            {
                if (source != null)
                    source.mute = !isSoundOn;
            }
        }

        // Меняем иконку кнопки
        if (soundOnIconObject != null && soundOffIconObject != null)
        {
            soundOnIconObject.SetActive(isSoundOn);
            soundOffIconObject.SetActive(!isSoundOn);
        }
    }
}
