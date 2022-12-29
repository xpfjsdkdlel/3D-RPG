using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get => instance;
    }

    AudioSource audioSource;
    [SerializeField]
    private GameObject sfx;
    [SerializeField]
    private int count = 10;
    private int cursor = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        for(int i = 0; i < count; i++)
        {
            Instantiate(sfx).transform.parent = transform;
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        transform.GetChild(cursor).GetComponent<AudioSource>().clip = clip;
        transform.GetChild(cursor).GetComponent<AudioSource>().Play();
        cursor++;
        if (cursor >= transform.childCount)
            cursor = 0;
    }

    public void SetVolume(float BGMVolume, float SFXVolume)
    {
        audioSource.volume = BGMVolume;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<AudioSource>().volume = SFXVolume;
    }
}
