using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
 

public class Slidbar : MonoBehaviour
{
    public Slider slider;       // —сылка на UI слайдер
    public float timerScript;   // —сылка на скрипт Timer с lifeTime
    public int xx;


    private float elapsedTime = 0f;
    void Update()
    {
        Debug.Log(elapsedTime);
    }
}
