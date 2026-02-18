using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public GameObject UpStop, DownStop, LeftStop, RightStop;
    [SerializeField] private Cat catScript;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject buttonFire;
    [SerializeField] private Joystick joystick; // Может быть null

    private Vector2 baseResolution = new Vector2(1920, 1080);
    private Vector2 moveDirection;

    void Start()
    {
        bool isMobile = Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.IPhonePlayer
            || Application.platform == RuntimePlatform.WindowsEditor;

        if (isMobile) ScoreManager.SendTutorialApp();
        else ScoreManager.SendTutorialWeb();

        buttonFire.SetActive(isMobile);
    }

    void Update()
    {
        HandleInput();

    }

    void FixedUpdate()
    {
        float scale = GetScreenScale();
        MoveCharacter(scale);
    }

    void HandleInput()
    {
        Vector2 inputDirection = Vector2.zero;

        // ДЖОЙСТИК
        if (joystick != null)
        {
            inputDirection = joystick.Direction;
        }

        // КЛАВИАТУРА (приоритет!)
        Vector2 keyboardInput = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) keyboardInput.y += 1;
        if (Input.GetKey(KeyCode.S)) keyboardInput.y -= 1;
        if (Input.GetKey(KeyCode.A)) keyboardInput.x -= 1;
        if (Input.GetKey(KeyCode.D)) keyboardInput.x += 1;
        if (Input.GetKey(KeyCode.UpArrow)) keyboardInput.y += 1;
        if (Input.GetKey(KeyCode.DownArrow)) keyboardInput.y -= 1;
        if (Input.GetKey(KeyCode.LeftArrow)) keyboardInput.x -= 1;
        if (Input.GetKey(KeyCode.RightArrow)) keyboardInput.x += 1;

        // ПРИОРИТЕТ: клавиатура > джойстик
        if (keyboardInput != Vector2.zero)
            inputDirection = keyboardInput.normalized;
        else if (joystick != null)
            inputDirection = joystick.Direction;

        moveDirection = inputDirection;

        if (Input.GetKeyDown(KeyCode.Space))
            catScript.fire();
    }

    void MoveCharacter(float scale)
    {
        float horizontal = moveDirection.x * speed * scale / 2 * Time.fixedDeltaTime;
        float vertical = moveDirection.y * speed * scale / 2 * Time.fixedDeltaTime;

        MoveWithLimits(horizontal, vertical);
        UpdateAnimations(horizontal, vertical);
    }

    void MoveWithLimits(float horizontal, float vertical)
    {
        Vector3 targetPosition = transform.position + new Vector3(horizontal, vertical, 0);

        targetPosition.x = Mathf.Clamp(targetPosition.x, LeftStop.transform.position.x, RightStop.transform.position.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, DownStop.transform.position.y, UpStop.transform.position.y);

        transform.position = targetPosition;
    }

    void UpdateAnimations(float horizontal, float vertical)
    {
        bool isMoving = Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f;
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isDamage", false); // передаем значение из аниматора
    }

    float GetScreenScale()
    {
        return Mathf.Min(Screen.width / baseResolution.x, Screen.height / baseResolution.y);
    }
}
