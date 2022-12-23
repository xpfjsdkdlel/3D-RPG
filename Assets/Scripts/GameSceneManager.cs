using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameSceneManager : MonoBehaviour
{
    private Fade fade;
    [SerializeField]
    private CharacterController player;
    [SerializeField]
    private GameObject gameUI;
    private Image HPBar;
    private Image MPBar;
    private Image EXPBar;
    private TextMeshProUGUI HPText;
    private TextMeshProUGUI MPText;
    private TextMeshProUGUI EXPText;
    private TextMeshProUGUI levelText;
    [SerializeField]
    private GameObject enemyHPBar;
    [SerializeField]
    private GameObject playerUI;
    [SerializeField]
    private Image enemyHP;
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

        // UI 불러오기
        gameUI = GameObject.Find("GameUI").gameObject;
        if (gameUI == null)
            gameUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/GameUI"));

        // 캐릭터 불러오기
        player = GameObject.FindObjectOfType<CharacterController>();

        enemyHPBar = gameUI.transform.GetChild(0).gameObject;
        enemyHP = enemyHPBar.transform.GetChild(0).GetComponent<Image>();
        enemyHPBar.gameObject.SetActive(false);
        
        playerUI = gameUI.transform.GetChild(1).gameObject;
        HPBar = playerUI.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        MPBar = playerUI.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        EXPBar = playerUI.transform.GetChild(2).GetChild(0).GetComponent<Image>();
        HPText = playerUI.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        MPText = playerUI.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        EXPText = playerUI.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        levelText = playerUI.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>();
        
    }

    public void ViewHP(Enemy target)
    {
        enemyHPBar.gameObject.SetActive(true);
        enemyHP.fillAmount = (float)target.HP / (float)target.maxHP;
        enemyHPBar.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = target.name;
    }

    void refresh()
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
        if(player.enemy != null)
            ViewHP(player.enemy);
        else
            enemyHPBar.gameObject.SetActive(false);
        refresh();
    }
}
