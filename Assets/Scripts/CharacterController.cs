using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterState
{
    Idle,
    move,
    attack,
    stun,
    down,
}

public class CharacterController : MonoBehaviour
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
    public float speed = 1.0f; // �̵��ӵ�

    private bool isControll = true; // ��� ������ ����
    private bool battle = false; // ���� ����
    private float attackPrevTime = 0f; // ���������� ������ �ð�
    private float attackTime = 0f; // ���� �� �帥 �ð�
    private bool attackState = false; // false�� ������ ������ ����
    public GameObject target; // ���� �� Ÿ��
    private Enemy enemy; // Ÿ���� ����
    private bool combo = false; // �ι�° ���� �ִϸ��̼� ��� ����

    private Animator animator;
    private NavMeshAgent navMesh;

    [SerializeField]
    private GameObject moveDir; // �̵� �� ��ġ ǥ��

    [SerializeField]
    private CharacterState state = new CharacterState();

    private RaycastHit hit; // �̵� �� ���

    void Start()
    {
        HP = maxHP;
        MP = maxMP;
        animator = GetComponent<Animator>();
        isControll = true;
        state = CharacterState.Idle;
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = speed;
        enabled = true;
    }
    
    void GetInput()
    {
        if (isControll)
        {
            // RŰ�� ���� ���� ���¿� �޽� ���¸� ����
            if (Input.GetKeyDown(KeyCode.R))
                ChangeMode();
            if (Input.GetMouseButtonDown(0))
            {
                
            }
            if (Input.GetMouseButtonDown(1))
            {
                // ȭ���� ��Ŭ�� ���� ��
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.transform.gameObject.CompareTag("Enemy"))
                    {
                        // ���� ó��
                        target = hit.transform.gameObject;
                        state = CharacterState.attack;
                    }
                    else
                    {
                        target = null;
                        state = CharacterState.move;
                    }
                }
            }
        }
        else
        {
            // �ӽ� �ڵ�(���� ����)
            state = CharacterState.Idle;
        }
    }

    void ChangeMode()
    {
        battle = !battle;
        if (battle)
        {
            animator.SetBool("battle", true); // ���� ������
            state = CharacterState.Idle;
        }
        else
        {
            animator.SetBool("battle", false); // ���� ����ֱ�
            state = CharacterState.Idle;
        }
    }

    void MoveStop()
    {// ���� ����
        navMesh.ResetPath();
        navMesh.velocity = Vector3.zero;
        animator.SetBool("isWalk", false);
    }

    void AttackState()
    {// ���� ����
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= range)
            {
                MoveStop();
                if (battle && !attackState)
                {
                    transform.LookAt(target.transform.position); // ���� �ٶ�
                    // ���� �Լ� ȣ��
                    attackPrevTime = Time.time; // ������ �ð� ����
                    attackState = true;
                    animator.SetTrigger("attack");
                    animator.SetBool("combo", combo);
                    combo = !combo; // ���� ������ �ִϸ��̼� ����
                    Attack();
                }
            }
            else
                MoveState(target.transform.position);
        }
        else
        {
            state = CharacterState.Idle;
        }
    }

    void Attack()
    {// ������ ������� ������ �Լ�
        if(target != null)
        {
            enemy = target.GetComponent<Enemy>();
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
            this.enabled = false;
        }
        else
        {
            enemy.GetComponent<Animator>().SetTrigger("hit");
        }
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

    void MoveState(Vector3 moveDir)
    {
        navMesh.SetDestination(moveDir);
        // �̵� �ִϸ��̼�
        animator.SetBool("isWalk", true);
        // �������� �����ߴٸ� �����·� ����
        if (navMesh.velocity.sqrMagnitude >= 0.2f * 0.2f && navMesh.remainingDistance <= 0.1f)
            state = CharacterState.Idle;
    }

    void Update()
    {
        GetInput(); // ����� �Է� �ޱ�
        UpdateAttackInfo(); // ���� ��Ÿ�� üũ
        switch (state)
        {
            case CharacterState.Idle:
                target = null;
                moveDir.SetActive(false);
                MoveStop();
                break;
            case CharacterState.move:
                target = null;
                moveDir.SetActive(true);
                moveDir.transform.position = hit.point + new Vector3(0, 0.1f, 0);
                MoveState(hit.point);
                break;
            case CharacterState.attack:
                moveDir.SetActive(false);
                AttackState();
                break;
            case CharacterState.stun:
                break;
            case CharacterState.down:
                break;
        }
    }
}
