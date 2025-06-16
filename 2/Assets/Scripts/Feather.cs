using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Feather : MonoBehaviour
{
    private Vector3 moveVector;
    [SerializeField] private float speed = 9.0f;  // Показываем на панели скорость
    private Vector2 baseResolution = new Vector2(1920, 1080); // Базовое разрешение
    private float randomSpeed;
    private GameObject scoreManager;
    private float scaleSize = 4; // коэффициент регулируем размер монеток

    void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float scale = Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y); // Рассчитываем коэффициент масштабирования

        randomSpeed = Random.Range(2, speed); // рандомная скорость
        moveVector = new Vector3(-randomSpeed * scale, 0); // создаем перменную для скорости
        transform.localScale = new Vector3(scale / scaleSize, scale / scaleSize); // изменяем размер в зависимости экрана
       
        scoreManager = GameObject.Find("ScoreManager"); 
    }
    private void OnTriggerEnter2D(Collider2D collision)   // обрабатывает столкновения с тапком
    {
        if (collision.gameObject.tag.Equals("rainbow"))
        {
            Destroy(gameObject);
        }
    }
        // Update is called once per frame
        void Update()
    {
        gameObject.transform.Translate(moveVector * Time.deltaTime);
            if (transform.position.x < -15)
            {
                //scoreManager.GetComponent<ScoreManager>().addToScore();
                Destroy(gameObject);
            }
    }
}
