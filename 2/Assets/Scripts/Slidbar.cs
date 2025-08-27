using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
 

public class Slidbar : MonoBehaviour
{
    public Slider slider;       // —сылка на UI слайдер
    public Timer timerScript;   // —сылка на скрипт Timer с lifeTime

    private float elapsedTime = 0f;
    void Update()
    {
        if (timerScript == null || slider == null) return;

        elapsedTime += Time.deltaTime;
        slider.value = Mathf.Clamp01(elapsedTime / timerScript.lifeTime);
    }
}
