using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float speed = 9.0f;  // ���������� �� ������ ��������
    private Vector3 moveVector; 
    private float randomSpeed;
    private Vector2 baseResolution = new Vector2(1920, 1080); // ������� ����������
    

    void Start()
    {

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // ������������ ����������� ���������������
        float scale = Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y);

        randomSpeed = Random.Range(2, speed); // ��������� �������� �� ���� �� ���������

        Color color = gameObject.GetComponent<SpriteRenderer>().color; // �������� ���� ���� �� ����������
        color.a = (randomSpeed / 10) + 0.1f; // ��������� ���� � ����������� �� ��������
        gameObject.GetComponent<SpriteRenderer>().color = color; // ������ ���� �� ���������

        moveVector = new Vector3(-randomSpeed * scale, 0); // ������� ��������� ��� ��������
        transform.localScale = new Vector3(scale * randomSpeed / 10, scale * randomSpeed / 10); // �������� ������ � ����������� �� ��������
        transform.Translate(new Vector3(0, 0, (-randomSpeed * 2) + 10 )); // ������� ��������� ��� ������ ����������� ��� ����� � ������
      
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(moveVector * Time.deltaTime); // Translate - ��� �����������
        if (transform.position.x < -12 )
        {
            Destroy(gameObject);
        }
    }
}
