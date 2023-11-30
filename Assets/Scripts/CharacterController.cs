using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterState
{
    Idle,
    move,
    attack,
    down,
    death,
}

public class EquipStat
{
    public int damage = 0;
    public int armor = 0;
    public int speed = 0;
    public int range = 0;
}

public class BuffStat
{
    public int buffDamage = 0; // 공격력 버프
    public int buffDefense = 0; // 방어력 버프
    public int buffSpeed = 0; // 이동속도 버프
    public int buffRange = 0; // 사거리 버프
}

public class CharacterController : MonoBehaviour
{
    public string name; // 이름
    public int level = 1; // 레벨
    public int HP; // 현재 체력
    public int maxHP; // 최대 체력
    public int MP; // 현재 마나
    public int maxMP; // 최대 마나
    public int EXP; // 경험치
    public int damage; // 공격력
    public int defense; // 방어력
    public float range; // 사정거리
    public float attackDelay; // 공격 딜레이
    public float speed = 1.0f; // 이동속도
    public EquipStat equipStat = new EquipStat(); // 장비 스탯
    public BuffStat buffStat = new BuffStat(); // 버프 스탯
    public bool isDead = false; // 생존 여부
    [SerializeField]
    private GameObject projectile; // 투사체
    public Skill[] skills = new Skill[3]; // 스킬
    [SerializeField]
    private GameObject maxRange;
    public GameObject firePos;

    public bool isControll = true; // 제어가 가능한 상태
    private bool battle = false; // 전투 상태
    public bool invincible = false; // 무적 상태
    private float attackPrevTime = 0f; // 마지막으로 공격한 시간
    private float attackTime = 0f; // 공격 후 흐른 시간
    private bool attackState = false; // false면 공격이 가능한 상태
    private float restTime = 0; // 휴식을 취한 시간
    public Enemy enemy; // 타겟의 정보
    private bool combo = false; // 두번째 공격 애니메이션 출력 여부

    private Animator animator;
    public NavMeshAgent navMesh;

    private RaycastHit hit; // 우클릭 시 이동 할 장소
    private RaycastHit click; // 스킬의 시전위치
    private int layerMask;
    [SerializeField]
    private GameObject moveDir; // 이동 할 위치 표시

    public CharacterState state = new CharacterState();

    private GameSceneManager sceneManager;

    public void Init()
    {
        animator = GetComponent<Animator>();
        isControll = true;
        state = CharacterState.Idle;
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = speed;
        sceneManager = GameObject.FindObjectOfType<GameSceneManager>();
        layerMask = (-1) - (1 << LayerMask.NameToLayer("Spawner")) - (1 << LayerMask.NameToLayer("Skill")) - (1 << LayerMask.NameToLayer("Water"));
        moveDir.transform.parent = null;
    }
    
    void GetInput()
    {
        if (isControll && !isDead)
        {
            // R키를 눌러 전투 상태와 휴식 상태를 변경
            if (Input.GetKeyDown(KeyCode.R))
                ChangeMode();
            if (battle)
            {// 전투 상태일 때 Q, W, E를 눌러 스킬을 사용
                if (Input.GetKeyDown(KeyCode.Q) && skills[0].cost <= MP && skills[0].active)
                {
                    state = CharacterState.Idle;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out click, Mathf.Infinity, layerMask))
                        transform.LookAt(click.point);
                    skills[0].active = false;
                    MP -= skills[0].cost;
                    sceneManager.Refresh();
                    animator.SetTrigger("skill1");
                    Invoke("ActiveSkill1", skills[0].coolDown);
                }
                else if (Input.GetKeyDown(KeyCode.W) && skills[1].cost <= MP && skills[1].active)
                {
                    state = CharacterState.Idle;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out click, Mathf.Infinity, layerMask))
                        transform.LookAt(click.point);
                    skills[1].active = false;
                    MP -= skills[1].cost;
                    sceneManager.Refresh();
                    animator.SetTrigger("skill2");
                    Invoke("ActiveSkill2", skills[1].coolDown);
                }
                else if (Input.GetKeyDown(KeyCode.E) && skills[2].cost <= MP && skills[2].active)
                {
                    state = CharacterState.Idle;
                    invincible = true;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out click, Mathf.Infinity, layerMask))
                        transform.LookAt(click.point);
                    skills[2].active = false;
                    MP -= skills[2].cost;
                    sceneManager.Refresh();
                    animator.SetTrigger("skill3");
                    Invoke("ActiveSkill3", skills[2].coolDown);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit click;
                // 화면을 좌클릭 했을 때
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out click, Mathf.Infinity, layerMask))
                {
                    if (click.transform.gameObject.CompareTag("NPC"))
                    {
                        // NPC 대화문
                        click.transform.GetComponent<NPC>().talk();
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                // 화면을 우클릭 했을 때
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,Mathf.Infinity,layerMask))
                {
                    if (hit.transform.gameObject.CompareTag("Enemy"))
                    {
                        // 몬스터라면 공격
                        enemy = hit.transform.gameObject.GetComponent<Enemy>();
                        state = CharacterState.attack;
                    }
                    else
                    {// 몬스터가 아니면 이동
                        enemy = null;
                        state = CharacterState.move;
                    }
                }
            }
        }
    }

    void ChangeMode()
    {
        battle = !battle;
        if (battle)
        {
            animator.SetBool("battle", true); // 무기 꺼내기
            state = CharacterState.Idle;
        }
        else
        {
            animator.SetBool("battle", false); // 무기 집어넣기
            state = CharacterState.Idle;
        }
    }

    void MoveStop()
    {// 정지 상태
        navMesh.ResetPath();
        navMesh.velocity = Vector3.zero;
        animator.SetBool("isWalk", false);
    }

    void AttackState()
    {// 공격 상태
        if (enemy != null)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range + equipStat.range)
            {
                MoveStop();
                if (battle && !attackState)
                {
                    transform.LookAt(enemy.transform.position); // 적을 바라봄
                    AttackAnim();
                }
            }
            else
                MoveState(enemy.transform.position);
        }
        else
        {
            state = CharacterState.Idle;
            restTime = 0;
        }
    }
    void AttackAnim()
    {// 공격 애니메이션
        if (!enemy.isDead)
        {
            animator.SetTrigger("attack");
            animator.SetBool("combo", combo);
            combo = !combo; // 다음 공격의 애니메이션 변경
            attackPrevTime = Time.time; // 공격한 시간 갱신
            attackState = true;
        }
        else
            enemy = null;
        restTime = 0;
    }

    void Attack()
    {// 적에게 대미지를 입히는 함수
        AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("AudioSource/SFX/Attack"));
        if (!enemy.isDead)
        {
            if (projectile == null)
                enemy.GetDamage(damage + equipStat.damage + buffStat.buffDamage);
        }
    }

    void ProjectileAttack()
    {// 원거리 투사체 공격
        projectile.transform.parent = null;
        projectile.SetActive(true);
        projectile.GetComponent<Projectile>().SetTarget(enemy.transform.position);
        AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("AudioSource/SFX/Attack"));
    }

    public void GetDamage(int damage)
    {// 적에게 대미지를 받는 함수
        if (damage <= defense + equipStat.armor + buffStat.buffDefense)
            HP -= 1;
        else
            HP -= damage - (defense + equipStat.armor + buffStat.buffDefense);
        AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("AudioSource/SFX/Hit"));
        if (HP <= 0)
        {
            HP = 0;
            animator.SetTrigger("death");
            isDead = true;
            state = CharacterState.death;
            moveDir.SetActive(false);
            MoveStop();
            sceneManager.GameOver();
        }
        else
        {
            if (damage - (defense + equipStat.armor + buffStat.buffDefense) > 40)
                animator.SetTrigger("down");
            else if (damage - (defense + equipStat.armor + buffStat.buffDefense) > 10)
            {
                if (isControll)// 애니메이션 꼬임 방지
                    animator.SetTrigger("hit");
            }
        }
        restTime = 0;
        sceneManager.Refresh();
    }

    public void Heal(int point)
    {// 휴식 시 체력 회복
        restTime = 0;
        HP += point;
        if (HP > maxHP)
            HP = maxHP;
        sceneManager.Refresh();
    }

    public void Resurrection()
    {// 캐릭터 사망 후 부활하는 함수
        sceneManager.CloseHP();
        state = CharacterState.Idle;
        isDead = false;
        HP = 10;
        EXP = EXP / 3;
        isControll = true;
        battle = false;
        restTime = 0;
        animator.SetBool("battle", false);
        animator.SetTrigger("resurrection");
    }

    public void SetSpeed()
    {
        navMesh.speed = speed + equipStat.speed;
    }

    public void Refresh()
    {
        sceneManager.Refresh();
    }

    void StartDown()
    {
        isControll = false;
        state = CharacterState.down;
    }

    void EndDown()
    {
        isControll = true;
        state = CharacterState.Idle;
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

    void MoveState(Vector3 moveDir)
    {
        navMesh.SetDestination(moveDir);
        // 이동 애니메이션
        animator.SetBool("isWalk", true);
        // 목적지에 도착했다면 대기상태로 변경
        if (navMesh.velocity.sqrMagnitude >= 0.2f * 0.2f && navMesh.remainingDistance <= 0.1f)
        {
            state = CharacterState.Idle;
            restTime = 0;
        }
        else if (moveDir.x == transform.position.x && moveDir.z == transform.position.z)
        {
            state = CharacterState.Idle;
            restTime = 0;
        }
        navMesh.speed = speed + buffStat.buffSpeed;
    }

    public void GetEXP(int EXP)
    {// 경험치 획득
        if(this.EXP + EXP >= level * 10)
        {// 레벨업
            this.EXP = (this.EXP + EXP) - (level * 10); 
            ++level;
            sceneManager.LevelUp();
            if(this.EXP + EXP >= level * 10)
            {
                this.EXP = (this.EXP + EXP) - (level * 10);
                ++level;
                sceneManager.LevelUp();
            }
        }
        else
            this.EXP += EXP;
        sceneManager.Refresh();
    }

    // 스킬 쿨타임 초기화 하는 함수
    void ActiveSkill1()
    {
        skills[0].active = true;
    }

    void ActiveSkill2()
    {
        skills[1].active = true;
    }

    void ActiveSkill3()
    {
        skills[2].active = true;
    }

    private GameObject archerSkill1;
    void ArcherSkill1()
    {// 더블 샷
        if (archerSkill1 == null)
            archerSkill1 = Instantiate(skills[0].effect, transform.position, Quaternion.identity);
        archerSkill1.SetActive(true);
        if (enemy != null)
            archerSkill1.GetComponent<Projectile>().SetTarget(enemy.transform.position, 2);
        else
            archerSkill1.GetComponent<Projectile>().SetTarget(maxRange.transform.position, 2);
        AudioManager.Instance.PlaySFX(skills[0].sound);
    }

    private GameObject archerSkill2;
    void ArcherSkill2()
    {// 애로우 레인
        if (archerSkill2 == null)
            archerSkill2 = Instantiate(skills[1].effect, click.point + new Vector3(0, 0.2f, 0), Quaternion.Euler(-90, 0, 0));
        else
            archerSkill2.transform.position = click.point + new Vector3(0, 0.2f, 0);
        archerSkill2.SetActive(true);
        archerSkill2.GetComponent<Installation>().SetTarget(3);
        AudioManager.Instance.PlaySFX(skills[1].sound);
        Invoke("ArcherSkill2Sound", 0.5f);
        Invoke("ArcherSkill2End", 2f);
    }

    void ArcherSkill2Sound()
    {
        AudioManager.Instance.PlaySFX(Resources.Load<AudioClip>("AudioSource/SFX/ArrowRain2"));
    }

    void ArcherSkill2End()
    {
        archerSkill2.SetActive(false);
    }

    private GameObject archerSkill3;
    void ArcherSkill3()
    {// 스나이핑
        if (archerSkill3 == null)
            archerSkill3 = Instantiate(skills[2].effect, transform.position, Quaternion.identity);
        archerSkill3.SetActive(true);
        if (enemy != null)
            archerSkill3.GetComponent<Projectile>().SetTarget(enemy.transform.position, 7, true);
        else
            archerSkill3.GetComponent<Projectile>().SetTarget(maxRange.transform.position, 7, true);
        invincible = false;
        AudioManager.Instance.PlaySFX(skills[2].sound);

    }

    private GameObject warriorSkill1;
    void WarriorSkill1()
    {// 스마이트
        if (warriorSkill1 == null)
            warriorSkill1 = Instantiate(skills[0].effect, firePos.transform.position, Quaternion.identity, firePos.transform);
        warriorSkill1.SetActive(true);
        warriorSkill1.transform.rotation = firePos.transform.rotation;
        warriorSkill1.GetComponent<Installation>().SetTarget(2);
        AudioManager.Instance.PlaySFX(skills[0].sound);
    }

    void WarriorSkill1End()
    {
        warriorSkill1.SetActive(false);
    }

    private GameObject warriorSkill2;
    void WarriorSkill2()
    {// 아머 크래시
        if (warriorSkill2 == null)
            warriorSkill2 = Instantiate(skills[1].effect, firePos.transform.position, Quaternion.identity);
        else
            warriorSkill2.transform.position = firePos.transform.position;
        warriorSkill2.transform.rotation = Quaternion.Euler(90, 0, 0);
        warriorSkill2.SetActive(true);
        warriorSkill2.GetComponent<Installation>().SetTarget(1, 3);
        AudioManager.Instance.PlaySFX(skills[1].sound);
        Invoke("WarriorSkill2End", 1f);
    }

    void WarriorSkill2End()
    {
        warriorSkill2.SetActive(false);
    }

    private GameObject warriorSkill3;
    void WarriorSkill3()
    {// 투지
        if (warriorSkill3 == null)
            warriorSkill3 = Instantiate(skills[2].effect, transform.position, Quaternion.identity, transform);
        warriorSkill3.transform.rotation = Quaternion.Euler(90, 0, 0);
        warriorSkill3.SetActive(true);
        buffStat.buffDamage = 10;
        buffStat.buffDefense = 10;
        AudioManager.Instance.PlaySFX(skills[2].sound);
        Invoke("WarriorSkill3End", 30f);
        invincible = false;
    }

    void WarriorSkill3End()
    {
        warriorSkill3.SetActive(false);
        buffStat.buffDamage = 0;
        buffStat.buffDefense = 0;
    }

    private GameObject wizardSkill1;
    void WizardSkill1()
    {// 썬더볼트
        if (wizardSkill1 == null)
            wizardSkill1 = Instantiate(skills[0].effect, firePos.transform.position, Quaternion.identity, firePos.transform);
        wizardSkill1.SetActive(true);
        wizardSkill1.transform.rotation = firePos.transform.rotation;
        wizardSkill1.GetComponent<Installation>().SetTarget(3);
        AudioManager.Instance.PlaySFX(skills[0].sound);
        Invoke("WizardSkill1End", 0.7f);
    }

    void WizardSkill1End()
    {
        wizardSkill1.SetActive(false);
    }

    private GameObject wizardSkill2;
    void WizardSkill2()
    {// 워터쉴드
        if (wizardSkill2 == null)
            wizardSkill2 = Instantiate(skills[1].effect, transform.position, Quaternion.identity, transform);
        wizardSkill2.transform.rotation = Quaternion.Euler(90, 0, 0);
        wizardSkill2.SetActive(true);
        invincible = true;
        AudioManager.Instance.PlaySFX(skills[1].sound);
        Invoke("WizardSkill2End", 5f);
    }

    void WizardSkill2End()
    {
        wizardSkill2.SetActive(false);
        invincible = false;
    }

    private GameObject wizardSkill3;
    void WizardSkill3()
    {// 익스플로전
        if (wizardSkill3 == null)
            wizardSkill3 = Instantiate(skills[2].effect, click.point + new Vector3(0, 0.2f, 0), Quaternion.Euler(-90, 0, 0));
        else
            wizardSkill3.transform.position = click.point + new Vector3(0, 0.2f, 0);
        wizardSkill3.SetActive(true);
        wizardSkill3.GetComponent<Installation>().SetTarget(5);
        invincible = false;
        AudioManager.Instance.PlaySFX(skills[2].sound);
        Invoke("WizardSkill3End", 1f);
    }

    void WizardSkill3End()
    {
        wizardSkill3.SetActive(false);
    }

    void Update()
    {
        GetInput(); // 사용자 입력 받기
        UpdateAttackInfo(); // 공격 쿨타임 체크
        switch (state)
        {
            case CharacterState.Idle:
                enemy = null;
                moveDir.SetActive(false);
                MoveStop();
                restTime += Time.deltaTime;
                if (restTime > 15)
                    Heal(10);
                break;
            case CharacterState.move:
                moveDir.SetActive(true);
                moveDir.transform.position = hit.point + new Vector3(0, 0.1f, 0);
                moveDir.transform.localScale = new Vector3(0.4f, 0.4f);
                MoveState(hit.point);
                break;
            case CharacterState.attack:
                if (enemy != null)
                {
                    moveDir.SetActive(true);
                    moveDir.transform.position = enemy.transform.position;
                    moveDir.transform.localScale = new Vector3(1f, 1f);
                }
                else
                    moveDir.SetActive(false);
                AttackState();
                break;
            case CharacterState.down:
                MoveStop();
                break;
            case CharacterState.death:
                break;
        }
    }
}
