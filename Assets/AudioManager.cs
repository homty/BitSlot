using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;
    public AudioClip matchDisappearClip;
    public AudioClip spinClickClip;
    public AudioClip rewardClip;
    public AudioClip balanceClip;
    private float lastRewardSoundTime = -10f;
    private float rewardSoundCooldown = 0.2f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
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

}

