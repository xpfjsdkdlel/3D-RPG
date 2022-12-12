using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    private Fade fade;
    private void Start()
    {
        fade = GameObject.FindObjectOfType<Fade>();
        if (fade == null)
        {
            fade = Instantiate(Resources.Load<Fade>("Prefabs/UI/Fade"));
            fade.Init();
        }
        fade.FadeIn();
    }
    void NextScene()
    {
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void LoadMainScene()
    {// 게임시작 버튼을 눌렀을 때 신을 전환
        fade.FadeOut();
        Invoke("NextScene", 2f);
    }
}
