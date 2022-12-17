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
    private Animator animator;
    private GameObject target;

    public void Init()
    {
        HP = maxHP;
        MP = maxMP;
        state = MonsterState.Idle;
        this.enabled = true;
    }

    void GetDamage()
    {// ������ ������� ������ �Լ�
        CharacterController enemy = target.GetComponent<CharacterController>();
        enemy.Hit(damage);
    }

    public void Hit(int damage)
    {// ������� �Դ� �Լ�
        HP -= damage - armor;
        if (HP <= 0)
        {
            animator.SetTrigger("die");
            this.enabled = false;
        }
        else
        {
            animator.SetTrigger("hit");
        }
    }

    void Update()
    {
        
    }
}
