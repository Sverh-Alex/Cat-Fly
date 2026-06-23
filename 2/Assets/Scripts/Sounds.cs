using UnityEngine;

public class Sounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] AudioSource click;
    public void PlaySound()
    {
        click.Play();
    }
}
