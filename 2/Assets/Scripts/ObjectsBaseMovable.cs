using UnityEngine;

// Базовый класс для всех летающих объектов (Cloud, Feather, Gift, Slipper)
public abstract class ObjectsBaseMovable : MonoBehaviour
{
    [SerializeField] protected float maxSpeed = 9f;       // максимальная скорость (задаётся в инспекторе)
    [SerializeField] protected float minSpeed = 2f;       // минимальная скорость (задаётся в инспекторе)

    protected float currentSpeed;                           // текущая скорость (выбирается случайно при Init)
    protected Vector3 moveVector;                           // вектор движения (влево)
    protected Vector2 baseResolution = new Vector2(1920, 1080); // базовое разрешение для расчёта масштаба

    // Метод инициализации из пула (вызывается при каждом «спавне»)
    public virtual void Init(float speed)
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);  // случайная скорость в диапазоне

        float screenWidth = Screen.width;                 // текущая ширина экрана
        float screenHeight = Screen.height;               // текущая высота экрана
        float scale = Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y); // коэффициент масштаба

        moveVector = new Vector3(-currentSpeed * scale, 0, 0); // вектор движения влево

        OnCustomInit(scale);                              // вызываем уникальную логику для каждого типа объекта
    }

    // Переопределяется в наследниках для уникальной логики (размер, цвет и т.д.)
    protected abstract void OnCustomInit(float scale);

    protected virtual void Update()
    {
        transform.Translate(moveVector * Time.deltaTime); // двигаем объект влево

        if (transform.position.x < -15f)                  // если ушёл далеко влево за экран
        {
            ReturnToPool();                               // возвращаем в пул
        }
    }

    protected virtual void ReturnToPool()
    {
        gameObject.SetActive(false);                      // выключаем объект (он вернётся в пул)
    }

    protected float GetScaledSpeed(float scale)
    {
        return currentSpeed * scale;                      // возвращаем скорость с учётом масштаба
    }
}