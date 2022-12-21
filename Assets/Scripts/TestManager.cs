using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;


public class TestManager : MonoBehaviour
{
    private Fade fade;
    private CharacterController Player;
    [SerializeField]
    private GameObject HPbar;
    [SerializeField]
    private Image enemyHP;
    private void Start()
    {
        fade = GameObject.FindObjectOfType<Fade>();
        if (fade == null)
        {
            fade = Instantiate(Resources.Load<Fade>("Prefabs/UI/Fade"));
            fade.Init();
        }
        fade.FadeIn();
        HPbar = GameObject.Find("UI").transform.GetChild(0).gameObject;
        enemyHP = HPbar.transform.GetChild(0).GetComponent<Image>();
        HPbar.gameObject.SetActive(false);
        Player = GameObject.FindObjectOfType<CharacterController>();
    }

    public void ViewHP(Enemy target)
    {
        HPbar.gameObject.SetActive(true);
        enemyHP.fillAmount = (float)target.HP / (float)target.maxHP;
        HPbar.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = target.name.ToString();
    }

    void Update()
    {    
        if(Player.enemy != null)
            ViewHP(Player.enemy);
        else
            HPbar.gameObject.SetActive(false);

    }
}
