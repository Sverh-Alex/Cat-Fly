using UnityEngine;
using UnityEngine.EventSystems;


public class FireButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject cat;
    private Cat catScript;

    public void OnPointerDown(PointerEventData eventData) // ��� ���������� ��� ������� �� ������
    {
        catScript.fire();
    }

  

    void Start()
    {
        catScript = cat.GetComponent<Cat>(); // "cat" ��������� �� ����� ��������� ������� ���� ������
    }

  
}
