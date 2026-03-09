using UnityEngine;

public class FireButton : MonoBehaviour
{
    // Сюда в инспекторе перетаскиваешь компонент Cat (или объект, Unity сама подставит компонент)
    [SerializeField] private Cat catScript;

    private void Start()
    {
        if (catScript == null)
        {
            Debug.LogError("[FireButton] Поле 'catScript' не назначено в инспекторе!");
        }
    }

    public void OnPointerDown()
    {
        if (catScript != null)
        {
            catScript.fire();
        }
        else
        {
            Debug.LogWarning("[FireButton] Нельзя вызвать fire(), catScript == null");
        }
    }
}
