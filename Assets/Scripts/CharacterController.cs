using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterState
{
    Idle,
    move,
    attack,
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
    private GameObject target; // ���� �� Ÿ��

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
                        // �̵� ó��
                        state = CharacterState.move;
                    }
                }
            }
        }
        else
        {
            
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

    void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < range)
        {
            if (battle && target != null)
            {
                if (!attackState)
                {
                    navMesh.destination = transform.position;
                    animator.SetBool("isWalk", false);
                    animator.SetBool("attack", true);
                    // ���� �Լ� ȣ��
                    attackPrevTime = Time.time; // ������ �ð� ����
                }
            }
            else
                state = CharacterState.Idle;

        }
        else
        {
            animator.SetBool("attack", false);
            navMesh.destination = target.transform.position;
            // �̵� �ִϸ��̼�
            animator.SetBool("isWalk", true);
        }
    }

    void UpdateAttackInfo()
    {
        // ������ �� ���¶��, ������ �ð��� �����ڿ� �ٽ� ������ �� �ֵ��� ����
        if (attackState)
        {
            attackTime = Time.time - attackPrevTime;
            if (attackTime > attackDelay)
            {
                attackState = false;
            }
        }
    }

    void Move()
    {
        navMesh.destination = hit.point;
        // �̵� �ִϸ��̼�
        animator.SetBool("isWalk", true);
        // �������� �����ߴٸ� �����·� ����
        if (navMesh.velocity.sqrMagnitude >= 0.2f * 0.2f && navMesh.remainingDistance <= 0.1f)
        {
            state = CharacterState.Idle;
        }
    }

    void Update()
    {
        GetInput(); // ����� �Է� �ޱ�
        UpdateAttackInfo(); // ���� ��Ÿ�� üũ
        switch (state)
        {
            case CharacterState.Idle:
                target = null;
                animator.SetBool("isWalk", false);
                animator.SetBool("attack", false);
                moveDir.SetActive(false);
                navMesh.destination = transform.position;
                break;
            case CharacterState.move:
                target = null;
                animator.SetBool("attack", false);
                moveDir.SetActive(true);
                moveDir.transform.position = hit.point + new Vector3(0, 0.1f, 0);
                Move();
                break;
            case CharacterState.attack:
                moveDir.SetActive(false);
                Attack();
                break;
        }
    }
}
