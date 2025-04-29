using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cat : MonoBehaviour
{
    
    public int lifeCounter = 0;
    [SerializeField] private int bulletCounter = 30;
    [SerializeField] private int bulletGift = 3;
    // [SerializeField] TextMeshProUGUI lifeStatus;
    [SerializeField] GameObject spawnBulletPoint;
    [SerializeField] GameObject bullet;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private Image[] hearts; // ������ ���� ����������� ����������� �������� 
    [SerializeField] TextMeshProUGUI bulletCounterText;
    [SerializeField] AudioSource explosionSound;
    [SerializeField] AudioSource featherSound;
    [SerializeField] AudioSource woolSound;
    [SerializeField] public float reloadTime = 0.01f;
    public Animator animator;
    private bool canFire = true; // ���� ���������� � ��������
    private GameObject scoreManager;
    public GameObject effect; // ������ �� ������ � Particle System ����
    public GameObject effectWool; // ������ �� ������ � Particle System ������

    void Start()
    {
        animator = GetComponent<Animator>();
        // lifeStatus.text = lifeCounter.ToString(); // ����������� � ������
        lifeCounter = hearts.Length; // ����� �� ����������� lifeCounter, � ����� ���������� � ����������� �� �����������
        bulletCounterText.text = bulletCounter.ToString();
        scoreManager = GameObject.Find("ScoreManager");
        loseMenu.SetActive(false);

    }
    public int GetLifeCounter()
    {
        return lifeCounter;
    }
    public void updateLife(int life)
    {
        lifeCounter += life; // ��������� �����
        // lifeStatus.text = lifeCounter.ToString();
        for (int i = 0; i < hearts.Length; i++) // ��� ����� �������� ���������� �������� (���� 5 ������ ���������� 5 ��������)
        {
                hearts[i].enabled = i < lifeCounter; // ���������� ��������
        }
        if (lifeCounter == 0)
        {
            animator.SetBool("isDead", true);  // ���������� �������� DeadCat �� ���������
                                                //GetComponent<Animation>().Play("DeadCat"); // ���������� �������� DeadCat
            StartCoroutine(LoadSceneAfterDelay(1f));
           
        }

    }

    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        loseMenu.SetActive(true);
        animator.SetBool("isDead", false);
        //SceneManager.LoadScene("GameOverScene");
    }

    public void fire()
    {
        if(bulletCounter == 0) return; // ����� ������ ����, �� ������� �� �������
        if (!canFire) return;
        GameObject bulletObject = Instantiate(bullet);
        bulletObject.transform.position = spawnBulletPoint.transform.position;
        bulletCounter--; // �������� ������ 
        bulletCounterText.text = bulletCounter.ToString(); // ��������� ����� �� ������ 
        canFire = false;
        StartCoroutine(Reload());
        return;
    }
    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        canFire = true;
    }

    public void updateGift(int upBullet)
    {
        bulletCounter += bulletGift; // ������� �������� ��������� 
        bulletCounterText.text = bulletCounter.ToString(); // ��������� ����� �� ������ 
    }


    public void Update()
    {
        if (transform.position.y < -4.0f)
        {
            loseMenu.SetActive(true);
            //SceneManager.LoadScene("GameOverScene");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) // ������ � ������� �����������, �� ��� ���������
    {


        Destroy(collision.gameObject); 
        
        if(collision.gameObject.tag.Equals("slipper"))
        {
            explosionSound.Play(); 
            updateLife(collision.gameObject.GetComponent<Slipper>().damage);
            gameObject.GetComponent<ParticleSystem>().Play(); // ���� �� �������� ������
            animator.SetBool("isMoving", true); // �������� �������� �� ���������
        }

        if (collision.gameObject.tag.Equals("gift"))
        {
            woolSound.Play();
            updateGift(collision.gameObject.GetComponent<Gift>().upBullet); // ��������� gift

            GameObject particleObject = Instantiate(effectWool, transform.position, Quaternion.identity);
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>(); // ��������� ���������� Particle System
            if (particleSystem != null) // ���� ��������� Particle System ������
            {
                particleSystem.Emit(1); // ������ ������
                //particleSystem.Play(); // ������ ������� ������
                Destroy(particleObject, particleSystem.main.duration); // �������� ������� ������ ����� ����������
            }
        }

        if (collision.gameObject.tag.Equals("feather"))
        {
            featherSound.Play();
            scoreManager.GetComponent<ScoreManager>().addToScore();
            
            GameObject particleObject = Instantiate(effect, transform.position, Quaternion.identity);

            // ��������� ���������� Particle System
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
            if (particleSystem != null) // ���� ��������� Particle System ������
            {
                particleSystem.Emit(1); // ������ ������
                //particleSystem.Play(); // ������ ������� ������
                Destroy(particleObject, particleSystem.main.duration); // �������� ������� ������ ����� ����������
            }
        }
    }
}
