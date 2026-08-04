using GameAnalyticsSDK;
using UnityEngine;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class GameAnalyticsScript : MonoBehaviour
{
    // Галочка в инспекторе: если включить, то при старте очистятся только ключи аналитики.
    [SerializeField] private bool resetAnalyticsPrefsOnStart = false;

    // Ключи PlayerPrefs для аналитики.
    private const string GA_0_btn = "ga_0_btn_sent";
    private const string GA_tutor_coin = "ga_tutor_coin_sent";
    private const string GA_tutor_fish = "ga_tutor_fish_sent";
    private const string GA_tutor_dmg = "ga_tutor_dmg_sent";

    void Start()
    {
        // Если галочка включена в Inspector, сбрасываем нужные ключи.
        if (resetAnalyticsPrefsOnStart)
        {
            ResetAnalyticsPrefs();
        }

        GameAnalytics.Initialize();
    }

    // Отдельный метод, который удаляет только нужные ключи.
    private void ResetAnalyticsPrefs()
    {
        PlayerPrefs.DeleteKey(GA_0_btn);
        PlayerPrefs.DeleteKey(GA_tutor_coin);
        PlayerPrefs.DeleteKey(GA_tutor_fish);
        PlayerPrefs.DeleteKey(GA_tutor_dmg);

        PlayerPrefs.Save();

        Debug.Log("GA PlayerPrefs были сброшены");
    }

    public void GA0Btn()
    {

        if (PlayerPrefs.GetInt(GA_0_btn, 0) == 1) // Проверяем: если уже отправляли, то выходим и ничего не делаем.
        {
            return;
        }
        GameAnalytics.NewDesignEvent("0_btn");
        Debug.Log("GA0Btn отправлен");
        PlayerPrefs.SetInt(GA_0_btn, 1);
        PlayerPrefs.Save();
    }

    public void GATutorCoin()
    {
        if (PlayerPrefs.GetInt(GA_tutor_coin, 0) == 1)
        {
            return;
        }
        GameAnalytics.NewDesignEvent("tutor_coin");
        Debug.Log("GATutorCoin отправлен");
        PlayerPrefs.SetInt(GA_tutor_coin, 1);
        PlayerPrefs.Save();
    }
    public void GATutorFish()
    {
        if (PlayerPrefs.GetInt(GA_tutor_fish, 0) == 1)
        {
            return;
        }
        GameAnalytics.NewDesignEvent("tutor_fish");
        Debug.Log("GATutorFish отправлен");
        PlayerPrefs.SetInt(GA_tutor_fish, 1);
        PlayerPrefs.Save();
    }

    public void GATutorDmg()
    {
        if (PlayerPrefs.GetInt(GA_tutor_dmg, 0) == 1)
        {
            return;
        }
        GameAnalytics.NewDesignEvent("tutor_dmg");
        Debug.Log("GATutorDmg отправлен");
        PlayerPrefs.SetInt(GA_tutor_dmg, 1);
        PlayerPrefs.Save();
    }
}
