using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip _bounce;
    [SerializeField] private AudioClip _brick;
    [SerializeField] private AudioClip _powerUp;
    [SerializeField] private AudioClip _launch;
    [SerializeField] private AudioClip _lifeLost;
    [SerializeField] private AudioClip _gameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayBounce()   { PlayOneShot(_bounce); }
    public void PlayBrick()    { PlayOneShot(_brick); }
    public void PlayPowerUp()  { PlayOneShot(_powerUp); }
    public void PlayLaunch()   { PlayOneShot(_launch); }
    public void PlayLifeLost() { PlayOneShot(_lifeLost); }
    public void PlayGameOver() { PlayOneShot(_gameOver); }

    private void PlayOneShot(AudioClip clip)
    {
        if (clip != null)
        {
            _sfxSource.PlayOneShot(clip);
        }
    }
}