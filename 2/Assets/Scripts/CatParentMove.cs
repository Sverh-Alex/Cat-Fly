using UnityEngine;
using UnityEngine.EventSystems;

public class CatParentMove : MonoBehaviour
{
    //[SerializeField] private float speed = 5.0f;  // ���������� �� ������ ��������
    private Vector3 moveVector; // 
    public int moveDirection = 0;

    void Start()
    {
   //     moveVector = new Vector3(0, speed); // ������� ��������� ��� ��������
    }

    // Update is called once per frame
    void Update()
    {
        //if (transform.position.y < 3.6 && moveDirection == 1) // ������ ����������� �� ����������� 
       // {
       //     gameObject.transform.Translate(moveVector * Time.deltaTime); // Translate - ��� �����������
       // }
      //  else if (transform.position.y > -3.6 && moveDirection == -1)
       // {
      //      gameObject.transform.Translate(-moveVector * Time.deltaTime); // Translate - ��� �����������
       // }
    }
}
