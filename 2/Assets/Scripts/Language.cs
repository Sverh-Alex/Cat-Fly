using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
public class Language : MonoBehaviour
{
    private bool active = false; // если несколько раз подряд нажимать, будет проблема. Исключаем это
    public void ChangeLocale(int localeID)
    {
        if (active)
            return;
        StartCoroutine(SetLocale(localeID));
    }
    IEnumerator SetLocale(int _localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        active = false;
    }
}
