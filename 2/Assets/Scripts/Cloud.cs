using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float speed = 9.0f;  // Показываем на панели скорость
    private Vector3 moveVector; 
    private float randomSpeed;
    private Vector2 baseResolution = new Vector2(1920, 1080); // Базовое разрешение
    

    void Start()
    {

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Рассчитываем коэффициент масштабирования
        float scale = Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y);

        randomSpeed = Random.Range(2, speed); // рандомная скорость от двух до максимума

        Color color = gameObject.GetComponent<SpriteRenderer>().color; // выбираем ВЕСЬ цвет из Компонента
        color.a = (randomSpeed / 10) + 0.1f; // указываем цвет в зависимости от скорости
        gameObject.GetComponent<SpriteRenderer>().color = color; // меняем цвет на указанный

        moveVector = new Vector3(-randomSpeed * scale, 0); // создаем перменную для скорости
        transform.localScale = new Vector3(scale * randomSpeed / 10, scale * randomSpeed / 10); // изменяем размер в зависимости от скорости
        transform.Translate(new Vector3(0, 0, (-randomSpeed * 2) + 10 )); // глубина появления чем больше коэффициент тем ближе к камере
      
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(moveVector * Time.deltaTime); // Translate - это переместить
        if (transform.position.x < -12 )
        {
            Destroy(gameObject);
        }
    }
}
