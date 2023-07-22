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
}

public enum MonsterType
{
    nomal,
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
    private bool getAttack = false; // 공격을 받았는지 여부
    private bool bodyAttack = false; // 플레이어와 접촉시 타격 여부(몸박뎀)

    [SerializeField]
    private float dis; // 플레이어와의 거리
    private float attackPrevTime = 0f; // 마지막으로 공격한 시간
    private float attackTime = 0f; // 공격 후 흐른 시간
    private bool attackState = false; // false면 공격이 가능한 상태
    [SerializeField]
    private MonsterState state = new MonsterState(); // 몬스터의 상태
    [SerializeField]
    private MonsterType type = new MonsterType(); // 몬스터 타입
    private int skill = 0; // 보스가 시전할 스킬 번호
    private Vector3 spawnPos;

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

    void AttackAnim()
    {// 공격 애니메이션
        if (!enemy.isDead)
        {
            transform.LookAt(enemy.transform.position);
            animator.SetTrigger("attack");
            attackPrevTime = Time.time; // 공격한 시간 갱신
            attackState = true;
        }
    }

    void BossSkill1()
    {// 돌진공격

    }

    void BossSkill2()
    {// 몬스터 소환

    }

    void BossSkill3()
    {// 회복

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
            // 경험치 주는 코드
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
        // 공격을 한 상태라면, 일정한 시간이 지난뒤에 다시 공격할 수 있도록 변경
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
            UpdateTarget(); // 플레이어와의 거리 체크
            UpdateAttackInfo(); // 공격 쿨타임 체크
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
                    else
                    {
                        if (type == MonsterType.boss)
                        {
                            // 보스 패턴
                            switch (skill)
                            {
                                case 0:
                                    break;
                                case 1:
                                    break;
                                case 2:
                                    break;
                            }
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
                    }
                    break;
            }
        }
    }
}
