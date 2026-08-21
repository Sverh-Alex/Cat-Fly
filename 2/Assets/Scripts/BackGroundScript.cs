using System.Collections;  // Подключает Coroutine
using UnityEngine;  // Подключает Unity API

public class BackGroundScript : MonoBehaviour
{
    [SerializeField] private GameObject Fon1;  // Первый фон
    [SerializeField] private GameObject Fon2;  // Второй фон
    [SerializeField] private GameObject Fon3;  // Третий фон

    [SerializeField] private float speed1 = 9.0f;  // Скорость первого фона
    [SerializeField] private float speed2 = 6.0f;  // Скорость второго фона
    [SerializeField] private float speed3 = 3.0f;  // Скорость третьего фона

    [SerializeField, Min(0f)] private float startDelay = 0f;  // Задержка перед началом движения

    private bool movementStarted;  // Показывает, началось ли движение

    private void Start()
    {
        StartCoroutine(StartMovementAfterDelay());  // Запускает ожидание перед движением
    }

    private IEnumerator StartMovementAfterDelay()
    {
        if (startDelay > 0f)  // Проверяет наличие задержки
        {
            yield return new WaitForSecondsRealtime(startDelay);  // Ждёт заданное время
        }

        movementStarted = true;  // Разрешает движение фонов
    }

    private void Update()
    {
        if (!movementStarted)  // Проверяет, закончилась ли задержка
        {
            return;  // Не двигает объекты до окончания задержки
        }

        if (Fon1 != null)  // Проверяет первый фон
        {
            Fon1.transform.Translate(
                Vector3.left * speed1 * Time.deltaTime  // Двигает первый фон влево
            );
        }

        if (Fon2 != null)  // Проверяет второй фон
        {
            Fon2.transform.Translate(
                Vector3.left * speed2 * Time.deltaTime  // Двигает второй фон влево
            );
        }

        if (Fon3 != null)  // Проверяет третий фон
        {
            Fon3.transform.Translate(
                Vector3.left * speed3 * Time.deltaTime  // Двигает третий фон влево
            );
        }
    }
}