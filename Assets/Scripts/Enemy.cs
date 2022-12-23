using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterState
{
    Idle,
    chase,
    stun,
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
    private float dis; // 플레이어와의 거리
    private float attackPrevTime = 0f; // 마지막으로 공격한 시간
    private float attackTime = 0f; // 공격 후 흐른 시간
    private bool attackState = false; // false면 공격이 가능한 상태
    [SerializeField]
    private MonsterState state = new MonsterState();
    private Animator animator;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private CharacterController enemy;

    private Collider collider;
    private Rigidbody rigidbody;
    private NavMeshAgent navMesh;

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
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.enabled = true;
    }

    void AttackAnim()
    {// 공격 애니메이션
        if (!enemy.isDead)
        {
            animator.SetTrigger("attack");
            attackPrevTime = Time.time; // 공격한 시간 갱신
            attackState = true;
        }
    }

    void Attack()
    {// 적에게 대미지를 주는 함수
        enemy.GetDamage(damage);
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
            navMesh.enabled = false;
            isDead = true;
            animator.SetTrigger("death");
            Invoke("Delete", 5f);
            // 경험치 주는 코드
        }
        else
            animator.SetTrigger("hit");
    }

    void StartStun()
    {
        state = MonsterState.stun;
    }

    void EndStun()
    {
        state = MonsterState.Idle;
    }

    void UpdateTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        if(target != null)
            enemy = target.GetComponent<CharacterController>();
        dis = Vector3.Distance(transform.position, enemy.transform.position);
    }

    void UpdateAttackInfo()
    {
        // 공격을 한 상태라면, 일정한 시간이 지난뒤에 다시 공격할 수 있도록 변경
        if (attackState)
        {
            attackTime = Time.time - attackPrevTime;
            if (attackTime > attackDelay)
                attackState = false;
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
        UpdateTarget(); // 플레이어와의 거리 체크
        UpdateAttackInfo(); // 공격 쿨타임 체크
        switch (state)
        {
            case MonsterState.Idle:
                navMesh.ResetPath();
                navMesh.velocity = Vector3.zero;
                animator.SetBool("isWalk", false);
                if (dis < 10)
                    state = MonsterState.chase;
                break;
            case MonsterState.chase:
                if (!attackState)
                {
                    if (dis > 15)
                        state = MonsterState.Idle;
                    else if (dis <= range)
                    {
                        navMesh.ResetPath();
                        navMesh.velocity = Vector3.zero;
                        animator.SetBool("isWalk", false);
                        AttackAnim();
                    }
                    else
                    {
                        navMesh.SetDestination(enemy.transform.position);
                        animator.SetBool("isWalk", true);
                    }
                }
                break;
            case MonsterState.stun:
                navMesh.ResetPath();
                navMesh.velocity = Vector3.zero;
                break;
        }
    }
}
