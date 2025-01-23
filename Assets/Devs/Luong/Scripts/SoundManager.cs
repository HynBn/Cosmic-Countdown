using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("---------- Audio Source ------------")]
   [SerializeField] AudioSource MusicSource;
   [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ------------")]

    public AudioClip BigMap_background;
    public AudioClip MediumMap_background;

    public AudioClip SmallMap_background;

    public AudioClip tick;
    public AudioClip explode;
    public AudioClip pickUp;
    public AudioClip bumpInto;
    public AudioClip swap;
    public AudioClip rocket;
    public AudioClip reverse;
    public AudioClip trap;

    void Start() {
        MusicSource.clip = BigMap_background;
        MusicSource.Play();
    }

   public void PlayMusic(AudioClip clip){
        MusicSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip){
        SFXSource.PlayOneShot(clip);
    }
    

}
