using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameSceneManager : MonoBehaviour
{
    private void Awake()
    {
        switch (GameManager.Instance.number)
        {
            case 0:
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Character/Archer")).GetComponent<CharacterController>();
                break;
            case 1:
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Character/Warrior")).GetComponent<CharacterController>();
                break;
            case 2:
                player = Instantiate(Resources.Load<GameObject>("Prefabs/Character/Wizard")).GetComponent<CharacterController>();
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
        refresh();
    }

    private Fade fade;
    [SerializeField]
    private CharacterController player;
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
    private Vector3 startPos; // 시작 위치

    private void Start()
    {
        // 페이드 불러오기
        fade = GameObject.FindObjectOfType<Fade>();
        if (fade == null)
        {
            fade = Instantiate(Resources.Load<Fade>("Prefabs/UI/Fade"));
            fade.Init();
        }
        fade.FadeIn();
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

    public void refresh()
    {
        HPBar.fillAmount = (float)player.HP / (float)player.maxHP;
        MPBar.fillAmount = (float)player.MP / (float)player.maxMP;
        EXPBar.fillAmount = (float)player.EXP / (player.level * 10);

        HPText.text = player.HP.ToString() + " / " + player.maxHP;
        MPText.text = player.MP.ToString() + " / " + player.maxMP;
        EXPText.text = player.EXP.ToString() + " / " + player.level * 10;
    }

    void Update()
    {    

    }
}
