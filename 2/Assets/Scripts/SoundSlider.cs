using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class SoundSlider : MonoBehaviour
{
    public AudioSource[] soundSources2;   // Все источники звука (музыка + эффекты)
    public Slider volumeSlider;          // Слайдер общей громкости
    private float masterVolume = 1f;     // Общая громкость (0–1)

    void Start()
    {
        // Загружаем сохранённую громкость, по умолчанию 1
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = masterVolume;

            // Подписываемся на изменение значения слайдера
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        ApplyVolume();
    }

    // Вызывается автоматически, когда двигаем слайдер
    public void OnVolumeChanged(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
        ApplyVolume();
    }

    // Применяем громкость ко всем источникам
    private void ApplyVolume()
    {
        if (soundSources2 == null) return;

        foreach (var source in soundSources2)
        {
            if (source == null) continue;
            source.volume = masterVolume; // 0 – тишина, 1 – максимум [web:12]
        }
    }
}
