using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData
{
    public int number; // Ŭ����
    public string name; // ĳ���� �̸�
    public int level; // ����
    public int HP; // ���� ü��
    public int maxHP; // �ִ� ü��
    public int MP; // ���� ����
    public int maxMP; // �ִ� ����
    public int EXP; // ����ġ
    public int gold; // ��
    public Inventory inventory = new Inventory(); // ������
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
    }

    public string nextScene;

    public int number = 0; // Ŭ����
    public string name; // ĳ���� �̸�
    public int level = 1; // ����
    public int HP = 50; // ���� ü��
    public int maxHP = 50; // �ִ� ü��
    public int MP = 20; // ���� ����
    public int maxMP = 20; // �ִ� ����
    public int EXP = 0; // ����ġ
    public int gold = 0; // ��

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
