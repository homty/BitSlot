using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusicClip;
    private AudioSource musicSource;
    public static AudioManager Instance;
    private AudioSource audioSource;
    public AudioClip matchDisappearClip;
    public AudioClip spinClickClip;
    public AudioClip rewardClip;
    public AudioClip balanceClip;
    public AudioClip endingRewardClip;
    public AudioClip MultiplierAppearClip;
    public AudioClip multiplyRewardClip;
    public AudioClip bonusSpinClip;
    public AudioClip PlusMinusClip;
    public AudioClip IncreasingBetClip;
    public AudioClip downloadClip;
    private float lastRewardSoundTime = -10f;
    private float rewardSoundCooldown = 0.2f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            // Add a separate AudioSource for background music
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = 0.5f; // Adjust volume as needed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusicClip != null && !musicSource.isPlaying)
        {
            musicSource.clip = backgroundMusicClip;
            musicSource.Play();
        }
    }

    public void PlayDownloadSound()
    {
        if (downloadClip != null)
        {
            audioSource.PlayOneShot(downloadClip);
        }
    }
    public void PlaySpinClick()
    {
        if (spinClickClip != null)
        {
            audioSource.PlayOneShot(spinClickClip);
        }
    }

    public void PlayDisappearSound()
    {
        if (matchDisappearClip != null)
            audioSource.PlayOneShot(matchDisappearClip);
    }

    public void PlayRewardSound()
    {
        if (rewardClip != null && Time.time - lastRewardSoundTime > rewardSoundCooldown)
        {
            audioSource.PlayOneShot(rewardClip);
            lastRewardSoundTime = Time.time;
        }
    }

    public void PlayBalanceSound()
    {
        if (balanceClip != null)
            audioSource.PlayOneShot(balanceClip);
    }

    public void PlayEndingRewardSound()
    {
        if (endingRewardClip != null)
            audioSource.PlayOneShot(endingRewardClip);
    }

    public void PlayMultiplierAppearSound()
    {
        if (MultiplierAppearClip != null)
            audioSource.PlayOneShot(MultiplierAppearClip);
    }

    public void PlayMultiplyRewardSound()
    {
        if (multiplyRewardClip != null)
            audioSource.PlayOneShot(multiplyRewardClip);
    }

    public void PlayBonusSpinSound()
    {
        if (bonusSpinClip != null)
            audioSource.PlayOneShot(bonusSpinClip);
    }

    public void PlayPlusMinusSound()
    {
        if (PlusMinusClip != null)
            audioSource.PlayOneShot(PlusMinusClip);
    }
    
    public void PlayIncreasingBetSound()
    {
        if (IncreasingBetClip != null)
            audioSource.PlayOneShot(IncreasingBetClip);
    }

}

