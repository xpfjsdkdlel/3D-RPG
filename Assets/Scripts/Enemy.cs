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
    public string name; // �̸�
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
    public bool isDead = false; // ���� ����

    [SerializeField]
    private float dis; // �÷��̾���� �Ÿ�
    private float attackPrevTime = 0f; // ���������� ������ �ð�
    private float attackTime = 0f; // ���� �� �帥 �ð�
    private bool attackState = false; // false�� ������ ������ ����
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
    {// ���� �ִϸ��̼�
        if (!enemy.isDead)
        {
            animator.SetTrigger("attack");
            attackPrevTime = Time.time; // ������ �ð� ����
            attackState = true;
        }
    }

    void Attack()
    {// ������ ������� �ִ� �Լ�
        enemy.GetDamage(damage);
    }

    public void GetDamage(int damage)
    {// ������ ������� �޴� �Լ�
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
            // ����ġ �ִ� �ڵ�
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
        // ������ �� ���¶��, ������ �ð��� �����ڿ� �ٽ� ������ �� �ֵ��� ����
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
        UpdateTarget(); // �÷��̾���� �Ÿ� üũ
        UpdateAttackInfo(); // ���� ��Ÿ�� üũ
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
