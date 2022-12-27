using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get => instance;
    }

    public int number = 0; // 클래스
    public string name;
    public int level = 1; // 레벨
    public int HP = 50; // 현재 체력
    public int maxHP = 50; // 최대 체력
    public int MP = 20; // 현재 마나
    public int maxMP = 20; // 최대 마나
    public int EXP = 0; // 경험치

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
