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
    private TextMeshProUGUI levelText;
    [SerializeField]
    private GameObject enemyHPBar;
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
        gameUI = GameObject.Find("GameUI").transform.GetChild(1).gameObject;
        if (gameUI == null)
            gameUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/GameUI"));
        
        HPBar = gameUI.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        MPBar = gameUI.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        EXPBar = gameUI.transform.GetChild(2).GetChild(0).GetComponent<Image>();
        levelText = gameUI.transform.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>();
        enemyHPBar = GameObject.Find("GameUI").transform.GetChild(0).gameObject;
        enemyHP = enemyHPBar.transform.GetChild(0).GetComponent<Image>();
        enemyHPBar.gameObject.SetActive(false);

        // 캐릭터 불러오기
        player = GameObject.FindObjectOfType<CharacterController>();
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
    }

    void Update()
    {    
        if(player.enemy != null)
            ViewHP(player.enemy);
        else
            enemyHPBar.gameObject.SetActive(false);
        //refresh();
    }
}
