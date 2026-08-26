using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerPrefs = RedefineYG.PlayerPrefs;

public class ButtonLevel : MonoBehaviour
{
    [Header("Сцена этого уровня")]
    [SerializeField] private string scene; // Имя сцены, например LVL_1

    [Header("Имена для сохранения")]
    [SerializeField] private string levelName; // Ключ текущего уровня, например LVL_1
    [SerializeField] private string nextLevelName; // Ключ следующего уровня, например LVL_2

    [Header("Звезды этого уровня")]
    [SerializeField] private GameObject stars0; // Объект отображения нуля звёзд
    [SerializeField] private GameObject stars1; // Объект отображения одной звезды
    [SerializeField] private GameObject stars2; // Объект отображения двух звёзд
    [SerializeField] private GameObject stars3; // Объект отображения трёх звёзд

    [Header("Блок следующего уровня")]
    [SerializeField] private GameObject nextLevelBlock; // Блокировка кнопки следующего уровня
    private const string OpenSuffix = "_open"; // Суффикс ключа открытия уровня
    private const string StarsSuffix = "_stars"; // Суффикс ключа количества звёзд
    private void Start()
    {
        UpdateStars(); // Обновляем отображение звёзд текущего уровня
        UpdateNextLevelBlock(); // Обновляем блокировку следующего уровня
    }

    private void UpdateStars()
    {
        if (string.IsNullOrWhiteSpace(levelName)) // Проверяем имя текущего уровня
        {
            Debug.LogError(
                $"[ButtonLevel] На объекте {name} не задан levelName"
            ); // Показываем ошибку настройки
            return; // Прерываем обновление звёзд
        }

        string starsKey = levelName + StarsSuffix; // Формируем ключ звёзд

        int stars = Mathf.Clamp(
            PlayerPrefs.GetInt(starsKey, 0),
            0,
            3
        ); // Загружаем и ограничиваем значение от 0 до 3

        if (stars0 != null) // Проверяем объект нулевого результата
        {
            stars0.SetActive(stars == 0); // Показываем объект при нуле звёзд
        }

        if (stars1 != null) // Проверяем объект одной звезды
        {
            stars1.SetActive(stars == 1); // Показываем объект при одной звезде
        }

        if (stars2 != null) // Проверяем объект двух звёзд
        {
            stars2.SetActive(stars == 2); // Показываем объект при двух звёздах
        }

        if (stars3 != null) // Проверяем объект трёх звёзд
        {
            stars3.SetActive(stars == 3); // Показываем объект при трёх звёздах
        }

        Debug.Log(
            $"[ButtonLevel] {starsKey} = {stars}"
        ); // Выводим загруженное количество звёзд
    }

    private void UpdateNextLevelBlock()
    {
        if (nextLevelBlock == null) // Проверяем наличие блока
        {
            return; // Нечего обновлять
        }

        if (string.IsNullOrWhiteSpace(nextLevelName)) // Проверяем имя следующего уровня
        {
            nextLevelBlock.SetActive(true); // Блокируем кнопку при отсутствии имени
            Debug.LogWarning(
                $"[ButtonLevel] На объекте {name} не задан nextLevelName"
            ); // Предупреждаем об ошибке настройки

            return; // Завершаем метод
        }

        string openKey = nextLevelName + OpenSuffix; // Формируем ключ открытия следующего уровня
        int openFlag = PlayerPrefs.GetInt(
            openKey,
            0
        ); // Загружаем флаг открытия

        bool isNextOpen = openFlag == 1; // Преобразуем флаг в логическое значение
        nextLevelBlock.SetActive(!isNextOpen); // Показываем блок только у закрытого уровня
        Debug.Log($"[ButtonLevel] {openKey} = {openFlag}"); // Выводим состояние следующего уровня
    }

    public void ChangeScene()
    {
        if (string.IsNullOrWhiteSpace(scene)) // Проверяем имя сцены
        {
            Debug.LogError(
                $"[ButtonLevel] На объекте {name} не задано имя сцены"
            ); // Показываем ошибку настройки
            return; // Не загружаем сцену
        }
        Time.timeScale = 1f; // Сбрасываем паузу перед загрузкой уровня
        SceneManager.LoadScene(scene); // Загружаем сцену уровня
    }
}