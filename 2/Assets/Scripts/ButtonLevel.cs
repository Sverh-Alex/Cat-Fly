using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLevel : MonoBehaviour
{
    [Header("Сцена этого уровня")]
    [SerializeField] private string scene;      // Имя сцены (LVL_1, LVL_2, ...)

    [Header("Имена для PlayerPrefs")]
    [SerializeField] private string levelName;      // Текущее имя уровня (для stars и open), например "LVL_1"
    [SerializeField] private string nextLevelName;  // Имя следующего уровня (для open), например "LVL_2"

    [Header("Звезды этого уровня")]
    [SerializeField] private GameObject stars0;
    [SerializeField] private GameObject stars1;
    [SerializeField] private GameObject stars2;
    [SerializeField] private GameObject stars3;

    [Header("Block следующего уровня")]
    [SerializeField] private GameObject nextLevelBlock; // Объект block на кнопке следующего уровня

    private const string OPEN_SUFFIX = "open";
    private const string STARS_SUFFIX = "stars";

    private void Start()
    {
        //PlayerPrefs.DeleteKey(levelName + "stars"); // для теста сбрасываем сохранение звезд
        //PlayerPrefs.DeleteKey(levelName + "open"); // для теста сбрасываем сохранение открытого уровня
        //PlayerPrefs.SetFloat(nextlevelName + "open", 0); // для теста закрываю блок следующего уровня
        //PlayerPrefs.GetFloat(nextlevelName + "open"); // для теста закрываю блок следующего уровня

        UpdateStars();
        UpdateNextLevelBlock();
    }

    // Показываем правильный объект (0/1/2/3 звезды) для ЭТОГО уровня
    private void UpdateStars()
    {
        int stars = PlayerPrefs.GetInt(levelName + STARS_SUFFIX, 0);

        if (stars0 != null) stars0.SetActive(stars == 0);
        if (stars1 != null) stars1.SetActive(stars == 1);
        if (stars2 != null) stars2.SetActive(stars == 2);
        if (stars3 != null) stars3.SetActive(stars == 3);
    }

    // Управляем блоком следующего уровня по флагу open этого следующего уровня
    private void UpdateNextLevelBlock()
    {
        if (nextLevelBlock == null || string.IsNullOrEmpty(nextLevelName))
            return;

        // Смотрим, открыт ли следующий уровень
        float openFlag = PlayerPrefs.GetFloat(nextLevelName + OPEN_SUFFIX, 0f);
        bool isNextOpen = openFlag == 1f;

        // Если следующий открыт — убираем block; если нет — включаем
        nextLevelBlock.SetActive(!isNextOpen);
    }

    // Загрузка сцены этого уровня
    public void ChangeScene()
    {
        if (string.IsNullOrEmpty(scene))
        {
            Debug.LogError($"[ButtonLevel] На {name} не задано имя сцены");
            return;
        }

        SceneManager.LoadScene(scene);
    }
}