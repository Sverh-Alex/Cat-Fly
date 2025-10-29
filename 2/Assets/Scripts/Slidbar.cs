using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
 

public class Slidbar : MonoBehaviour
{
    public Slider slider;          // —сылка на UI Slider
    public Timer timerScript;   // —сылка на скрипт Timer с lifeTime

    private float elapsedTime = 0f;

    void Start()
    {
        elapsedTime = 0f;
        slider.maxValue = timerScript.lifeTime;
    }

    void Update()
    {
        float progress = elapsedTime += Time.deltaTime;
        slider.value = progress;
    }

}
    

