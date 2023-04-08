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
    public string name; // �̸�
    public int level = 1; // ����
    public int HP; // ���� ü��
    public int maxHP; // �ִ� ü��
    public int MP; // ���� ����
    public int maxMP; // �ִ� ����
    public int EXP; // ����ġ
    public int gold; // ��
    public int damage; // ���ݷ�
    public int weapon; // ���� ���ݷ�
    public int defense; // ����
    public int armor; // �� ����
    public float range; // �����Ÿ�
    public float addRange; // �߰� �����Ÿ�
    public float attackDelay; // ���� ������
    public float speed = 1.0f; // �̵��ӵ�
    public bool isDead = false; // ���� ����
    [SerializeField]
    private GameObject projectile; // ����ü

    private bool isControll = true; // ��� ������ ����
    private bool battle = false; // ���� ����
    private bool invincible = false; // ���� ����
    private float attackPrevTime = 0f; // ���������� ������ �ð�
    private float attackTime = 0f; // ���� �� �帥 �ð�
    private bool attackState = false; // false�� ������ ������ ����
    public Enemy enemy; // Ÿ���� ����
    private bool combo = false; // �ι�° ���� �ִϸ��̼� ��� ����

    private Animator animator;
    private Collider collider;
    private NavMeshAgent navMesh;

    private RaycastHit hit; // ��Ŭ�� �� �̵� �� ���
    private int layerMask;
    [SerializeField]
    private GameObject moveDir; // �̵� �� ��ġ ǥ��

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
            // RŰ�� ���� ���� ���¿� �޽� ���¸� ����
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
                // ȭ���� ��Ŭ�� ���� ��
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.transform.gameObject.CompareTag("NPC"))
                    {

                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                // ȭ���� ��Ŭ�� ���� ��
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,Mathf.Infinity,layerMask))
                {
                    if (hit.transform.gameObject.CompareTag("Enemy"))
                    {
                        // ���Ͷ�� ���� ó��
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
            // �ӽ� �ڵ�(���� ����)
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
            animator.SetBool("battle", true); // ���� ������
            state = CharacterState.Idle;
        }
        else
        {
            animator.SetBool("battle", false); // ���� ����ֱ�
            state = CharacterState.Idle;
        }
    }

    void MoveStop()
    {// ���� ����
        navMesh.ResetPath();
        navMesh.velocity = Vector3.zero;
        animator.SetBool("isWalk", false);
    }

    void AttackState()
    {// ���� ����
        if (enemy != null)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range)
            {
                MoveStop();
                if (battle && !attackState)
                {
                    transform.LookAt(enemy.transform.position); // ���� �ٶ�
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
    {// ���� �ִϸ��̼�
        if (!enemy.isDead)
        {
            animator.SetTrigger("attack");
            animator.SetBool("combo", combo);
            combo = !combo; // ���� ������ �ִϸ��̼� ����
            attackPrevTime = Time.time; // ������ �ð� ����
            attackState = true;
        }
        else
            enemy = null;
    }

    void Attack()
    {// ������ ������� ������ �Լ�
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
    {// ������ ������� �޴� �Լ�
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
        // ������ �� ���¶��, ������ �ð��� �����ڿ� �ٽ� ������ �� �ֵ��� ����
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
        // �̵� �ִϸ��̼�
        animator.SetBool("isWalk", true);
        // �������� �����ߴٸ� �����·� ����
        if (navMesh.velocity.sqrMagnitude >= 0.2f * 0.2f && navMesh.remainingDistance <= 0.1f)
            state = CharacterState.Idle;
        else if (moveDir.x == transform.position.x && moveDir.z == transform.position.z)
            state = CharacterState.Idle;
    }

    public void GetEXP(int EXP)
    {
        if(this.EXP + EXP >= level * 10)
        {// ������
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
        GetInput(); // ����� �Է� �ޱ�
        UpdateAttackInfo(); // ���� ��Ÿ�� üũ
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
