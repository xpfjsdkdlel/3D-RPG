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
    normal,
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
    public bool superArmor = false; // ���۾Ƹ�
    private bool getAttack = false; // ������ �޾Ҵ��� ����
    private bool bodyAttack = false; // �÷��̾�� ���˽� Ÿ�� ����(���ڵ�)
    [SerializeField]
    private AudioClip deathSound;

    private float dis; // �÷��̾���� �Ÿ�
    private float attackPrevTime = 0f; // ���������� ������ �ð�
    private float attackTime = 0f; // ���� �� �帥 �ð�
    private bool attackState = false; // false�� ������ ������ ����
    [SerializeField]
    private MonsterState state = new MonsterState(); // ������ ����
    [SerializeField]
    private MonsterType type = new MonsterType(); // ���� Ÿ��
    private Vector3 spawnPos;
    [SerializeField] private GameObject skill1;
    [SerializeField] private GameObject skill2;
    [SerializeField] private GameObject skill3;

    private Animator animator;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private CharacterController enemy;
    private GameUI gameUI;

    private CapsuleCollider collider;
    private Rigidbody rigidbody;
    private NavMeshAgent navMesh;

    private float coolDown = 0f; // �帥 �ð�
    private float prevTime = 0f; // ������ ��ų�� ����� �ð�
    private bool isCoolDown = true; // false�� ��ų ���
    private int skillNum = 0; // ������ ������ ��ų ��ȣ
    private Vector3 rushPosition = new Vector3(0, 0, 0);

    [SerializeField]
    private GameSceneManager sceneManager;
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
        collider = GetComponent<CapsuleCollider>();
        collider.enabled = true;
        rigidbody = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.enabled = true;
        isDead = false;
        getAttack = false;
        spawnPos = transform.position;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void AttackAnim()
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
        {// ���� ��� ��
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
            AudioManager.Instance.PlaySFX(deathSound);
            Invoke("Delete", 5f);
        }
        else
        {// ���� ���� ��
            sceneManager.ViewHP(this); // ü�� UI���
            if (superArmor)
                getAttack = true; // ��׷� ���� ����
            else if (stunTime > 0)
                StartStun(stunTime);
            else
            {// �ǰ� �ִϸ��̼�
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

    void UpdateTarget()
    {
        if (target != null)
        {
            enemy = target.GetComponent<CharacterController>();
            dis = Vector3.Distance(transform.position, enemy.transform.position);
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
        if(type == MonsterType.boss && isCoolDown)
        {
            coolDown = Time.time - prevTime;
            if (coolDown > 15)
                isCoolDown = false;
        }
    }

    public void Delete()
    {
        gameObject.SetActive(false);
    }

    void bossSkill1()
    {// ���� ����
        rushPosition = enemy.transform.position;
        transform.LookAt(rushPosition);
        navMesh.ResetPath();
        navMesh.velocity = Vector3.zero;
        navMesh.speed = 40f;
        AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("AudioSource/SFX/BossSkill1"));
    }

    void Charge()
    {
        navMesh.SetDestination(rushPosition);
        animator.SetBool("isWalk", true);
        state = MonsterState.skill1;
        bodyAttack = true;
        skill1.SetActive(true);
        collider = transform.GetComponent<CapsuleCollider>();
        collider.center = new Vector3(0, 0.75f, 1f);
    }

    void bossSkill2()
    {// ���� ��ȯ
        skill2.SetActive(true);
        skill2.transform.parent = null;
        skill2.transform.position = enemy.transform.position + new Vector3(0, 0.2f, 0);
        AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("AudioSource/SFX/BossSkill2(1)"));
    }

    void bossSkill2End()
    {
        attackPrevTime = Time.time;
        attackState = true;
        prevTime = Time.time;
        isCoolDown = true;
        state = MonsterState.chase;
    }

    void bossSkill3()
    {// ȸ�� ����
        attackPrevTime = Time.time;
        attackState = true;
        prevTime = Time.time;
        isCoolDown = true;
        HP += 20;
        skill3.SetActive(false);
        skill3.SetActive(true);
        AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("AudioSource/SFX/BossSkill3"));
        sceneManager.Refresh();
    }

    void endSkill3()
    {
        state = MonsterState.chase;
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
            if(enemy == null)
            {// ��ǥ�� ���� ���ٸ� Ž��
                enemy = FindObjectOfType<CharacterController>();
                target = GameObject.FindGameObjectWithTag("Player");
            }
            UpdateTarget(); // �÷��̾���� �Ÿ� üũ
            UpdateAttackInfo(); // ���� ��Ÿ�� üũ
            switch (state)
            {
                case MonsterState.Idle:
                    navMesh.ResetPath();
                    navMesh.velocity = Vector3.zero;
                    animator.SetBool("isWalk", false);
                    if (!enemy.isDead && (dis < 10 || getAttack))
                        state = MonsterState.chase;
                    break;
                case MonsterState.chase:
                    if (Vector3.Distance(transform.position, spawnPos) > 35)
                        state = MonsterState.retreat;
                    else if (type == MonsterType.boss && !isCoolDown)
                    {
                        switch (skillNum)
                        {
                            case 0:
                                animator.SetTrigger("Skill1");
                                state = MonsterState.skill1;
                                skillNum++;
                                prevTime = Time.time;
                                isCoolDown = true;
                                break;
                            case 1:
                                animator.SetTrigger("Skill2");
                                state = MonsterState.skill2;
                                skillNum++;
                                prevTime = Time.time;
                                isCoolDown = true;
                                break;
                            case 2:
                                animator.SetTrigger("Skill3");
                                state = MonsterState.skill3;
                                skillNum = 0;
                                prevTime = Time.time;
                                isCoolDown = true;
                                break;
                        }
                    }
                    else if (!attackState)
                    {
                        if (enemy.isDead)
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
                case MonsterState.retreat:
                    navMesh.SetDestination(spawnPos);
                    animator.SetBool("isWalk", true);
                    getAttack = false;
                    if (Vector3.Distance(transform.position, spawnPos) < 2)
                    {// ���� �� ü�� ȸ��
                        state = MonsterState.Idle;
                        HP = maxHP;
                        sceneManager.CloseHP();
                    }
                    break;
                case MonsterState.skill1:
                    if(Vector3.Distance(transform.position, rushPosition) < 1)
                    {
                        attackPrevTime = Time.time;
                        attackState = true;
                        prevTime = Time.time;
                        isCoolDown = true;
                        animator.SetBool("isWalk", false);
                        transform.GetChild(2).gameObject.SetActive(false);
                        bodyAttack = false;
                        navMesh.speed = speed;
                        collider.center = new Vector3(0, 0.75f, 0.1f);
                        state = MonsterState.chase;
                    }
                    break;
                case MonsterState.skill2:
                    if (!attackState)
                        state = MonsterState.chase;
                    else
                    {
                        transform.LookAt(enemy.transform);
                        navMesh.ResetPath();
                        navMesh.velocity = Vector3.zero;
                    }
                    break;
                case MonsterState.skill3:
                    if (!attackState)
                        state = MonsterState.chase;
                    else
                    {
                        navMesh.ResetPath();
                        navMesh.velocity = Vector3.zero;
                    }
                    break;
            }
        }
    }
}
