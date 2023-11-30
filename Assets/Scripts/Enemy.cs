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
    public int uid; // 몬스터의 id
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
    public bool superArmor = false; // 슈퍼아머
    private bool getAttack = false; // 공격을 받았는지 여부
    private bool bodyAttack = false; // 플레이어와 접촉시 타격 여부(몸박뎀)
    [SerializeField]
    private AudioClip deathSound;

    private float dis; // 플레이어와의 거리
    private float attackPrevTime = 0f; // 마지막으로 공격한 시간
    private float attackTime = 0f; // 공격 후 흐른 시간
    private bool attackState = false; // false면 공격이 가능한 상태
    [SerializeField]
    private MonsterState state = new MonsterState(); // 몬스터의 상태
    [SerializeField]
    private MonsterType type = new MonsterType(); // 몬스터 타입
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

    private float coolDown = 0f; // 흐른 시간
    private float prevTime = 0f; // 마지막 스킬을 사용한 시간
    private bool isCoolDown = true; // false면 스킬 사용
    private int skillNum = 0; // 보스가 시전할 스킬 번호
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
    {// 공격 애니메이션
        if (!enemy.isDead)
        {
            transform.LookAt(enemy.transform.position);
            animator.SetTrigger("attack");
            attackPrevTime = Time.time; // 공격한 시간 갱신
            attackState = true;
        }
    }

    void Attack()
    {// 적에게 대미지를 주는 함수
        if(!enemy.invincible)
            enemy.GetDamage(damage);
    }

    public void GetDamage(int damage, float stunTime = 0)
    {// 적에게 대미지를 받는 함수
        if (damage <= armor)
            HP -= 1;
        else
            HP -= damage - armor;
        AudioManager.Instance.PlaySFX(hitSound);
        if (HP <= 0)
        {// 몬스터 사망 시
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
            // 경험치 주는 코드
            sceneManager.player.GetEXP(EXP);
            sceneManager.CloseHP();
            itemSpawner.DropItem(transform.position);
            AudioManager.Instance.PlaySFX(deathSound);
            Invoke("Delete", 5f);
        }
        else
        {// 몬스터 생존 시
            sceneManager.ViewHP(this); // 체력 UI출력
            if (superArmor)
                getAttack = true; // 어그로 상태 변경
            else if (stunTime > 0)
                StartStun(stunTime);
            else
            {// 피격 애니메이션
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
        // 공격을 한 상태라면, 일정한 시간이 지난뒤에 다시 공격할 수 있도록 변경
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
    {// 돌진 공격
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
    {// 장판 소환
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
    {// 회복 패턴
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
            {// 목표한 적이 없다면 탐색
                enemy = FindObjectOfType<CharacterController>();
                target = GameObject.FindGameObjectWithTag("Player");
            }
            UpdateTarget(); // 플레이어와의 거리 체크
            UpdateAttackInfo(); // 공격 쿨타임 체크
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
                    {// 후퇴 시 체력 회복
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
