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
    public int uid; // ������ id
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
    private GameUI gameUI;

    private Collider collider;
    private Rigidbody rigidbody;
    private NavMeshAgent navMesh;

    [SerializeField]
    private GameSceneManager sceneManager;
    [SerializeField]
    private ItemSpawner itemSpawner;

    private void Awake()
    {
        gameUI = GameUI.FindObjectOfType<GameUI>();
    }

    void OnEnable()
    {
        Init();
        sceneManager = GameObject.FindObjectOfType<GameSceneManager>();
        itemSpawner = GameObject.FindObjectOfType<ItemSpawner>();
    }
    
    void Init()
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
        isDead = false;
    }

    void AttackAnim()
    {// ���� �ִϸ��̼�
        if (!enemy.isDead)
        {
            transform.LookAt(enemy.transform.position);
            animator.SetTrigger("attack");
            attackPrevTime = Time.time; // ������ �ð� ����
            attackState = true;
        }
    }

    void Attack()
    {// ������ ������� �ִ� �Լ�
        if(!enemy.invincible)
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
            HP = 0;
            collider.enabled = false;
            navMesh.enabled = false;
            isDead = true;
            animator.SetTrigger("death");
            if (gameUI.quest != null && gameUI.quest.targetId == uid && gameUI.quest.progress < gameUI.quest.complete)
            {
                gameUI.quest.progress++;
                gameUI.RefreshQuestUI(gameUI.quest);
            }
            // ����ġ �ִ� �ڵ�
            sceneManager.player.GetEXP(EXP);
            itemSpawner.DropItem(transform.position);
            Invoke("Delete", 5f);
        }
        else
            animator.SetTrigger("hit");
    }

    public void StartStun(float time)
    {
        state = MonsterState.stun;
        animator.SetBool("stun", true);
        Invoke("EndStun", time);
    }

    void EndStun()
    {
        state = MonsterState.Idle;
        animator.SetBool("stun", false);
    }

    void UpdateTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        if (target != null)
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

    public void Delete()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isDead)
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
                        else if (enemy.isDead)
                            state = MonsterState.Idle;
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
}
