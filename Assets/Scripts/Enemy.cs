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
    public int HP; // 현재 체력
    public int maxHP; // 최대 체력
    public int MP; // 현재 마나
    public int maxMP; // 최대 마나
    public int EXP; // 경험치
    public int damage; // 공격력
    public int armor; // 방어력
    public float range; // 사정거리
    public float attackDelay; // 공격 딜레이
    public float speed = 2.0f; // 이동속도

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
    {// 적에게 대미지를 입히는 함수
        CharacterController enemy = target.GetComponent<CharacterController>();
        enemy.Hit(damage);
    }

    public void Hit(int damage)
    {// 대미지를 입는 함수
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
