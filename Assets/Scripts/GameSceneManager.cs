using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameSceneManager : MonoBehaviour
{
    [SerializeField]
    private Transform startPos; // 시작 위치
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
        player.gold = GameManager.Instance.gold;
        Refresh();
        cam = Camera.main;
        playerName.transform.GetComponent<TextMeshProUGUI>().text = player.name;
    }
    private Camera cam = null;

    public CharacterController player;
    [SerializeField]
    private GameObject playerName;
    [SerializeField]
    private GameObject gameUI;
    [SerializeField]
    private Image HPBar;
    [SerializeField]
    private Image MPBar;
    [SerializeField]
    private Image EXPBar;
    [SerializeField]
    private TextMeshProUGUI HPText;
    [SerializeField]
    private TextMeshProUGUI MPText;
    [SerializeField]
    private TextMeshProUGUI EXPText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private GameObject enemyHPBar;
    [SerializeField]
    private Image enemyHP;
    [SerializeField]
    private GameObject levelUpUI; // 레벨업 시 출력할 UI
    [SerializeField]
    private GameObject confrim; // 종료 버튼 클릭 시 출력할 UI
    [SerializeField]
    private AudioClip BGM; // 배경 음악

    private void Start()
    {
        // 페이드 불러오기
        GameManager.Instance.fade.FadeIn();
        AudioManager.Instance.PlayBGM(BGM);
    }

    public void ViewHP(Enemy target)
    {
        enemyHPBar.gameObject.SetActive(true);
        enemyHP.fillAmount = (float)target.HP / (float)target.maxHP;
        enemyHPBar.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = target.name;
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

    public void ShowConfrim()
    {
        confrim.SetActive(true);
    }

    public void CloseConfrim()
    {
        confrim.SetActive(false);
    }

    public void ExitGame()
    {
        GameManager.Instance.fade.FadeOut();
        Invoke("EndGame", 2f);
    }

    void EndGame()
    {
        GameManager.Instance.LoadScene("TitleScene");
    }

    public void SavePlayerData()
    {
        GameManager.Instance.name = player.name;
        GameManager.Instance.level = player.level;
        GameManager.Instance.HP = player.HP;
        GameManager.Instance.maxHP = player.maxHP;
        GameManager.Instance.MP = player.MP;
        GameManager.Instance.maxMP = player.maxMP;
        GameManager.Instance.EXP = player.EXP;
        GameManager.Instance.gold = player.gold;
    }

    public void LevelUp()
    {
        levelUpUI.SetActive(true);
        levelUpUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = player.level.ToString();
        levelUpUI.GetComponent<Animation>().Play();
        Invoke("DisableLevel", 6f);
    }

    void DisableLevel()
    {
        levelUpUI.SetActive(false);
    }

    void Update()
    {
        playerName.transform.position = cam.WorldToScreenPoint(player.transform.position + new Vector3(0, 2f, 0));
    }
}
