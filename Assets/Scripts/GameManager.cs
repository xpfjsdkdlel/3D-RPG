using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

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
    public int row; // 인벤토리 행
    public Item[] items; // 아이템
    public Item[] equip; // 장착한 장비
    public QuestData[] quest; // 퀘스트
    public int questNum; // 퀘스트 진행도
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
    }

    public string nextScene;

    private PlayerData pData = new PlayerData(); // 저장된 플레이어 데이터
    public string dataPath; // 데이터의 저장 경로

    public int number = 0; // 클래스
    public string name; // 캐릭터 이름
    public int level = 1; // 레벨
    public int HP = 50; // 현재 체력
    public int maxHP = 50; // 최대 체력
    public int MP = 20; // 현재 마나
    public int maxMP = 20; // 최대 마나
    public int EXP = 0; // 경험치
    public int gold = 0; // 돈
    public int row = 5; // 인벤토리 행
    public Item[] items = null; // 인벤토리
    public Item[] equip = null; // 장착한 장비
    public QuestData[] quest; // 퀘스트
    public int questNum = 1; // 퀘스트 진행도

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
        Debug.Log("데이터가 저장되었습니다.");
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
