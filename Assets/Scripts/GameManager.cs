using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData
{
    public int number; // 클래스
    public string name; // 캐릭터 이름
    public int level; // 레벨
    public int HP; // 현재 체력
    public int maxHP; // 최대 체력
    public int MP; // 현재 마나
    public int maxMP; // 최대 마나
    public int EXP; // 경험치
    public int gold; // 돈
    public Inventory inventory = new Inventory(); // 아이템
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
    }

    public string nextScene;

    public int number = 0; // 클래스
    public string name; // 캐릭터 이름
    public int level = 1; // 레벨
    public int HP = 50; // 현재 체력
    public int maxHP = 50; // 최대 체력
    public int MP = 20; // 현재 마나
    public int maxMP = 20; // 최대 마나
    public int EXP = 0; // 경험치
    public int gold = 0; // 돈

    public Fade fade;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        fade = GameObject.FindObjectOfType<Fade>();
        if (fade == null)
        {
            fade = Instantiate(Resources.Load<Fade>("Prefabs/UI/Fade"));
            fade.Init();
        }
        fade.FadeIn();
    }

    public void ResetData()
    {
        number = 0;
        name = "player";
        level = 1;
        HP = 50;
        maxHP = 50;
        MP = 20;
        maxMP = 20;
        EXP = 0;
        gold = 0;
    }

    public void SaveData()
    {

    }

    public void LoadData()
    {

    }

    public void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadSceneAsync("LoadingScene");
    }
}
