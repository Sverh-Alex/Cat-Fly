using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Скорость движения персонажа
    public GameObject UpStop; // Верхняя граница движения
    public GameObject DownStop; // Нижняя граница движения
    public GameObject LeftStop; // Левая граница движения
    public GameObject RightStop; // Правая граница движения
    [SerializeField] private Cat catScript; // Ссылка на скрипт, управляющий "котом", для выполнения действия fire
    [SerializeField] private Animator animator; // Аниматор для управления анимацией персонажа
    [SerializeField] private GameObject buttonFire; // Кнопка огня для мобильных устройств

    private Vector2 baseResolution = new Vector2(1920, 1080); // Базовое разрешение для масштабирования движения
    private Vector2 touchStartPosition; // Позиция касания в момент начала
    private Vector2 moveDirection; // Итоговое направление движения
    private Vector2 touchMoveDirection = Vector2.zero; // Направление движения с касания
    private Vector2 keyboardMoveDirection = Vector2.zero; // Направление движения с клавиатуры

    void Start()
    {
        bool isMobile = // Определение платформы — мобильная или нет
            Application.platform == RuntimePlatform.Android 
            || Application.platform == RuntimePlatform.IPhonePlayer
            || Application.platform == RuntimePlatform.WindowsEditor; // WindowsEditor хоть и ПК, здесь учитывается для тестирования касания

        if (isMobile) // Отправка сигнала в ScoreManager в зависимости от платформы (для показа туториала)
        {
            ScoreManager.SendTutorialApp();
        }
        else
        {
            ScoreManager.SendTutorialWeb();
        }
            buttonFire.SetActive(isMobile); // Скрываем/показываем кнопку огня в зависимости от платформы (на мобильных показываем)
    }

    void Update()
    {
        HandleTouchInput(); // Обработка касаний
        HandleKeyboardInput(); // Обработка нажатий клавиатуры
    }

    void FixedUpdate()
    {
        float scale = GetScreenScale(); // Получение коэффициента масштабирования для разрешения экрана
        MoveCharacter(scale); // Перемещение персонажа с учетом масштабирования
        HandleKeyboardInput(); // Обработка клавиатуры во FixedUpdate для плавности (повторное)
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began: // Запомнить начальную позицию касания
                    touchStartPosition = touch.position;
                    break;

                case TouchPhase.Moved: // Рассчитать направление движения пальца
                    Vector2 delta = touch.position - touchStartPosition;
                    touchMoveDirection = delta.normalized;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled: // При окончании касания сбросить движение
                    touchMoveDirection = Vector2.zero;
                    break;
            }
        }
        else
        {
            touchMoveDirection = Vector2.zero; // Нет касания - движение по касанию отсутствует
        }
    }

    void HandleKeyboardInput()
    {
        Vector2 keyboardInput = Vector2.zero;

        // WASD
        if (Input.GetKey(KeyCode.W)) keyboardInput.y += 1;
        if (Input.GetKey(KeyCode.S)) keyboardInput.y -= 1;
        if (Input.GetKey(KeyCode.A)) keyboardInput.x -= 1;
        if (Input.GetKey(KeyCode.D)) keyboardInput.x += 1;

        // Стрелки
        if (Input.GetKey(KeyCode.UpArrow)) keyboardInput.y += 1;
        if (Input.GetKey(KeyCode.DownArrow)) keyboardInput.y -= 1;
        if (Input.GetKey(KeyCode.LeftArrow)) keyboardInput.x -= 1;
        if (Input.GetKey(KeyCode.RightArrow)) keyboardInput.x += 1;

        // Если есть ввод, нормализуем направление движения
        if (keyboardInput != Vector2.zero)
        {
            keyboardMoveDirection = keyboardInput.normalized;
        }
        else
        {
            keyboardMoveDirection = Vector2.zero;
        }

        // При нажатии пробела вызывается метод fire у объекта catScript (например, стрельба)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            catScript.fire();
        }
    }

    void MoveCharacter(float scale)
    {
        // Приоритет — касание, если есть движение пальцем, иначе клавиатура
        if (touchMoveDirection != Vector2.zero)
            moveDirection = touchMoveDirection;
        else
            moveDirection = keyboardMoveDirection;

        // Расчет смещения по горизонтали и вертикали с учетом скорости, масштаба экрана и времени кадра
        float horizontal = moveDirection.x * speed * scale/2 * Time.deltaTime;
        float vertical = moveDirection.y * speed * scale/2 * Time.deltaTime;

        MoveWithLimits(horizontal, vertical); // Перемещение объекта с ограничениями по краям
        UpdateAnimations(horizontal, vertical); // Обновление анимаций движения
    }

    void MoveWithLimits(float horizontal, float vertical)
    {
        Vector3 targetPosition = transform.position + new Vector3(horizontal, vertical, 0); // Новая позиция объекта с добавленным смещением
        
        // Ограничение позиции по осям X и Y в пределах установленных границ-объектов
        targetPosition.x = Mathf.Clamp(targetPosition.x,
            LeftStop.transform.position.x,
            RightStop.transform.position.x);

        targetPosition.y = Mathf.Clamp(targetPosition.y,
            DownStop.transform.position.y,
            UpStop.transform.position.y);

        transform.position = targetPosition;
    }

    void UpdateAnimations(float horizontal, float vertical)
    {
        bool isMoving = Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f;
        animator.SetBool("isMoving", false);

        /*if (isMoving)
        {
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
        }
        */
    }

    float GetScreenScale() // Расчет коэффициента масштабирования по минимальному соотношению текущего разрешения к базовому
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        return Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y);
    }
}
