using UnityEngine;

public class LevelsReset : MonoBehaviour
{
    [Header("Сброс прогресса при старте сцены")]
    [SerializeField] private bool resetOnStart = false;

    [Header("Имена уровней для сброса")]
    [SerializeField] private string[] levelNames; // Например: LVL_1, LVL_2, LVL_3

    private const string OPEN_SUFFIX = "open";
    private const string STARS_SUFFIX = "stars";

    private void Start()
    {
        if (resetOnStart)
        {
            ResetLevels();
            // Один раз сбросили — выключаем, чтобы не сбрасывало каждый запуск
            resetOnStart = false;
        }
    }

    [ContextMenu("Reset Levels Now")]
    public void ResetLevels()
    {
        foreach (var levelName in levelNames)
        {
            if (string.IsNullOrEmpty(levelName))
                continue;

            PlayerPrefs.DeleteKey(levelName + STARS_SUFFIX);
            PlayerPrefs.DeleteKey(levelName + OPEN_SUFFIX);
        }

        PlayerPrefs.Save();
        Debug.Log("[LevelsReset] Прогресс уровней сброшен");
    }
}