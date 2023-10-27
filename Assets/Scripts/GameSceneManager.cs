using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameSceneManager : MonoBehaviour
{
    [SerializeField]
    private Transform startPos; // 시작 위치
    public CharacterController player; // 플레이어 캐릭터
    private CameraMove cameraMove; // 메인 카메라
    private GameUI ui; // 게임 UI
    public GameObject NPCList; // NPC들
    private void Awake()
    {
        switch (GameManager.Instance.number)
        {
            case 0:
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Character/Archer"), startPos.position,Quaternion.identity).GetComponent<CharacterController>();
                break;
            case 1:
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Character/Warrior"), startPos.position, Quaternion.identity).GetComponent<CharacterController>();
                break;
            case 2:
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Character/Wizard"), startPos.position, Quaternion.identity).GetComponent<CharacterController>();
                break;
            default:
                break;
        }
        player.name = GameManager.Instance.name;
        player.level = GameManager.Instance.level;
        player.HP = GameManager.Instance.HP;
        player.maxHP = GameManager.Instance.maxHP;
        player.MP = GameManager.Instance.MP;
        player.maxMP = GameManager.Instance.maxMP;
        player.EXP = GameManager.Instance.EXP;
        player.Init();
        player.skills[0].active = true;
        player.skills[1].active = true;
        player.skills[2].active = true;
        Refresh();
        ui = gameUI.GetComponent<GameUI>();
        ui.Init();
        cameraMove = GameObject.FindObjectOfType<CameraMove>();

        // 페이드 불러오기
        GameManager.Instance.fade.FadeIn();
        AudioManager.Instance.PlayBGM(BGM);
    }

    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject enemyHPBar;
    [SerializeField] private Image enemyHP;
    [SerializeField] private Image HPBar;
    [SerializeField] private Image MPBar;
    [SerializeField] private Image EXPBar;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private TextMeshProUGUI MPText;
    [SerializeField] private TextMeshProUGUI EXPText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject confrim; // 종료 버튼 클릭 시 출력할 UI
    [SerializeField] private GameObject gameOver; // 캐릭터 사망 시 출력할 UI
    [SerializeField] private AudioClip BGM; // 배경 음악

    public void ViewHP(Enemy target)
    {
        if(target != null)
        {
            enemyHPBar.gameObject.SetActive(true);
            enemyHP.fillAmount = (float)target.HP / (float)target.maxHP;
            enemyHPBar.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = target.name;
        }
    }

    public void CloseHP()
    {
        enemyHPBar.gameObject.SetActive(false);
    }

    public void Refresh()
    {
        HPBar.fillAmount = (float)player.HP / (float)player.maxHP;
        MPBar.fillAmount = (float)player.MP / (float)player.maxMP;
        EXPBar.fillAmount = (float)player.EXP / (player.level * 10);

        HPText.text = player.HP.ToString() + " / " + player.maxHP;
        MPText.text = player.MP.ToString() + " / " + player.maxMP;
        EXPText.text = player.EXP.ToString() + " / " + player.level * 10;
        levelText.text = player.level.ToString();
    }
    public void RefreshStat()
    {
        player.equipStat.damage = 0;
        player.equipStat.armor = 0;
        player.equipStat.speed = 0;
        player.equipStat.range = 0;
        for (int i = 0; i < 5; i++)
        {
            if (GameManager.Instance.equip[i] != null)
            {
                player.equipStat.damage += GameManager.Instance.equip[i].damage;
                player.equipStat.armor += GameManager.Instance.equip[i].armor;
                player.equipStat.speed += GameManager.Instance.equip[i].speed;
                player.equipStat.range += GameManager.Instance.equip[i].range;
            }
        }
    }

    public void ShowConfrim()
    {
        AudioManager.Instance.PlaySFX(GameManager.Instance.click);
        confrim.SetActive(true);
    }

    public void CloseConfrim()
    {
        AudioManager.Instance.PlaySFX(GameManager.Instance.click);
        confrim.SetActive(false);
    }

    public void ExitGame()
    {
        AudioManager.Instance.PlaySFX(GameManager.Instance.click);
        GameManager.Instance.fade.FadeOut();
        Invoke("EndGame", 2f);
    }

    void EndGame()
    {
        GameManager.Instance.LoadScene("TitleScene");
    }

    public void SavePlayerData()
    {
        AudioManager.Instance.PlaySFX(GameManager.Instance.click);
        GameManager.Instance.name = player.name;
        GameManager.Instance.level = player.level;
        GameManager.Instance.HP = player.HP;
        GameManager.Instance.maxHP = player.maxHP;
        GameManager.Instance.MP = player.MP;
        GameManager.Instance.maxMP = player.maxMP;
        GameManager.Instance.EXP = player.EXP;
        GameManager.Instance.questNum = ui.questIndex;
        GameManager.Instance.SaveData();
    }

    public void LevelUp()
    {
        AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("AudioSource/SFX/LevelUp"));
        switch (GameManager.Instance.number)
        {
            case 0:
                player.maxHP += 3;
                player.maxMP += 4;
                break;
            case 1:
                player.maxHP += 5;
                player.maxMP += 2;
                break;
            case 2:
                player.maxHP += 2;
                player.maxMP += 10;
                break;
        }
        if (player.level % 5 == 0)
            player.damage++;
        player.HP = player.maxHP;
        player.MP = player.maxMP;
        ui.LevelUpUI(player.level);
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
    }

    public void ReStart()
    {// 부활
        GameManager.Instance.fade.FadeOut();
        Invoke("Resurrection", 2f);
    }

    void Resurrection()
    {
        player.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
        gameOver.SetActive(false);
        player.Resurrection();
        player.navMesh.enabled = false;
        player.transform.position = startPos.position;
        player.navMesh.enabled = true;
        Refresh();
        GameManager.Instance.fade.FadeIn();
    }

    public void ControllEnable()
    {
        player.isControll = true;
        cameraMove.isControll = true;
    }

    public void ControllDisable()
    {
        player.isControll = false;
        cameraMove.isControll = false;
    }
}
