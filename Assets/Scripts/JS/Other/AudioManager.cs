using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public PlayerInteraction player;

    private bool isPlayedLight = false;
    [Header(" Audio ")]
    public AudioSource General;
  
    public AudioSource enemyHurt;
    public AudioSource shoot;
    public AudioSource playerDash;
    public AudioSource playerHurt;
    public AudioSource ZhuXiTaiUp;
    public AudioSource lightOn;
    public AudioClip up;

    void Start()
    {

        instance = this;
    }

    void Update()
    {

    }

    public void ShootAudio()
    {
        shoot.Play();
    }

    public void LevelUpAudio()
    {


    }

    public void LightOnAudio()
    {
        if (!lightOn.isPlaying && !isPlayedLight)
        {
            lightOn.Play(); // 如果音效正在播放，则暂停
            isPlayedLight = true;
        }
    }

    public void DashAudio()
    {
        if (!playerDash.isPlaying)
        {
            playerDash.Play();
           
        }
    }

    public void PlayOneTime(AudioClip audioClip)
    {
        General.PlayOneShot(audioClip);
    }
}
