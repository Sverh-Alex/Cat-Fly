using UnityEngine;

public class SpamClouds : MonoBehaviour
{
    [SerializeField] private GameObject[] spamPoints;
    [SerializeField] private GameObject cloud;
    [SerializeField] private float spamInterval = 1.0f;
    [SerializeField] private GameObject slipper;
    [SerializeField] private float spamSlipperInterval = 2.0f;
    [SerializeField] private GameObject gift;
    [SerializeField] private float spamGiftInterval = 0.5f;
    [SerializeField] private GameObject feather;
    [SerializeField] private float spamFeatherInterval = 0.5f;


    void Start()
    {
        Invoke("SpamCloud", spamInterval);
        Invoke("SpamSlipper", spamSlipperInterval);
        Invoke("SpamGift", spamGiftInterval);
        Invoke("SpamFeather", spamFeatherInterval);

    }
    private void SpamGift()
    {
        GameObject gft = Instantiate(gift); // создали объект
        int index = UnityEngine.Random.Range(0, 7); // выбрали рандомное число
        Vector3 position = spamPoints[index].transform.position; // создали точку спавна
        gft.transform.position = position; // присвоили объекту точку спавна
        Invoke("SpamGift", spamGiftInterval); // вызвали метод снова (зациклили)

    }
    private void SpamCloud()
    {
        int index = UnityEngine.Random.Range(0, 7); // рандомное число от 0 до 4
        GameObject cl = Instantiate(cloud); // создание облака
        Vector3 position = spamPoints[index].transform.position; // присваеваем позицию СпамПоинта в зависимости от рандомного числа
        cl.transform.position = position;
        Invoke("SpamCloud", spamInterval);
    }
    private void SpamSlipper()
    {
        int index = UnityEngine.Random.Range(0, 7);
        GameObject sl = Instantiate(slipper);
        Vector3 position = spamPoints[index].transform.position;
        sl.transform.position = position;
        Invoke("SpamSlipper", spamSlipperInterval);
    }
    private void SpamFeather()
    {
        int index = UnityEngine.Random.Range(0, 7);
        GameObject sl = Instantiate(feather);
        Vector3 position = spamPoints[index].transform.position;
        sl.transform.position = position;
        Invoke("SpamFeather", spamFeatherInterval);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
