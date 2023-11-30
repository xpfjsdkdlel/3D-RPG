using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonGroup;
    [SerializeField]
    private GameObject setting;
    [SerializeField]
    private GameObject confrim;
    [SerializeField]
    private Slider sliderBGM;
    [SerializeField]
    private Slider sliderSFX;
    [SerializeField]
    private AudioClip BGM;

    private void Start()
    {
        sliderBGM.value = AudioManager.Instance.BGMVolume;
        sliderSFX.value = AudioManager.Instance.SFXVolume;
        SetBGMVolume();
        SetSFXVolume();
        AudioManager.Instance.PlayBGM(BGM);
        GameManager.Instance.fade.FadeIn();
    }
    void LoadCharacterSelectScene()
    {
        GameManager.Instance.LoadScene("CharacterSelectScene");
    }

    void LoadGameScene()
    {
        GameManager.Instance.LoadScene("GameScene");
    }

    public void GameStart()
    {// 게임시작 버튼을 눌렀을 때
        AudioManager.Instance.PlaySFX(GameManager.Instance.click);
        if (File.Exists(GameManager.Instance.dataPath))
        {// 저장된 파일이 있다면 새 게임, 이어하기 여부 출력
            confrim.SetActive(true);
            buttonGroup.SetActive(false);
        }
        else
            // 없다면 새 게임 시작
            NewGame();
    }

    public void NewGame()
    {
        AudioManager.Instance.PlaySFX(GameManager.Instance.click);
        GameManager.Instance.ResetData();
        GameManager.Instance.fade.FadeOut();
        Invoke("LoadCharacterSelectScene", 2f);
    }

    public void Continue()
    {
        AudioManager.Instance.PlaySFX(GameManager.Instance.click);
        GameManager.Instance.LoadData();
        GameManager.Instance.fade.FadeOut();
        Invoke("LoadGameScene", 2f);
    }

    public void Cancel()
    {
        AudioManager.Instance.PlaySFX(GameManager.Instance.click);
        buttonGroup.SetActive(true);
        confrim.SetActive(false);
    }

    public void Setting(bool open)
    {
        AudioManager.Instance.PlaySFX(GameManager.Instance.click);
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
        AudioManager.Instance.PlaySFX(GameManager.Instance.click);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
