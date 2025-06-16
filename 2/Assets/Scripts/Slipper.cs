using UnityEngine;

public class Slipper : MonoBehaviour
{
    [SerializeField] private float speed = 9.0f;  // ���������� �� ������ ��������
    [SerializeField] public int damage = -1;  // ���������� �� ������ ���� �� ������������
    private Vector3 moveVector; 
    private float randomSpeed;
    private GameObject scoreManager;
    private Vector2 baseResolution = new Vector2(1920, 1080); // ������� ����������
    private int koff = 8;
    private int koffSpeed = 15/10;

    void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        // ������������ ����������� ���������������
        float scale = Mathf.Min(screenWidth / baseResolution.x, screenHeight / baseResolution.y);

        randomSpeed = Random.Range(2, speed); // ��������� ��������
        moveVector = new Vector3(-randomSpeed * scale / koffSpeed, 0); // ������� ��������� ��� ��������
        transform.localScale = new Vector3(scale / koff, scale / koff); // �������� ������ � ����������� ������
        
        scoreManager = GameObject.Find("ScoreManager");

    }
    
    private void OnTriggerEnter2D(Collider2D collision)   // ������������ ������������ � ������
    {
        if (collision.gameObject.tag.Equals("rainbow"))
        {
            Destroy(gameObject);
        }
        //Destroy(gameObject);
        //collision.gameObject.GetComponent<Cat>().updateLife(damage); // ���� �� �������� ������
        //collision.gameObject.GetComponent<ParticleSystem>().Play(); // ���� �� �������� ������
    }

    void Update()
    {
        gameObject.transform.Translate(moveVector * Time.deltaTime); // Translate - ��� �����������
        if (transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }
}
