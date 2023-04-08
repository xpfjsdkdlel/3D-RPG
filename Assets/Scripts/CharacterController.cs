using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterState
{
    Idle,
    move,
    attack,
    death,
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
    public int gold; // 돈
    public int damage; // 공격력
    public int weapon; // 무기 공격력
    public int defense; // 방어력
    public int armor; // 방어구 방어력
    public float range; // 사정거리
    public float addRange; // 추가 사정거리
    public float attackDelay; // 공격 딜레이
    public float speed = 1.0f; // 이동속도
    public bool isDead = false; // 생존 여부
    [SerializeField]
    private GameObject projectile; // 투사체

    private bool isControll = true; // 제어가 가능한 상태
    private bool battle = false; // 전투 상태
    private bool invincible = false; // 무적 상태
    private float attackPrevTime = 0f; // 마지막으로 공격한 시간
    private float attackTime = 0f; // 공격 후 흐른 시간
    private bool attackState = false; // false면 공격이 가능한 상태
    public Enemy enemy; // 타겟의 정보
    private bool combo = false; // 두번째 공격 애니메이션 출력 여부

    private Animator animator;
    private Collider collider;
    private NavMeshAgent navMesh;

    private RaycastHit hit; // 우클릭 시 이동 할 장소
    private int layerMask;
    [SerializeField]
    private GameObject moveDir; // 이동 할 위치 표시

    [SerializeField]
    private CharacterState state = new CharacterState();

    private GameSceneManager sceneManager;

    public void Init()
    {
        HP = maxHP;
        MP = maxMP;
        animator = GetComponent<Animator>();
        isControll = true;
        state = CharacterState.Idle;
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = speed;
        sceneManager = GameObject.FindObjectOfType<GameSceneManager>();
        layerMask = (-1) - (1 << LayerMask.NameToLayer("Spawner"));
    }
    
    void GetInput()
    {
        if (isControll && !isDead)
        {
            // R키를 눌러 전투 상태와 휴식 상태를 변경
            if (Input.GetKeyDown(KeyCode.R))
                ChangeMode();
            //if(battle)
            //{
            //    if(Input.GetKeyDown(KeyCode.Q))
            //    {
            //        Skill1(damage);
            //    }
            //    else if (Input.GetKeyDown(KeyCode.W))
            //    {
            //        Skill2(damage);
            //    }
            if (Input.GetMouseButtonDown(0))
            {
                // 화면을 좌클릭 했을 때
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.transform.gameObject.CompareTag("NPC"))
                    {

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
                        // 몬스터라면 공격 처리
                        enemy = hit.transform.gameObject.GetComponent<Enemy>();
                        state = CharacterState.attack;
                    }
                    else
                    {
                        enemy = null;
                        state = CharacterState.move;
                    }
                }
            }
        }
        else if(!isControll && !isDead)
        {
            // 임시 코드(추후 수정)
            state = CharacterState.Idle;
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
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range)
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
    }

    void Attack()
    {// 적에게 대미지를 입히는 함수
        if (!enemy.isDead)
        {
            if(projectile == null)
                enemy.GetDamage(damage + weapon);
        }
    }

    void ProjectileAttack()
    {
        projectile.transform.parent = null;
        projectile.SetActive(true);
        projectile.GetComponent<Projectile>().SetTarget(enemy.transform.position);
    }

    public void GetDamage(int damage)
    {// 적에게 대미지를 받는 함수
        if (damage <= defense + armor)
            HP -= 1;
        else
            HP -= damage - (defense + armor);
        if (HP <= 0)
        {
            HP = 0;
            animator.SetTrigger("death");
            isDead = true;
            state = CharacterState.death;
            moveDir.SetActive(false);
            MoveStop();
        }
        else
            animator.SetTrigger("hit");
        sceneManager.Refresh();
    }

    public void Refresh()
    {
        sceneManager.Refresh();
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
            state = CharacterState.Idle;
        else if (moveDir.x == transform.position.x && moveDir.z == transform.position.z)
            state = CharacterState.Idle;
    }

    public void GetEXP(int EXP)
    {
        if(this.EXP + EXP >= level * 10)
        {// 레벨업
            this.EXP = (this.EXP + EXP) - (level * 10); 
            ++level;
            sceneManager.LevelUp();
        }
        else
            this.EXP += EXP;
        sceneManager.Refresh();
    }

    //public virtual void Skill1(int damage)
    //{
    //    animator.SetBool("skill1", true);
    //}

    //public virtual void Skill2(int damage)
    //{
    //    animator.SetBool("skill2", true);
    //}

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
                sceneManager.CloseHP();
                break;
            case CharacterState.move:
                moveDir.SetActive(true);
                moveDir.transform.position = hit.point + new Vector3(0, 0.1f, 0);
                MoveState(hit.point);
                sceneManager.CloseHP();
                break;
            case CharacterState.attack:
                moveDir.SetActive(false);
                AttackState();
                sceneManager.ViewHP(enemy);
                break;
            case CharacterState.death:
                break;
        }
    }
}
