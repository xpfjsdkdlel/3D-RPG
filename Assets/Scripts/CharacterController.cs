using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int HP; // 현재 체력
    public int maxHP; // 최대 체력
    public int MP; // 현재 마나
    public int maxMP; // 최대 마나
    public int EXP; // 경험치
    public int damage; // 공격력
    public int armor; // 방어력
    public float Range; // 사정거리
    public float speed = 1.0f; // 이동속도

    private Vector3 move = Vector3.zero;
    private Animator animator;
    private bool isControll = true; // 제어가 가능한 상태
    private bool battle = false; // 전투 상태
    private float attackTime = 0f; // 공격 후 흐른 시간
    private bool isAttack = false; // 공격 중이면 true, 대기 중이면 false

    private Rigidbody rb;

    void Start()
    {
        HP = maxHP;
        MP = maxMP;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        isControll = true;
    }

    void GetInput()
    {
        if (isControll)
        {
            // R키를 눌러 전투 상태와 휴식 상태를 변경
            if (Input.GetKeyDown(KeyCode.R))
                ChangeMode();
            if (Input.GetMouseButtonDown(0) && battle)
            {
                animator.SetTrigger("attack");
                Attack();
            }
            move.x = Input.GetAxis("Horizontal");
            move.z = Input.GetAxis("Vertical");
        }
        else
        {
            move = Vector3.zero;
            isAttack = false;
        }
    }


    void ChangeMode()
    {
        battle = !battle;
        if (battle)
        {
            animator.SetBool("battle", true);

        }
        else
            animator.SetBool("battle", false);
    }

    void Locomotion()
    {
        move.Normalize();
        // 이동 처리
        if (isAttack)
            move = Vector3.zero;
        transform.position += move * speed * Time.deltaTime;
        // 이동 애니메이션
        transform.LookAt(transform.position + move);
        animator.SetBool("isWalk", move != Vector3.zero);
    }

    void Attack()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Locomotion();
    }
}
