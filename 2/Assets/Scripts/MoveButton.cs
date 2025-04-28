using UnityEngine;
using UnityEngine.EventSystems;


public class MoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
    [SerializeField] private GameObject cat;
    private CatParentMove catScript;
    [SerializeField] private int moveDirection;

    public void OnPointerDown(PointerEventData eventData)
    {
        catScript.moveDirection = moveDirection;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        catScript.moveDirection = 0;
    }

    void Start()
    {
        catScript = cat.GetComponent<CatParentMove>(); // "cat" указываем на каком конкретно объекте ищем скрипт
    }

    void Update()
    {
        // управление без остановки
        // if (Input.GetKey(KeyCode.UpArrow)) { catScript.moveDirection = 1; } 
        // if (Input.GetKey(KeyCode.DownArrow)){ catScript.moveDirection = -1; }
    }


}
