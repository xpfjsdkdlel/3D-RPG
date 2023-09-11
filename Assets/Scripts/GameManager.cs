using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

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
    public int row; // �κ��丮 ��
    public Item[] items; // ������
    public Item[] equip; // ������ ���
    public QuestData[] quest; // ����Ʈ
    public int questNum; // ����Ʈ ���൵
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
    }

    public string nextScene;

    private PlayerData pData = new PlayerData(); // ����� �÷��̾� ������
    public string dataPath; // �������� ���� ���

    public int number = 0; // Ŭ����
    public string name; // ĳ���� �̸�
    public int level = 1; // ����
    public int HP = 50; // ���� ü��
    public int maxHP = 50; // �ִ� ü��
    public int MP = 20; // ���� ����
    public int maxMP = 20; // �ִ� ����
    public int EXP = 0; // ����ġ
    public int gold = 0; // ��
    public int row = 5; // �κ��丮 ��
    public Item[] items = null; // �κ��丮
    public Item[] equip = null; // ������ ���
    public QuestData[] quest; // ����Ʈ
    public int questNum = 1; // ����Ʈ ���൵

    public AudioClip click;

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
            Destroy(gameObject);
            return;
        }
        fade = GameObject.FindObjectOfType<Fade>();
        if (fade == null)
        {
            fade = Instantiate(Resources.Load<Fade>("Prefabs/UI/Fade"));
            fade.Init();
        }
        items = new Item[row * 3];
        equip = new Item[5];
        fade.FadeIn();
        dataPath = Application.persistentDataPath + "/save";
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
        row = 5;
        items = new Item[row * 3];
        equip = new Item[5];
        for(int i = 0; i < quest.Length; i++)
        {
            quest[i].isProgress = false;
            quest[i].isClear = false;
        }
        questNum = 1;
    }

    public void SaveData()
    {
        pData.number = number;
        pData.name = name;
        pData.level = level;
        pData.HP = HP;
        pData.maxHP = maxHP;
        pData.MP = MP;
        pData.maxMP = maxMP;
        pData.EXP = EXP;
        pData.gold = gold;
        pData.row = row;
        pData.items = items;
        pData.equip = equip;
        pData.quest = quest;
        pData.questNum = questNum;

        string data = JsonUtility.ToJson(pData);
        File.WriteAllText(dataPath, data);
        Debug.Log("�����Ͱ� ����Ǿ����ϴ�.");
    }

    public void LoadData()
    {
        if (File.Exists(dataPath))
        { 
            string data = File.ReadAllText(dataPath);
            pData = JsonUtility.FromJson<PlayerData>(data);
            number = pData.number;
            name = pData.name;
            level = pData.level;
            HP = pData.HP;
            maxHP = pData.maxHP;
            MP = pData.MP;
            maxMP = pData.maxMP;
            EXP = pData.EXP;
            gold = pData.gold;
            row = pData.row;
            items = pData.items;
            equip = pData.equip;
            quest = pData.quest;
            questNum = pData.questNum;
        }
        else
            ResetData();
    }

    public void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadSceneAsync("LoadingScene");
    }
}
