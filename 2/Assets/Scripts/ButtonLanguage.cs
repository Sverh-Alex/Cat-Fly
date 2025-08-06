using UnityEngine;

public class ButtonLanguage : MonoBehaviour
{
    [SerializeField] public GameObject _button;


    void Start()
    {
        _button.SetActive(false);
    }

    public void OnClicLanguage()
    {
        _button.SetActive(true);
    }
    public void OffClicLanguage()
    {
        _button.SetActive(false);
    }
  
}
