using UnityEngine;
using UnityEngine.UIElements;

public class BackGroundScript : MonoBehaviour
{
    [SerializeField] GameObject Fon1;
    [SerializeField] GameObject Fon2;
    [SerializeField] GameObject Fon3;
    [SerializeField] private float speed1 = 9.0f;  // ѕоказываем на панели скорость
    [SerializeField] private float speed2 = 6.0f;  // ѕоказываем на панели скорость
    [SerializeField] private float speed3 = 3.0f;  // ѕоказываем на панели скорость

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if(Fon1 != null)
        {
            Fon1.transform.Translate(Vector3.right * -speed1 * Time.deltaTime); // Translate - это переместить
        }
        if (Fon2 != null)
        {
            Fon2.transform.Translate(Vector3.right * -speed2 * Time.deltaTime); // Translate - это переместить
        }
        if (Fon3 != null)
        {
            Fon3.transform.Translate(Vector3.right * -speed3 * Time.deltaTime); // Translate - это переместить
        }
    }
}
