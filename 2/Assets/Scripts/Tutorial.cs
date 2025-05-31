using System.Collections;
using System;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject tutorWeb;
    [SerializeField] GameObject tutorApp;
    [SerializeField] float closeTutorial = 10f;
    private bool isTutorialShown = false;
    void Start()
    {
        tutorWeb.SetActive(false);
        tutorApp.SetActive(false);
        ScoreManager.OnTutorWeb += isTutorWeb;
        ScoreManager.OnTutorApp += isTutorApp;
    }
    public void isTutorWeb()
    {
        if (!isTutorialShown)
        {
            isTutorialShown = true;
            tutorWeb.SetActive(true); 
            StartCoroutine(HidenTutorial());
        }
        
        
    }
    private void isTutorApp()
    {
        if(!isTutorialShown)
        {
            isTutorialShown = true;
            tutorApp.SetActive(true);
            StartCoroutine(HidenTutorial());
        }
    }
    private IEnumerator HidenTutorial()
    {

        yield return new WaitForSeconds(closeTutorial);
        if(tutorApp)
        {
            
            tutorApp.SetActive(false);
        }
       if(tutorWeb)
        {
            tutorWeb.SetActive(false);
        }
        
        Debug.Log("Закрыл Туториал");
        
    }

}
