using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cat : MonoBehaviour
{
    
    public static int lifeCounter = 0;
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
    [SerializeField] public float reloadTime = 0.1f;
    public Animator animator;
    private bool canFire = true; // ���� ���������� � ��������
    private GameObject scoreManager;
    public GameObject effect; // ������ �� ������ � Particle System ����
    public GameObject effectWool; // ������ �� ������ � Particle System ������
    
    public static int coinCounterLevel = 0;
    [SerializeField] private TextMeshProUGUI textCoinCounter; // UI ����� ��� ����������� �����

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
    public void UpdateLife(int life)
    {
        lifeCounter += life;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                hearts[i].gameObject.SetActive(i < lifeCounter);
            }
        }

        animator.SetBool("isAlive", false);

        if (lifeCounter <= 0)
        {
            lifeCounter = 0;
           // animator.SetBool("isDead", true);
            loseMenu.SetActive(true);
            Timer.Pause();
            ScoreManager.OnAlive += OnContinue;
            //StartCoroutine(LoadSceneAfterDelay(1f));
        }

    }
    public void AddLife()
    {
        lifeCounter += 1;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                hearts[i].gameObject.SetActive(i < lifeCounter);
            }
        }
    }
        IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        loseMenu.SetActive(true);
        animator.SetBool("isDead", false);
        
        Timer.Pause();
        ScoreManager.OnAlive += OnContinue;

    }
    private void OnContinue()
    {
        //animator.SetBool("isAlive", true);
        lifeCounter = 0;
        AddLife();

        // ������� ��� ������� ������ �� �����
        GameObject[] slippers = GameObject.FindGameObjectsWithTag("slipper");
        foreach (GameObject slipper in slippers)
        {
            Destroy(slipper);
        }

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
        //if (transform.position.y < -4.0f)
        //{
        //    loseMenu.SetActive(true);
        //    SceneManager.LoadScene("GameOverScene");
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision) // ������ � ������� �����������, �� ��� ���������
    {


        Destroy(collision.gameObject); 
        
        if(collision.gameObject.tag.Equals("slipper"))
        {
            explosionSound.Play(); 
            UpdateLife(collision.gameObject.GetComponent<Slipper>().damage);
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
            scoreManager.GetComponent<ScoreManager>().AddToScore();
            
            GameObject particleObject = Instantiate(effect, transform.position, Quaternion.identity);

            // ��������� ���������� Particle System
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
            if (particleSystem != null) // ���� ��������� Particle System ������
            {
                particleSystem.Emit(1); // ������ ������
                //particleSystem.Play(); // ������ ������� ������
                Destroy(particleObject, particleSystem.main.duration); // �������� ������� ������ ����� ����������
            }
            coinCounterLevel += 1;
            textCoinCounter.text = $"+{coinCounterLevel}";

            Debug.Log("Coin +1 " + coinCounterLevel);
        }
    }
}
