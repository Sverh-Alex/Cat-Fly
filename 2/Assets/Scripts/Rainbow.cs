using UnityEngine;

// Патрон игрока, который использует пул
public class Rainbow : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 9.0f;        // базовая скорость патрона
    private Vector3 moveVector;                               // вектор движения (вправо)
    private Vector2 baseResolution = new Vector2(1920, 1080); // базовое разрешение
    [SerializeField] private GameObject effectDestroy;        // эффект при попадании
    [SerializeField] private AudioSource explSource;          // звук взрыва (child объект)

    // Метод инициализации из пула (вызывается при каждом выстреле)
    public void Init(float speed)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Рассчитываем коэффициент масштабирования
        float scale = Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y);

        moveVector = new Vector3(speed * scale, 0, 0);      // вектор движения вправо
        transform.localScale = new Vector3(scale / 2, scale / 2, 1f); // размер патрона
    }

    private void Start()
    {
        // Инициализация при первом создании (для префаба)
        Init(baseSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, есть ли звук взрыва
        if (explSource != null)
        {
            explSource.transform.SetParent(null);           // отвязываем звук от патрона
            explSource.Play();                              // воспроизводим звук
            Destroy(explSource.gameObject, 2f);             // уничтожаем звук через 2 секунды
        }

        // Создаём эффект разрушения в точке столкновения
        if (effectDestroy != null)
        {
            Instantiate(effectDestroy, collision.transform.position, Quaternion.identity);
        }

        // Возвращаем патрон в пул вместо уничтожения
        ReturnToPool();
    }

    private void Update()
    {
        transform.Translate(moveVector * Time.deltaTime);   // двигаем патрон вправо

        // Если ушёл далеко вправо за экран — возвращаем в пул
        if (transform.position.x > 15f)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool("Rainbow", gameObject);
    }

}