using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Idle,
    move,
    attack,
    stun,
    down,
}

public class Enemy : MonoBehaviour
{
    public int HP; // ���� ü��
    public int maxHP; // �ִ� ü��
    public int MP; // ���� ����
    public int maxMP; // �ִ� ����
    public int EXP; // ����ġ
    public int damage; // ���ݷ�
    public int armor; // ����
    public float range; // �����Ÿ�
    public float attackDelay; // ���� ������
    public float speed = 2.0f; // �̵��ӵ�

    [SerializeField]
    private MonsterState state = new MonsterState();

    public void Init()
    {
        HP = maxHP;
        MP = maxMP;
        state = MonsterState.Idle;
        this.enabled = true;
    }

    void Update()
    {
        
    }
}
