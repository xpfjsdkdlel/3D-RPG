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

    [SerializeField]
    private MonsterState state = new MonsterState();
    private Animator animator;
    private GameObject target;
    private CharacterController enemy;

    private Collider collider;
    private Rigidbody rigidbody;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        HP = maxHP;
        MP = maxMP;
        state = MonsterState.Idle;
        collider = GetComponent<Collider>();
        collider.enabled = true;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
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
            animator.SetTrigger("die");
            enemy.EXP += EXP;
            collider.enabled = false;
            rigidbody.isKinematic = true;
        }
        else
        {
            enemy.GetComponent<Animator>().SetTrigger("hit");
        }
    }

    void Update()
    {
        
    }
}
