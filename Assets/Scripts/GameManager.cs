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
    public int[] items; // 아이템
    public int[] itemsCount; // 아이템 개수
    public int[] equip; // 장착한 장비
    public int[] questProgress; // 퀘스트 진행도
    public bool[] questIsProgress; // 퀘스트 진행여부
    public bool[] questIsClear; // 퀘스트 클리어 여부
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
    public int HP = 100; // 현재 체력
    public int maxHP = 100; // 최대 체력
    public int MP = 50; // 현재 마나
    public int maxMP = 50; // 최대 마나
    public int EXP = 0; // 경험치
    public int gold = 0; // 돈
    public Item[] items = null; // 인벤토리
    public Item[] equip = null; // 장착한 장비
    public QuestData[] quest; // 퀘스트
    public int questNum = 1; // 퀘스트 진행도

    public AudioClip click;

    public Fade fade;
    public GameDB gameDB;

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
        items = new Item[15];
        equip = new Item[5];
        fade.FadeIn();
        dataPath = Application.persistentDataPath + "/save";
    }

    public void ResetData()
    {
        number = 0;
        name = "player";
        level = 1;
        HP = 100;
        maxHP = 100;
        MP = 50;
        maxMP = 50;
        EXP = 0;
        gold = 0;
        items = new Item[15];
        equip = new Item[5];
        for(int i = 0; i < quest.Length; i++)
        {
            quest[i] = new QuestData();
            QuestData tempData = Resources.Load<QuestData>("Quest/Quest" + (i + 1));
            quest[i].questId = tempData.questId;
            quest[i].targetId = tempData.targetId;
            quest[i].questName = tempData.questName;
            quest[i].questInfo = tempData.questInfo;
            quest[i].startLevel = tempData.startLevel;
            quest[i].text = tempData.text;
            quest[i].progress = tempData.progress;
            quest[i].complete = tempData.complete;
            quest[i].result = tempData.result;
            quest[i].progText = tempData.progText;
            quest[i].clearText = tempData.clearText;
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
        pData.items = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        pData.itemsCount = new int[15] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        pData.equip = new int[5] { 0, 0, 0, 0, 0 };
        for (int i = 0; i < pData.items.Length; i++)
        {
            if (items[i] != null)
            {
                pData.items[i] = items[i].uid;
                pData.itemsCount[i] = items[i].count;
            }
            else
            {
                pData.items[i] = 0;
                pData.itemsCount[i] = 0;
            }
        }
        for (int i = 0; i < pData.equip.Length; i++)
        {
            if (equip[i] != null)
                pData.equip[i] = equip[i].uid;
            else
                pData.equip[i] = 0;
        }
        pData.questProgress = new int[3] { 0, 0, 0 };
        pData.questIsProgress = new bool[3] { false, false, false };
        pData.questIsClear = new bool[3] { false, false, false };
        for (int i = 0; i < quest.Length; i++)
        {
            pData.questProgress[i] = quest[i].progress;
            pData.questIsProgress[i] = quest[i].isProgress;
            pData.questIsClear[i] = quest[i].isClear;
        }
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
            for (int i = 0; i < pData.items.Length; i++)
            {
                if(pData.items[i] != 0)
                {
                    items[i] = new Item();
                    for(int j = 0; j < gameDB.ItemData.Count; j++)
                    {
                        if(gameDB.ItemData[j].uid == pData.items[i])
                        {
                            items[i].uid = gameDB.ItemData[j].uid;
                            items[i].type = gameDB.ItemData[j].type;
                            items[i].name = gameDB.ItemData[j].name;
                            items[i].iconImg = Resources.Load<Sprite>("Sprite/" + gameDB.ItemData[j].iconImg);
                            items[i].classNum = gameDB.ItemData[j].classNum;
                            items[i].price = gameDB.ItemData[j].price;
                            items[i].count = pData.itemsCount[i];
                            items[i].maxCount = gameDB.ItemData[j].maxCount;
                            items[i].damage = gameDB.ItemData[j].damage;
                            items[i].armor = gameDB.ItemData[j].armor;
                            items[i].speed = gameDB.ItemData[j].speed;
                            items[i].range = gameDB.ItemData[j].range;
                            items[i].heal = gameDB.ItemData[j].heal;
                            break;
                        }
                    }
                }
                else
                    items[i] = null;
            }
            for (int i = 0; i < pData.equip.Length; i++)
            {
                if (pData.equip[i] != 0)
                {
                    equip[i] = new Item();
                    for (int j = 0; j < gameDB.ItemData.Count; j++)
                    {
                        if (gameDB.ItemData[j].uid == pData.equip[i])
                        {
                            equip[i].uid = pData.equip[i];
                            equip[i].type = gameDB.ItemData[j].type;
                            equip[i].name = gameDB.ItemData[j].name;
                            equip[i].iconImg = Resources.Load<Sprite>("Sprite/" + gameDB.ItemData[j].iconImg);
                            equip[i].classNum = gameDB.ItemData[j].classNum;
                            equip[i].price = gameDB.ItemData[j].price;
                            equip[i].count = 1;
                            equip[i].maxCount = gameDB.ItemData[j].maxCount;
                            equip[i].damage = gameDB.ItemData[j].damage;
                            equip[i].armor = gameDB.ItemData[j].armor;
                            equip[i].speed = gameDB.ItemData[j].speed;
                            equip[i].range = gameDB.ItemData[j].range;
                            equip[i].heal = gameDB.ItemData[j].heal;
                            break;
                        }
                    }
                }
                else
                    equip[i] = null;
            }
            for (int i = 0; i < quest.Length; i++)
            {
                quest[i] = new QuestData();
                QuestData tempData = Resources.Load<QuestData>("Quest/Quest" + (i + 1));
                quest[i].questId = tempData.questId;
                quest[i].targetId = tempData.targetId;
                quest[i].questName = tempData.questName;
                quest[i].questInfo = tempData.questInfo;
                quest[i].startLevel = tempData.startLevel;
                quest[i].text = tempData.text;
                quest[i].progress = pData.questProgress[i];
                quest[i].complete = tempData.complete;
                quest[i].result = tempData.result;
                quest[i].progText = tempData.progText;
                quest[i].clearText = tempData.clearText;
                quest[i].isProgress = pData.questIsProgress[i];
                quest[i].isClear = pData.questIsClear[i];
                questNum = pData.questNum;
            }
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
