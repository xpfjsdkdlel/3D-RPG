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
    private GameObject target; // 공격 할 타겟

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
                        // 이동 처리
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
            animator.SetBool("battle", true); // 무기 꺼내기
            state = CharacterState.Idle;
        }
        else
        {
            animator.SetBool("battle", false); // 무기 집어넣기
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
                    // 공격 함수 호출
                    attackPrevTime = Time.time; // 공격한 시간 갱신
                }
            }
            else
                state = CharacterState.Idle;

        }
        else
        {
            animator.SetBool("attack", false);
            navMesh.destination = target.transform.position;
            // 이동 애니메이션
            animator.SetBool("isWalk", true);
        }
    }

    void UpdateAttackInfo()
    {
        // 공격을 한 상태라면, 일정한 시간이 지난뒤에 다시 공격할 수 있도록 변경
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
        // 이동 애니메이션
        animator.SetBool("isWalk", true);
        // 목적지에 도착했다면 대기상태로 변경
        if (navMesh.velocity.sqrMagnitude >= 0.2f * 0.2f && navMesh.remainingDistance <= 0.1f)
        {
            state = CharacterState.Idle;
        }
    }

    void Update()
    {
        GetInput(); // 사용자 입력 받기
        UpdateAttackInfo(); // 공격 쿨타임 체크
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
