using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip UIClic;
    public AudioClip deathSound;
    public AudioClip chestSound;

    public void DeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }
    public void ButtonClic()
    {
        audioSource.PlayOneShot(UIClic);
    }public void TouchChest()
    {
        audioSource.PlayOneShot(chestSound);
    }
}
