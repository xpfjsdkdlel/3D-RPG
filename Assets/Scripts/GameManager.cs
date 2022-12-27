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

    public int number = 0; // Ŭ����
    public string name;
    public int level = 1; // ����
    public int HP = 50; // ���� ü��
    public int maxHP = 50; // �ִ� ü��
    public int MP = 20; // ���� ����
    public int maxMP = 20; // �ִ� ����
    public int EXP = 0; // ����ġ

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
