using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;  // PluginYG v2

public class LanguageManager : MonoBehaviour
{
    private bool active = false;

    void Start()
    {
        // ПРИ КАЖДОМ ЗАПУСКЕ — дефолтный от SDK
        StartCoroutine(SetSdkLanguage());
    }

    //  ДЕФОЛТНЫЙ ЯЗЫК ОТ YANDEX SDK
    IEnumerator SetSdkLanguage()
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;

        // Ждём инициализации PluginYG (правильная проверка)
        yield return new WaitUntil(() => YG2.isSDKEnabled);  //  isSDKEnabled вместо Initialized [page:38]

        string sdkLang = YG2.lang;  // "ru", "en"
        var sdkLocale = LocalizationSettings.AvailableLocales.GetLocale(sdkLang);
        if (sdkLocale != null)
        {
            LocalizationSettings.SelectedLocale = sdkLocale;
            Debug.Log($"Установлен SDK-язык: {sdkLang}");
        }
        active = false;
    }

    // ВЫБОР ИЗ МЕНЮ (временный)
    public void ChangeLocale(int localeID)
    {
        if (active) return;
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        Debug.Log($"Временно выбран язык ID {localeID}");
        active = false;
        // При рестарте — обратно к SDK
    }

    void OnEnable()
    {
        YG2.onCorrectLang += OnYGChangeLang;
        YG2.onSwitchLang += OnYGChangeLang;
    }

    void OnYGChangeLang(string lang)
    {
        StartCoroutine(SetSdkLanguage());  // Всегда SDK при смене платформы
    }

    void OnDisable()
    {
        YG2.onCorrectLang -= OnYGChangeLang;
        YG2.onSwitchLang -= OnYGChangeLang;
    }
}
