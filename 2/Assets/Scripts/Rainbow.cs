using UnityEngine;

public class Rainbow : MonoBehaviour
{
    [SerializeField] private float speed = 9.0f;  // Показываем на панели скорость
    //[SerializeField] private int damage = -1;  // Показываем на панели вред от столкновения
    private Vector3 moveVector; // 
    private Vector2 baseResolution = new Vector2(1920, 1080); // Базовое разрешение
    [SerializeField] private GameObject effectDestroy; // эффект при клике

    void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Рассчитываем коэффициент масштабирования
        float scale = Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y);

        moveVector = new Vector3(speed * scale, 0); // создаем перменную для скорости
        transform.localScale = new Vector3(scale / 2, scale / 2); // изменяем размер в зависимости экрана

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(effectDestroy, collision.transform.position, Quaternion.identity); // содает префаб взрыва в месте соприкосновения
        Destroy(gameObject);
    }

    void Update()
    {
        gameObject.transform.Translate(moveVector * Time.deltaTime); // Translate - это переместить
        if (transform.position.x > 12)
        {
            Destroy(gameObject);
        }
    }
}
