using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DelayedShowSelf : MonoBehaviour
{
    [SerializeField] private float delayBeforeShow = 2f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0f;        // невидим
        StartCoroutine(ShowAfterDelay());
    }

    private IEnumerator ShowAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeShow);
        canvasGroup.alpha = 1f;        // видим
    }
}