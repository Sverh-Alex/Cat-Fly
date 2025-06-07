using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public GameObject UpStop;
    public GameObject DownStop;
    public GameObject LeftStop;
    public GameObject RightStop;
    [SerializeField] private Cat catScript;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject buttonFire;

    private Vector2 baseResolution = new Vector2(1920, 1080);
    private Vector2 touchStartPosition;
    private Vector2 moveDirection;
    private Vector2 touchMoveDirection = Vector2.zero;
    private Vector2 keyboardMoveDirection = Vector2.zero;

    void Start()
    {
        bool isMobile = Application.platform == RuntimePlatform.Android ||
                      Application.platform == RuntimePlatform.WindowsEditor;

        buttonFire.SetActive(isMobile);
    }

    void Update()
    {
        HandleTouchInput();
        HandleKeyboardInput();
    }

    void FixedUpdate()
    {
        float scale = GetScreenScale();
        MoveCharacter(scale);
        HandleKeyboardInput();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    Vector2 delta = touch.position - touchStartPosition;
                    touchMoveDirection = delta.normalized;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    touchMoveDirection = Vector2.zero;
                    break;
            }
        }
        else
        {
            touchMoveDirection = Vector2.zero;
        }
    }

    void HandleKeyboardInput()
    {
        Vector2 keyboardInput = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) keyboardInput.y += 1;
        if (Input.GetKey(KeyCode.S)) keyboardInput.y -= 1;
        if (Input.GetKey(KeyCode.A)) keyboardInput.x -= 1;
        if (Input.GetKey(KeyCode.D)) keyboardInput.x += 1;

        if (keyboardInput != Vector2.zero)
        {
            keyboardMoveDirection = keyboardInput.normalized;
        }
        else
        {
            keyboardMoveDirection = Vector2.zero;
        }

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

        float horizontal = moveDirection.x * speed * scale * Time.deltaTime;
        float vertical = moveDirection.y * speed * scale * Time.deltaTime;

        MoveWithLimits(horizontal, vertical);
        UpdateAnimations(horizontal, vertical);
    }

    void MoveWithLimits(float horizontal, float vertical)
    {
        Vector3 targetPosition = transform.position +
            new Vector3(horizontal, vertical, 0);

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

    float GetScreenScale()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        return Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y);
    }
}
