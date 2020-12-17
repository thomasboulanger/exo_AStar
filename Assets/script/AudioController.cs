using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip UIClic;
    public AudioClip deathSound;

    public void DeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }
    public void ButtonClic()
    {
        audioSource.PlayOneShot(UIClic);
    }
}
