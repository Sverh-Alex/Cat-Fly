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
    [SerializeField] private Image[] hearts; // массив куда добавляются изображения сердечек 
    [SerializeField] TextMeshProUGUI bulletCounterText;
    [SerializeField] AudioSource explosionSound;
    [SerializeField] AudioSource featherSound;
    [SerializeField] AudioSource woolSound;
    [SerializeField] public float reloadTime = 0.01f;
    public Animator animator;
    private bool canFire = true; // Флаг готовности к выстрелу
    private GameObject scoreManager;
    public GameObject effect; // Ссылка на объект с Particle System Перо
    public GameObject effectWool; // Ссылка на объект с Particle System Шерсть

    void Start()
    {
        animator = GetComponent<Animator>();
        // lifeStatus.text = lifeCounter.ToString(); // преобразуем в строку
        lifeCounter = hearts.Length; // чтобы не прописывать lifeCounter, а чтобы задавалось в зависимости от изображений
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
        lifeCounter += life; // обновляем жизни
        // lifeStatus.text = lifeCounter.ToString();
        for (int i = 0; i < hearts.Length; i++) // для кадой картинки перебираем значение (если 5 жизней показываем 5 сердечек)
        {
                hearts[i].enabled = i < lifeCounter; // показываем картинки
        }
        if (lifeCounter == 0)
        {
            animator.SetBool("isDead", true);  // показываем анимацию DeadCat из Аниматора
                                                //GetComponent<Animation>().Play("DeadCat"); // показываем анимацию DeadCat
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
        if(bulletCounter == 0) return; // когда меньше нуля, то выходит из функции
        if (!canFire) return;
        GameObject bulletObject = Instantiate(bullet);
        bulletObject.transform.position = spawnBulletPoint.transform.position;
        bulletCounter--; // отнимаем патрон 
        bulletCounterText.text = bulletCounter.ToString(); // обнавляем текст на экране 
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
        bulletCounter += bulletGift; // сколько патронов добавляет 
        bulletCounterText.text = bulletCounter.ToString(); // обнавляем текст на экране 
    }


    public void Update()
    {
        if (transform.position.y < -4.0f)
        {
            loseMenu.SetActive(true);
            //SceneManager.LoadScene("GameOverScene");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) // объект с которым столкнулись, мы его разрушаем
    {


        Destroy(collision.gameObject); 
        
        if(collision.gameObject.tag.Equals("slipper"))
        {
            explosionSound.Play(); 
            updateLife(collision.gameObject.GetComponent<Slipper>().damage);
            gameObject.GetComponent<ParticleSystem>().Play(); // путь до значения жизней
            animator.SetBool("isMoving", true); // передаем значение из аниматора
        }

        if (collision.gameObject.tag.Equals("gift"))
        {
            woolSound.Play();
            updateGift(collision.gameObject.GetComponent<Gift>().upBullet); // обновляет gift

            GameObject particleObject = Instantiate(effectWool, transform.position, Quaternion.identity);
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>(); // Получение компонента Particle System
            if (particleSystem != null) // Если компонент Particle System найден
            {
                particleSystem.Emit(1); // Выпуск частиц
                //particleSystem.Play(); // Запуск системы частиц
                Destroy(particleObject, particleSystem.main.duration); // Удаление объекта частиц после завершения
            }
        }

        if (collision.gameObject.tag.Equals("feather"))
        {
            featherSound.Play();
            scoreManager.GetComponent<ScoreManager>().addToScore();
            
            GameObject particleObject = Instantiate(effect, transform.position, Quaternion.identity);

            // Получение компонента Particle System
            ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
            if (particleSystem != null) // Если компонент Particle System найден
            {
                particleSystem.Emit(1); // Выпуск частиц
                //particleSystem.Play(); // Запуск системы частиц
                Destroy(particleObject, particleSystem.main.duration); // Удаление объекта частиц после завершения
            }
        }
    }
}
