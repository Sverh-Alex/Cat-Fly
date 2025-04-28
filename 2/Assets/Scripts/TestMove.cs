using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TestMove : MonoBehaviour
{
    [SerializeField] private float speed;
    public GameObject UpStop;
    public GameObject DownStop;
    public GameObject LeftStop;
    public GameObject RightStop;
    [SerializeField] private GameObject cat;
    private Cat catScript;
    public Animator animator;
    public bool isKeyboardActive = true; // Флаг для блокировки клавиатуры
    private Vector2 baseResolution = new Vector2(1920, 1080); // Базовое разрешение
    public Joystick joystick;
    public GameObject fixedJoystick;
    public bool isJoystickActive = true;
    [SerializeField] private float speedJoystick = 1f;


    // private Vector2 moveVector;

    void Start()
    {
        catScript = cat.GetComponent<Cat>(); // "cat" указываем на каком конкретно объекте ищем скрипт
        animator = cat.GetComponent<Animator>();
        fixedJoystick.SetActive(false);

    }
    void MoveWithLimits(float horizontal, float vertical) // ограничения на передвижение для Джостика
    {
        // Рассчитываем новую позицию
        Vector3 targetPosition = transform.position + new Vector3(horizontal * Time.deltaTime, vertical * Time.deltaTime, 0);
        // Ограничиваем позицию по горизонтали
        targetPosition.x = Mathf.Clamp(targetPosition.x, LeftStop.transform.position.x, RightStop.transform.position.x);
        // Ограничиваем позицию по вертикали
        targetPosition.y = Mathf.Clamp(targetPosition.y, DownStop.transform.position.y, UpStop.transform.position.y);
        // Применяем новую позицию
        transform.position = targetPosition;
    }

    void FixedUpdate()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height; 
        float scale = Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y);
        float HorizontalInput = 0;
        float VerticalInput = 0;

        if (screenWidth > 800)
        {
            fixedJoystick.SetActive(false);
            isKeyboardActive = true;
        }
        if (screenWidth <= 800)
        {
            fixedJoystick.SetActive(true);
            isKeyboardActive = false;
        }

        if (isJoystickActive)
        {
            HorizontalInput = joystick.Horizontal * speedJoystick * speed * scale;
            VerticalInput = joystick.Vertical * speedJoystick * speed * scale;
            MoveWithLimits(HorizontalInput, VerticalInput);
        }

        if (isKeyboardActive)
        {

            if (Input.GetKey(KeyCode.W))
            {
                animator.SetBool("isMoving", false); // блокируем управление аниматором
                if (transform.position.y < UpStop.transform.position.y)
                {
                    // gameObject.transform.position += new Vector3(0, speed * 0.1f, 0);
                    transform.Translate(Vector2.up * scale * speed *  Time.deltaTime);
                }  
            }
            if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool("isMoving", false);
                if (transform.position.y > DownStop.transform.position.y)
                {
                    // gameObject.transform.position -= new Vector3(0, speed * 0.1f, 0);
                    transform.Translate(Vector2.down * scale * speed * Time.deltaTime);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                animator.SetBool("isMoving", false);
                if (transform.position.x > LeftStop.transform.position.x)
                {
                    // gameObject.transform.position -= new Vector3(0, speed * 0.1f, 0);
                    transform.Translate(Vector2.left * scale * speed * Time.deltaTime);
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                animator.SetBool("isMoving", false);
                if (transform.position.x < RightStop.transform.position.x)
                {
                    // gameObject.transform.position -= new Vector3(0, speed * 0.1f, 0);
                    transform.Translate(Vector2.right * scale * speed * Time.deltaTime);
                }
            }
            if (Input.GetKey(KeyCode.Space))
            {
                catScript.fire();
            }
        
        }
    }
}
