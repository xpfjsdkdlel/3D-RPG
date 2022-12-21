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
    {// ������ ������� ������ �Լ�
        if (target != null)
        {
            enemy = target.GetComponent<CharacterController>();
            enemy.GetDamage(damage);
        }
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
            navMeshAgent.enabled = false;
            isDead = true;
            animator.SetTrigger("death");
            Invoke("Delete", 5f);
            // ����ġ �ִ� �ڵ�
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
