using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public string name; // 이름
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
    public bool isDead = false; // 생존 여부
    [SerializeField]
    private MonsterState state = new MonsterState();
    private Animator animator;
    private GameObject target;
    private CharacterController enemy;

    private Collider collider;
    private Rigidbody rigidbody;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        HP = maxHP;
        MP = maxMP;
        state = MonsterState.Idle;
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        collider.enabled = true;
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
    }

    void Attack()
    {// 적에게 대미지를 입히는 함수
        if (target != null)
        {
            enemy = target.GetComponent<CharacterController>();
            enemy.GetDamage(damage);
        }
    }

    public void GetDamage(int damage)
    {// 적에게 대미지를 받는 함수
        if (damage <= armor)
            HP -= 1;
        else
            HP -= damage - armor;
        if (HP <= 0)
        {
            this.enabled = false;
            collider.enabled = false;
            navMeshAgent.enabled = false;
            isDead = true;
            animator.SetTrigger("death");
            Invoke("Delete", 5f);
            // 경험치 주는 코드
        }
        else
        {
            animator.SetTrigger("hit");
        }
    }

    public void DropItem()
    {

    }

    public void Delete()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
