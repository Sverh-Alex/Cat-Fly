using UnityEngine;
using UnityEngine.EventSystems;


public class FireButton : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    private Cat catScript;

    public void OnPointerDown() // ��� ���������� ��� ������� �� ������
    {
        catScript.fire();
    }

  

    void Start()
    {
        catScript = cat.GetComponent<Cat>(); // "cat" ��������� �� ����� ��������� ������� ���� ������
    }

  
}
