using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Туториалы для ПК / Web")]
    [SerializeField] private List<GameObject> webTutorials = new List<GameObject>();       // Туториалы, которые показываются на ПК / в браузере

    [Header("Туториалы для мобильного приложения")]
    [SerializeField] private List<GameObject> appTutorials = new List<GameObject>();       // Туториалы, которые показываются на мобильных устройствах

    [Header("Скрыть на ПК")]
    [SerializeField] private List<GameObject> hideOnPC = new List<GameObject>();           // Объекты, которые нужно выключить, если запуск на ПК

    [Header("Настройки")]
    [SerializeField] private float closeTutorialDelay = 10f;                               // Время показа туториала перед автозакрытием (в секундах)

    private bool isTutorialShown = false;                                                  // Флаг: сейчас какой-то туториал уже показан

    private void Start()
    {
        if (!Application.isMobilePlatform)                                                 // Если это не мобильная платформа (значит ПК)
        {
            SetActiveForList(hideOnPC, false);                                             // Отключаем все объекты, которые должны быть только на мобилке
        }

        SetActiveForList(webTutorials, false);                                             // В начале скрываем все веб-туториалы
        SetActiveForList(appTutorials, false);                                             // И все мобильные туториалы

        ScoreManager.OnTutorWeb += ShowWebTutorial;                                        // Подписываемся на событие показа веб-туториала
        ScoreManager.OnTutorApp += ShowAppTutorial;                                        // Подписываемся на событие показа мобильного туториала
    }

    private void OnDestroy()
    {
        ScoreManager.OnTutorWeb -= ShowWebTutorial;                                        // Отписываемся от события при уничтожении объекта
        ScoreManager.OnTutorApp -= ShowAppTutorial;                                        // Отписываемся от второго события
    }

    public void ShowWebTutorial() => ShowTutorial(webTutorials, appTutorials);             // Показать веб-туториалы и скрыть мобильные
    public void ShowAppTutorial() => ShowTutorial(appTutorials, webTutorials);             // Показать мобильные туториалы и скрыть веб

    private void ShowTutorial(List<GameObject> toShow, List<GameObject> toHide)
    {
        if (isTutorialShown) return;                                                       // Если туториал уже показан — выходим

        isTutorialShown = true;                                                            // Помечаем, что туториал активен

        SetActiveForList(toHide, false);                                                   // Скрываем список "другой" платформы
        SetActiveForList(toShow, true);                                                    // Показываем список текущей платформы

        StartCoroutine(HideTutorialAfterDelay());                                          // Запускаем корутину автозакрытия
    }

    private void SetActiveForList(List<GameObject> list, bool value)
    {
        foreach (var obj in list)                                                          // Проходим по всем объектам в списке
        {
            if (obj != null) obj.SetActive(value);                                         // Включаем/выключаем объект, если ссылка не пустая
        }
    }

    private IEnumerator HideTutorialAfterDelay()
    {
        yield return new WaitForSeconds(closeTutorialDelay);                               // Ждём указанное количество секунд

        SetActiveForList(webTutorials, false);                                             // Скрываем все веб-туториалы
        SetActiveForList(appTutorials, false);                                             // Скрываем все мобильные туториалы

        isTutorialShown = false;                                                           // Разрешаем снова показывать туториалы

        Debug.Log("Туториал закрыт");                                                      // Сообщение в консоль для отладки
    }
}
