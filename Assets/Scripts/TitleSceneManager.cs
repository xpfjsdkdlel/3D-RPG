using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    private Fade fade;
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
        fade = GameObject.FindObjectOfType<Fade>();
        if (fade == null)
        {
            fade = Instantiate(Resources.Load<Fade>("Prefabs/UI/Fade"));
            fade.Init();
        }
        fade.FadeIn();
        SetVolume();
        AudioManager.Instance.PlayBGM(BGM);
    }
    void LoadCharacterSelectScene()
    {
        GameManager.Instance.LoadScene("CharacterSelectScene");
    }

    public void GameStart()
    {// ���ӽ��� ��ư�� ������ �� ���� ��ȯ
        fade.FadeOut();
        Invoke("LoadCharacterSelectScene", 2f);
    }

    public void Setting(bool open)
    {
        setting.SetActive(open);
        buttonGroup.SetActive(!open);
    }

    public void SetVolume()
    {
        AudioManager.Instance.SetVolume(sliderBGM.value, sliderSFX.value);
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
