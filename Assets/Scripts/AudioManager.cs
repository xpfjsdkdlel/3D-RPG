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

    private AudioSource audioSource;
    [SerializeField]
    private GameObject sfx;
    [SerializeField]
    private int count = 10;
    private int cursor = 0;
    public float BGMVolume = 1;
    public float SFXVolume = 1;

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
            return;
        }
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < count; i++)
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

    public void SetBGMVolume(float BGMVolume)
    {
        // 배경음 볼륨 조절
        this.BGMVolume = BGMVolume;
        audioSource.volume = this.BGMVolume;
    }

    public void SetSFXVolume(float SFXVolume)
    {
        // 효과음 볼륨 조절
        this.SFXVolume = SFXVolume;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<AudioSource>().volume = this.SFXVolume;
    }
}
