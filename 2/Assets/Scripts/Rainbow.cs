using UnityEngine;

public class Rainbow : MonoBehaviour
{
    [SerializeField] private float speed = 9.0f;  // ���������� �� ������ ��������
    //[SerializeField] private int damage = -1;  // ���������� �� ������ ���� �� ������������
    private Vector3 moveVector; // 
    private Vector2 baseResolution = new Vector2(1920, 1080); // ������� ����������
    [SerializeField] private GameObject effectDestroy; // ������ ��� �����

    void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // ������������ ����������� ���������������
        float scale = Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y);

        moveVector = new Vector3(speed * scale, 0); // ������� ��������� ��� ��������
        transform.localScale = new Vector3(scale / 2, scale / 2); // �������� ������ � ����������� ������

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(effectDestroy, collision.transform.position, Quaternion.identity); // ������ ������ ������ � ����� ���������������
        Destroy(gameObject);
    }

    void Update()
    {
        gameObject.transform.Translate(moveVector * Time.deltaTime); // Translate - ��� �����������
        if (transform.position.x > 12)
        {
            Destroy(gameObject);
        }
    }
}
