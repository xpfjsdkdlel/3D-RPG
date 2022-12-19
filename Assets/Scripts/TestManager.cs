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
        HPbar = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        enemyHP = HPbar.transform.GetChild(1).GetComponent<Image>();
        HPbar.gameObject.SetActive(false);
        Player = GameObject.FindObjectOfType<CharacterController>();
    }

    public void ViewHP(GameObject target)
    {
        Enemy enemy;
        if(target.TryGetComponent<Enemy>(out enemy))
        {
            HPbar.gameObject.SetActive(true);
            enemyHP.fillAmount = (float)enemy.HP / (float)enemy.maxHP;
            HPbar.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = enemy.name.ToString();
        }
    }

    void Update()
    {    
        if(Player.target != null)
            ViewHP(Player.target);
        else
            HPbar.gameObject.SetActive(false);

    }
}
