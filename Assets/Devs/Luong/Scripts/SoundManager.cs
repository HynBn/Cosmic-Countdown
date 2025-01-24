using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private static SoundManager instance;
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


    void Awake()
    {
        // If there's no existing instance, set this one and make it persist across scenes
        if (instance == null) 
        {
            instance = this; 
            DontDestroyOnLoad(gameObject); // Keep the SoundManager across scene transitions
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances 
        }
    }

    void Start() {
        MusicSource.clip = BigMap_background;
        MusicSource.loop = true;
        MusicSource.Play();
    }

   public void PlayMusic(AudioClip clip){
        if (MusicSource.isPlaying) 
        {
            MusicSource.Stop(); // Stop the current music if it's playing
        }
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void PlaySFX(AudioClip clip){
        SFXSource.PlayOneShot(clip);
    }
    

}
