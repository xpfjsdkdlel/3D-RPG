using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterState
{
    Idle,
    chase,
    stun,
    retreat,
    skill1,
    skill2,
    skill3,
}

public enum MonsterType
{
    nomal,
    boss,
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
    private bool getAttack = false; // ������ �޾Ҵ��� ����
    protected bool bodyAttack = false; // �÷��̾�� ���˽� Ÿ�� ����(���ڵ�)

    [SerializeField]
    protected float dis; // �÷��̾���� �Ÿ�
    protected float attackPrevTime = 0f; // ���������� ������ �ð�
    protected float attackTime = 0f; // ���� �� �帥 �ð�
    protected bool attackState = false; // false�� ������ ������ ����
    [SerializeField]
    protected MonsterState state = new MonsterState(); // ������ ����
    [SerializeField]
    private MonsterType type = new MonsterType(); // ���� Ÿ��
    private Vector3 spawnPos;

    protected Animator animator;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    protected CharacterController enemy;
    private GameUI gameUI;

    private Collider collider;
    private Rigidbody rigidbody;
    protected NavMeshAgent navMesh;

    [SerializeField]
    protected GameSceneManager sceneManager;
    [SerializeField]
    private ItemSpawner itemSpawner;
    private AudioClip hitSound;

    private void Awake()
    {
        gameUI = GameUI.FindObjectOfType<GameUI>();
        hitSound = Resources.Load<AudioClip>("AudioSource/SFX/EnemyHit");
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
        getAttack = false;
        spawnPos = transform.position;
    }

    protected void AttackAnim()
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

    public void GetDamage(int damage, float stunTime = 0)
    {// ������ ������� �޴� �Լ�
        if (damage <= armor)
            HP -= 1;
        else
            HP -= damage - armor;
        AudioManager.Instance.PlaySFX(hitSound);
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
            sceneManager.CloseHP();
            itemSpawner.DropItem(transform.position);
            Invoke("Delete", 5f);
        }
        else
        {
            sceneManager.ViewHP(this);
            if (stunTime > 0)
                StartStun(stunTime);
            else
            {
                animator.SetTrigger("hit");
                getAttack = true;
            }
        }
    }

    void StartStun(float time)
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

    public void Retreat()
    {
        state = MonsterState.retreat;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && bodyAttack)
        {
            if (!other.GetComponent<CharacterController>().invincible)
                enemy.GetDamage(damage * 2);
        }
    }

    void Update()
    {
        if (!isDead)
        {
            UpdateTarget(); // �÷��̾���� �Ÿ� üũ
            UpdateAttackInfo(); // ���� ��Ÿ�� üũ
            if (type == MonsterType.nomal)
            {
                switch (state)
                {
                    case MonsterState.Idle:
                        navMesh.ResetPath();
                        navMesh.velocity = Vector3.zero;
                        animator.SetBool("isWalk", false);
                        if (dis < 10 || getAttack)
                            state = MonsterState.chase;
                        break;
                    case MonsterState.chase:
                        if (!attackState)
                        {
                            if (dis <= range)
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
                    case MonsterState.retreat:
                        navMesh.SetDestination(spawnPos);
                        animator.SetBool("isWalk", true);
                        getAttack = false;
                        if (Vector3.Distance(transform.position, spawnPos) < 2)
                        {// ���� �� ü�� ȸ��
                            state = MonsterState.Idle;
                            HP = maxHP;
                            sceneManager.Refresh();
                        }
                        break;
                }
            }
        }
    }
}
