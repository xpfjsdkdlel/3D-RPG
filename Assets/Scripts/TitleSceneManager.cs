using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonGroup;
    [SerializeField]
    private GameObject setting;
    [SerializeField]
    private Slider sliderBGM;
    [SerializeField]
    private Slider sliderSFX;
    [SerializeField]
    private AudioClip BGM;

    private void Start()
    {
        SetBGMVolume();
        SetSFXVolume();
        AudioManager.Instance.PlayBGM(BGM);
        GameManager.Instance.fade.FadeIn();
    }
    void LoadCharacterSelectScene()
    {
        GameManager.Instance.LoadScene("CharacterSelectScene");
    }

    public void GameStart()
    {// 게임시작 버튼을 눌렀을 때 신을 전환
        GameManager.Instance.fade.FadeOut();
        Invoke("LoadCharacterSelectScene", 2f);
    }

    public void Setting(bool open)
    {
        setting.SetActive(open);
        buttonGroup.SetActive(!open);
    }

    public void SetBGMVolume()
    {
        AudioManager.Instance.SetBGMVolume(sliderBGM.value);
    }

    public void SetSFXVolume()
    {
        AudioManager.Instance.SetSFXVolume(sliderSFX.value);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
