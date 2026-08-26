using UnityEngine;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class LevelsReset : MonoBehaviour
{
    [Header("Сброс при запуске сцены")]
    [SerializeField] private bool resetOnStart; // Выполнять выбранный сброс при запуске сцены

    [Header("Что сбрасывать")]
    [SerializeField] private bool resetAllStars; // Сбросить звёзды всех указанных уровней
    [SerializeField] private bool resetAllLevels; // Сбросить открытие всех указанных уровней

    [Header("Имена уровней")]
    [SerializeField] private string[] levelNames; // Имена уровней, например LVL_1, LVL_2, LVL_3
    private const string OpenSuffix = "_open"; // Суффикс ключа открытия уровня
    private const string StarsSuffix = "_stars"; // Суффикс ключа звёзд уровня

    private void Start()
    {
        if (!resetOnStart) // Проверяем, включён ли сброс при запуске
        {
            return; // Ничего не сбрасываем
        }
        ResetLevels(); // Выполняем выбранный сброс
        resetOnStart = false; // Выключаем флаг после выполнения
    }

    [ContextMenu("Reset Selected Progress")]
    public void ResetLevels()
    {
        if (!resetAllStars && !resetAllLevels) // Проверяем, выбрано ли хотя бы одно действие
        {
            Debug.LogWarning(
                "[LevelsReset] Не выбрано, что именно сбрасывать"
            ); // Предупреждаем об отсутствии выбора

            return; // Завершаем выполнение
        }

        if (levelNames == null || levelNames.Length == 0) // Проверяем массив уровней
        {
            Debug.LogWarning(
                "[LevelsReset] Массив levelNames пуст"
            ); // Предупреждаем об отсутствии уровней

            return; // Завершаем выполнение
        }

        int resetLevelsCount = 0; // Считаем количество обработанных уровней

        foreach (string levelName in levelNames) // Перебираем все уровни из массива
        {
            if (string.IsNullOrWhiteSpace(levelName)) // Проверяем имя уровня
            {
                Debug.LogWarning(
                    "[LevelsReset] Найдено пустое имя уровня"
                ); // Предупреждаем о пустом имени

                continue; // Переходим к следующему элементу
            }

            string cleanLevelName = levelName.Trim(); // Убираем пробелы вокруг имени

            if (resetAllStars) // Проверяем, нужно ли сбросить звёзды
            {
                string starsKey = cleanLevelName + StarsSuffix; // Формируем ключ звёзд

                PlayerPrefs.DeleteKey(starsKey); // Удаляем звёзды уровня

                Debug.Log(
                    $"[LevelsReset] Удалён ключ звёзд: {starsKey}"
                ); // Выводим информацию об удалении
            }

            if (resetAllLevels) // Проверяем, нужно ли сбросить открытие уровней
            {
                string openKey = cleanLevelName + OpenSuffix; // Формируем ключ открытия

                PlayerPrefs.DeleteKey(openKey); // Удаляем открытие уровня

                Debug.Log(
                    $"[LevelsReset] Удалён ключ открытия: {openKey}"
                ); // Выводим информацию об удалении
            }

            resetLevelsCount++; // Увеличиваем количество обработанных уровней
        }

        PlayerPrefs.Save(); // Сохраняем изменения в PlayerPrefs

        Debug.Log(
            $"[LevelsReset] Сброс завершён. Обработано уровней: {resetLevelsCount}"
        ); // Выводим итоговую информацию
    }
}