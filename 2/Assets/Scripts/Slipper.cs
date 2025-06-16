using UnityEngine;

public class Slipper : MonoBehaviour
{
    [SerializeField] private float speed = 9.0f;  // Показываем на панели скорость
    [SerializeField] public int damage = -1;  // Показываем на панели вред от столкновения
    private Vector3 moveVector; 
    private float randomSpeed;
    private GameObject scoreManager;
    private Vector2 baseResolution = new Vector2(1920, 1080); // Базовое разрешение
    private int koff = 8;
    private int koffSpeed = 15/10;

    void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        // Рассчитываем коэффициент масштабирования
        float scale = Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y);

        randomSpeed = Random.Range(2, speed); // рандомная скорость
        moveVector = new Vector3(-randomSpeed * scale / koffSpeed, 0); // создаем перменную для скорости
        transform.localScale = new Vector3(scale / koff, scale / koff); // изменяем размер в зависимости экрана
        
        scoreManager = GameObject.Find("ScoreManager");

    }
    
    private void OnTriggerEnter2D(Collider2D collision)   // обрабатывает столкновения с тапком
    {
        if (collision.gameObject.tag.Equals("rainbow"))
        {
            Destroy(gameObject);
        }
        //Destroy(gameObject);
        //collision.gameObject.GetComponent<Cat>().updateLife(damage); // путь до значения жизней
        //collision.gameObject.GetComponent<ParticleSystem>().Play(); // путь до значения жизней
    }

    void Update()
    {
        gameObject.transform.Translate(moveVector * Time.deltaTime); // Translate - это переместить
        if (transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }
}
