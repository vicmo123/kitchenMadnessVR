using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingBell : MonoBehaviour
{

    public AudioClip bellSoundEffect;
    private AudioSource bellAudioSource;


    public AudioClip winSoundEffect;
    private AudioSource winAudioSource;


    public AudioClip loseSoundEffect;
    private AudioSource loseAudioSource;

    bool isServable;

    // Start is called before the first frame update
    void Start()
    {
        bellAudioSource = GetComponent<AudioSource>();

        winAudioSource = GetComponent<AudioSource>();

        loseAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("VRHANDS"))
        {
            bellAudioSource.PlayOneShot(bellSoundEffect);

            if(isServable)
            {
                winAudioSource.PlayOneShot(winSoundEffect); //winSound;
            }
            else
            {
                loseAudioSource.PlayOneShot(loseSoundEffect); //loseSound;
            }
        }
    }
}
