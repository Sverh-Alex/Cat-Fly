using UnityEngine;

public class Gift : MonoBehaviour
{
    [SerializeField] private float speedGift = 9.0f;
    private Vector3 moveVector;
    //private float speedGiftInterval = 2.0f; // частота появления подарка
    private float randomSpeed;
    [SerializeField] public int upBullet = +1;
    private Vector2 baseResolution = new Vector2(1920, 1080); // Базовое разрешение

    void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        // Рассчитываем коэффициент масштабирования
        float scale = Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y);

        randomSpeed = Random.Range(2, speedGift);
        moveVector = new Vector3(-randomSpeed * scale, 0);
        transform.localScale = new Vector3(scale / 5, scale / 5); // изменяем размер в зависимости экрана
    }
    private void OnTriggerEnter2D(Collider2D collision)   // обрабатывает столкновения с тапком
    {
       if (collision.gameObject.tag.Equals("rainbow"))
       {
        Destroy(gameObject);
       } 
    }

    void Update()
    {
        gameObject.transform.Translate(moveVector * Time.deltaTime);
        if (transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }
}
