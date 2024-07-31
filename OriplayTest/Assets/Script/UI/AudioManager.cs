using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----- Audio Source -----")]
    [SerializeField] AudioSource musicSource;
    public AudioClip background;
    
    /*[SerializeField] AudioSource SFXSource;

    [Header("----- Audio Clip -----")]
   
    public AudioClip AddCoins;
    public AudioClip AddLife;
    public AudioClip LoseLife;
    public AudioClip WinGame;
    public AudioClip LoseGame;
    public AudioClip Gun1Sound;
    public AudioClip Gun2Sound;
    public AudioClip Gun3Sound;*/
    

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }
    /*public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }*/
}
