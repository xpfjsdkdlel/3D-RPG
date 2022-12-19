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
    public int HP; // 현재 체력
    public int maxHP; // 최대 체력
    public int MP; // 현재 마나
    public int maxMP; // 최대 마나
    public int EXP; // 경험치
    public int damage; // 공격력
    public int armor; // 방어력
    public float range; // 사정거리
    public float attackDelay; // 공격 딜레이
    public float speed = 1.0f; // 이동속도

    private bool isControll = true; // 제어가 가능한 상태
    private bool battle = false; // 전투 상태
    private float attackPrevTime = 0f; // 마지막으로 공격한 시간
    private float attackTime = 0f; // 공격 후 흐른 시간
    private bool attackState = false; // false면 공격이 가능한 상태
    public GameObject target; // 공격 할 타겟
    private Enemy enemy; // 타겟의 정보
    private bool combo = false; // 두번째 공격 애니메이션 출력 여부

    private Animator animator;
    private NavMeshAgent navMesh;

    [SerializeField]
    private GameObject moveDir; // 이동 할 위치 표시

    [SerializeField]
    private CharacterState state = new CharacterState();

    private RaycastHit hit; // 이동 할 장소

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
            // R키를 눌러 전투 상태와 휴식 상태를 변경
            if (Input.GetKeyDown(KeyCode.R))
                ChangeMode();
            if (Input.GetMouseButtonDown(0))
            {
                
            }
            if (Input.GetMouseButtonDown(1))
            {
                // 화면을 우클릭 했을 때
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.transform.gameObject.CompareTag("Enemy"))
                    {
                        // 공격 처리
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
            // 임시 코드(추후 수정)
            state = CharacterState.Idle;
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
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= range)
            {
                MoveStop();
                if (battle && !attackState)
                {
                    transform.LookAt(target.transform.position); // 적을 바라봄
                    // 공격 함수 호출
                    attackPrevTime = Time.time; // 공격한 시간 갱신
                    attackState = true;
                    animator.SetTrigger("attack");
                    animator.SetBool("combo", combo);
                    combo = !combo; // 다음 공격의 애니메이션 변경
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
    {// 적에게 대미지를 입히는 함수
        if(target != null)
        {
            enemy = target.GetComponent<Enemy>();
            enemy.GetDamage(damage);
        }
    }

    public void GetDamage(int damage)
    {// 적에게 대미지를 받는 함수
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
    }

    void Update()
    {
        GetInput(); // 사용자 입력 받기
        UpdateAttackInfo(); // 공격 쿨타임 체크
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
