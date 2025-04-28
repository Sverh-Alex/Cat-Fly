using UnityEngine;
using UnityEngine.EventSystems;


public class FireButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject cat;
    private Cat catScript;

    public void OnPointerDown(PointerEventData eventData) // что происходит при нажатии на кнопку
    {
        catScript.fire();
    }

  

    void Start()
    {
        catScript = cat.GetComponent<Cat>(); // "cat" указываем на каком конкретно объекте ищем скрипт
    }

  
}
