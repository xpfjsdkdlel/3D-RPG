using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private GameSceneManager gameSceneManager;
    public GameObject inventoryObj; // 인벤토리 오브젝트
    public Inventory inventory; // 인벤토리
    public GameObject equipment; // 장비창
    public GameObject menu; // 메뉴
    [SerializeField]
    private GameObject dialogue; // 대화문
    [SerializeField]
    private TextMeshProUGUI npcName; // 대화문 내 NPC이름
    [SerializeField]
    private TextMeshProUGUI npcText; // 대화문 텍스트
    public QuestData quest = null; // 진행중인 퀘스트
    public int questIndex; // 퀘스트 인덱스
    private int n = 0; // 대화문 인덱스
    private NPCrole npcRole;
    [SerializeField]
    private GameObject storeObj; // 상점
    [SerializeField]
    private GameObject questUI; // 퀘스트 UI
    [SerializeField]
    private TextMeshProUGUI questName;
    [SerializeField]
    private TextMeshProUGUI questInfo;
    [SerializeField]
    private TextMeshProUGUI questProg;
    [SerializeField]
    private Slider sliderBGM; // 배경음악 슬라이더
    [SerializeField]
    private Slider sliderSFX; // 효과음 슬라이더
    private bool invenActive = false;
    private bool equipActive = false;
    private bool menuActive = false;

    private Skill[] skills;

    [SerializeField] private GameObject playerName; // 플레이어 이름
    private Camera cam = null;

    [SerializeField] private Image skill1; // 스킬 이미지 UI
    [SerializeField] private Image skill2;
    [SerializeField] private Image skill3;
    [SerializeField] private Image skill1Img; // 스킬 쿨타임 이미지 UI
    [SerializeField] private Image skill2Img;
    [SerializeField] private Image skill3Img;
    [SerializeField] private TextMeshProUGUI skill1Text;
    [SerializeField] private TextMeshProUGUI skill2Text;
    [SerializeField] private TextMeshProUGUI skill3Text;
    private float skill1C = 0; // 스킬 사용후 경과한 시간
    private float skill2C = 0;
    private float skill3C = 0;

    [SerializeField] private GameObject levelUpUI; // 레벨업 시 출력할 UI
    [SerializeField] private GameObject getItemUI; // 아이템 획득시 출력할 UI
    [SerializeField]
    private AudioClip click;

    public void Init()
    {
        if (GameManager.Instance.quest[GameManager.Instance.questNum - 1].isClear == false && GameManager.Instance.quest[GameManager.Instance.questNum - 1].isProgress == true)
        {
            quest = GameManager.Instance.quest[GameManager.Instance.questNum - 1];
            questUI.SetActive(true);
            RefreshQuestUI(quest);
        }
        sliderBGM.value = AudioManager.Instance.BGMVolume;
        sliderSFX.value = AudioManager.Instance.SFXVolume;
        questIndex = GameManager.Instance.questNum;
        inventory.Init();
        inventoryObj.SetActive(invenActive);
        equipment.SetActive(equipActive);
        menu.SetActive(menuActive);
        skills = gameSceneManager.player.skills;
        skill1.sprite = skills[0].iconImg;
        skill2.sprite = skills[1].iconImg;
        skill3.sprite = skills[2].iconImg;
        skill1Img.sprite = skills[0].iconImg;
        skill2Img.sprite = skills[1].iconImg;
        skill3Img.sprite = skills[2].iconImg;
        playerName.transform.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.name;
        cam = Camera.main;
        skill1Text.text = skills[0].cost.ToString();
        skill2Text.text = skills[1].cost.ToString();
        skill3Text.text = skills[2].cost.ToString();
    }

    public void SetBGMVolume()
    {// 배경음악 조정
        AudioManager.Instance.SetBGMVolume(sliderBGM.value);
    }

    public void SetSFXVolume()
    {// 효과음 조정
        AudioManager.Instance.SetSFXVolume(sliderSFX.value);
    }

    public void CloseInventory()
    {// 인벤토리 닫기
        invenActive = false;
        inventoryObj.SetActive(false);
    }

    public void CloseEquipment()
    {// 장비창 닫기
        equipActive = false;
        equipment.SetActive(false);
    }

    public void CloseStore()
    {// 상점 닫기
        storeObj.SetActive(false);
        gameSceneManager.ControllEnable();
    }

    public void ActiveDialogue(NPC npcData)
    {// 대화문 활성화
        AudioManager.Instance.PlaySFX(click);
        n = 0;
        dialogue.SetActive(true);
        npcName.text = npcData.name;
        npcRole = npcData.role;
        switch (npcRole)
        {
            case NPCrole.quest:
                if (quest == null)
                {
                    for (int i = 0; i < GameManager.Instance.quest.Length; i++)
                    {
                        if(!GameManager.Instance.quest[i].isClear
                            && gameSceneManager.player.level >= GameManager.Instance.quest[i].startLevel)
                        {
                            if (questIndex + 1000 == GameManager.Instance.quest[i].questId
                                && gameSceneManager.player.level >= GameManager.Instance.quest[i].startLevel
                                && !GameManager.Instance.quest[i].isClear)
                            {
                                quest = GameManager.Instance.quest[questIndex - 1];
                                npcText.text = quest.text[n];
                                n++;
                                QuestDialogue(quest);
                                break;
                            }
                        }
                        else
                            npcText.text = npcData.text;
                    }
                }
                else
                    QuestDialogue(quest);
                break;
            default:
                npcText.text = npcData.text;
                break;
        }
        gameSceneManager.ControllDisable();
    }

    public void NextDialogue()
    {// 대화문 넘기기
        AudioManager.Instance.PlaySFX(click);
        switch (npcRole)
        {
            case NPCrole.normal:
                CloseDialogue();
                break;
            case NPCrole.quest:
                if (quest != null)
                    QuestDialogue(quest);
                else
                    CloseDialogue();
                break;
            case NPCrole.store:
                storeObj.SetActive(true);
                CloseDialogue();
                gameSceneManager.ControllDisable();
                break;
        }
    }

    public void QuestDialogue(QuestData quest)
    {
        AudioManager.Instance.PlaySFX(click);
        if (quest.isProgress)
        {// 퀘스트가 진행중일 경우
            if (quest.progress == quest.complete)
            {// 클리어 시
                if (quest.clearText.Length > n)
                {
                    npcText.text = quest.clearText[n];
                    n++;
                }
                else
                {
                    quest.isProgress = false;
                    quest.isClear = true;
                    questIndex++;
                    GameManager.Instance.questNum++;
                    this.quest = null;
                    questUI.SetActive(false);
                    GameManager.Instance.gold += quest.result;
                    CloseDialogue();
                    if (GameManager.Instance.questNum > GameManager.Instance.quest.Length)
                        gameSceneManager.SpawnBoss();
                }
            }
            else if (quest.progress < quest.complete)
            {// 진행 중
                if (quest.progText.Length > n)
                {
                    npcText.text = quest.progText[n];
                    n++;
                }
                else
                    CloseDialogue();
            }
        }
        else
        {// 퀘스트를 새로 시작하는 경우
            if (quest.text.Length > n)
            {
                npcText.text = quest.text[n];
                n++;
            }
            else
            {
                quest.isProgress = true;
                questUI.SetActive(true);
                quest.progress = 0;
                RefreshQuestUI(quest);
                CloseDialogue();
            }
        }
    }

    public void CloseDialogue()
    {// 대화문 닫기
        AudioManager.Instance.PlaySFX(click);
        dialogue.SetActive(false);
        if (!storeObj.activeSelf)
            gameSceneManager.ControllEnable();
    }

    private void CloseTab()
    {// 모든 창 닫기
        AudioManager.Instance.PlaySFX(click);
        invenActive = false;
        inventoryObj.SetActive(false);
        equipActive = false;
        equipment.SetActive(false);
        dialogue.SetActive(false);
        storeObj.SetActive(false);
        gameSceneManager.ControllEnable();
    }

    public void RefreshQuestUI(QuestData quest)
    {
        questName.text = quest.questName;
        questInfo.text = quest.questInfo;
        questProg.text = quest.progress + " / " + quest.complete;
    }

    private void SkillCoolDownCheck()
    {
        if (!skills[0].active && skill1C < skills[0].coolDown)
        {
            skill1C += Time.deltaTime;
            skill1Img.fillAmount = (skills[0].coolDown - skill1C) / skills[0].coolDown;
        }
        else
        {
            skill1Img.fillAmount = 0;
            skill1C = 0;
        }
        if (!skills[1].active && skill2C < skills[1].coolDown)
        {
            skill2C += Time.deltaTime;
            skill2Img.fillAmount = (skills[1].coolDown - skill2C) / skills[1].coolDown;
        }
        else
        {
            skill2Img.fillAmount = 0;
            skill2C = 0;
        }
        if (!skills[2].active && skill3C < skills[2].coolDown)
        {
            skill3C += Time.deltaTime;
            skill3Img.fillAmount = (skills[2].coolDown - skill3C) / skills[2].coolDown;
        }
        else
        {
            skill3Img.fillAmount = 0;
            skill3C = 0;
        }
    }

    public void LevelUpUI(int level)
    {
        levelUpUI.SetActive(true);
        levelUpUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = level.ToString();
        levelUpUI.GetComponent<Animation>().Play();
        Invoke("DisableLevel", 6f);
    }

    void DisableLevel()
    {
        levelUpUI.SetActive(false);
    }

    public void GetItemUI(Item item)
    {
        getItemUI.SetActive(true);
        getItemUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.name.ToString() + " 획득";
        getItemUI.transform.GetChild(1).GetComponent<Image>().sprite = item.iconImg;
        getItemUI.GetComponent<Animation>().Play();
        Invoke("DisableGetItem", 5f);
    }

    void DisableGetItem()
    {
        getItemUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !dialogue.activeSelf && !storeObj.activeSelf)
        {
            invenActive = !invenActive;
            inventoryObj.SetActive(invenActive);
            if(invenActive)
                inventory.RefreshSlot();
        }
        if (Input.GetKeyDown(KeyCode.U) && !dialogue.activeSelf && !storeObj.activeSelf)
        {
            equipActive = !equipActive;
            equipment.SetActive(equipActive);
            if(equipActive)
                equipment.GetComponent<Equipment>().RefreshSlot();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(invenActive || equipActive || dialogue.activeSelf || storeObj.activeSelf)
                CloseTab();
            else
            {
                menuActive = !menuActive;
                menu.SetActive(menuActive);
                if (menuActive)
                    gameSceneManager.ControllDisable();
                else
                    gameSceneManager.ControllEnable();
            }
        }
        SkillCoolDownCheck();
        playerName.transform.position = cam.WorldToScreenPoint(gameSceneManager.player.transform.position + new Vector3(0, 2f, 0));
    }
}
