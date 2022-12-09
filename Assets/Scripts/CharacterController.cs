using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int HP; // ���� ü��
    public int maxHP; // �ִ� ü��
    public int MP; // ���� ����
    public int maxMP; // �ִ� ����
    public int EXP; // ����ġ
    public int damage; // ���ݷ�
    public int armor; // ����
    public float Range; // �����Ÿ�
    public float speed = 1.0f; // �̵��ӵ�

    private Vector3 move = Vector3.zero;
    private Animator animator;
    private bool isControll = true; // ��� ������ ����
    private bool battle = false; // ���� ����
    private float attackTime = 0f; // ���� �� �帥 �ð�
    private bool isAttack = false; // ���� ���̸� true, ��� ���̸� false

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
            // RŰ�� ���� ���� ���¿� �޽� ���¸� ����
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
        // �̵� ó��
        if (isAttack)
            move = Vector3.zero;
        transform.position += move * speed * Time.deltaTime;
        // �̵� �ִϸ��̼�
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
