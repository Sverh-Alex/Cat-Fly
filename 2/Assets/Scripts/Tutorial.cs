using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Туториалы для ПК / Web")]
    [SerializeField] private List<GameObject> webTutorials = new();   // Туториалы для ПК / Web

    [Header("Туториалы для мобильного приложения")]
    [SerializeField] private List<GameObject> appTutorials = new();   // Туториалы для мобилок

    [Header("Скрыть на ПК")]
    [SerializeField] private List<GameObject> hideOnPC = new();       // Элементы только для мобилки

    [Header("Настройки")]
    [SerializeField] private float closeTutorialDelay = 10f;          // Время показа туториала

    private bool isTutorialShown = false;                             // Флаг: туториал уже показан

    private void Start()
    {
        Debug.Log("[Tutorial] Start, платформа мобильная: " + Application.isMobilePlatform);

        // Если это не мобильная платформа (ПК / WebGL), прячем мобильные элементы
        if (!Application.isMobilePlatform)
        {
            SetActiveForList(hideOnPC, false);
        }

        // В начале скрываем все туториалы
        SetActiveForList(webTutorials, false);
        SetActiveForList(appTutorials, false);

        // Подписываемся на события ScoreManager
        Debug.Log("[Tutorial] Start, подписываемся на события");

        ScoreManager.OnTutorWeb += ShowWebTutorial;
        ScoreManager.OnTutorApp += ShowAppTutorial;
        Debug.Log("[Tutorial] Подписались на события OnTutorWeb и OnTutorApp");
    }

    private void OnDestroy()
    {
        ScoreManager.OnTutorWeb -= ShowWebTutorial;
        ScoreManager.OnTutorApp -= ShowAppTutorial;

        Debug.Log("[Tutorial] Отписались от событий");
    }

    // Показ веб‑туториалов
    public void ShowWebTutorial()
    {
        Debug.Log("[Tutorial] Вызван ShowWebTutorial");
        ShowTutorial(webTutorials, appTutorials);
    }

    // Показ мобильных туториалов
    public void ShowAppTutorial()
    {
        Debug.Log("[Tutorial] Вызван ShowAppTutorial");
        ShowTutorial(appTutorials, webTutorials);
    }

    // Общий метод показа
    private void ShowTutorial(List<GameObject> toShow, List<GameObject> toHide)
    {
        if (isTutorialShown)
        {
            Debug.Log("[Tutorial] Туториал уже показан, новый вызов проигнорирован");
            return;
        }

        isTutorialShown = true;

        // Скрываем другой набор
        SetActiveForList(toHide, false);

        // Показываем нужный набор
        SetActiveForList(toShow, true);

        Debug.Log("[Tutorial] Туториал показан, будет скрыт через " + closeTutorialDelay + " секунд");

        // Стартуем корутину автозакрытия
        StartCoroutine(HideTutorialAfterDelay());
    }

    // Включение/выключение списка объектов
    private void SetActiveForList(List<GameObject> list, bool value)
    {
        foreach (var obj in list)
        {
            if (obj != null)
            {
                obj.SetActive(value);
            }
            else
            {
                Debug.LogWarning("[Tutorial] В списке есть пустая ссылка (null)");
            }
        }
    }

    // Корутинa для автозакрытия
    private IEnumerator HideTutorialAfterDelay()
    {
        yield return new WaitForSeconds(closeTutorialDelay);

        SetActiveForList(webTutorials, false);
        SetActiveForList(appTutorials, false);

        isTutorialShown = false;

        Debug.Log("[Tutorial] Туториал закрыт");
    }
}
