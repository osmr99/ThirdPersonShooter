using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    public AudioSource sfxSource;

    //public AudioSource masterAudioSource;
    //public AudioSource musicAudioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        sfxSource = GetComponentInChildren<AudioSource>();
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos, float vol)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, vol);
        }
    }

    //public void PlaySound3D(string soundName, Vector3 pos)
    //{
        //PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    //}

    public void PlaySound2D(string soundName)
    {
        sfxSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }
}
