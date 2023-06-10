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
    public int buffDamage = 0; // ���ݷ� ����
    public int defense; // ����
    public int armor; // �� ����
    public int buffDefense = 0; // ���� ����
    public float range; // �����Ÿ�
    public float addRange; // �߰� �����Ÿ�
    public float attackDelay; // ���� ������
    public float speed = 1.0f; // �̵��ӵ�
    public bool isDead = false; // ���� ����
    [SerializeField]
    private GameObject projectile; // ����ü
    public Skill[] skills = new Skill[3]; // ��ų
    [SerializeField]
    private GameObject maxRange;
    public GameObject firePos;

    public bool isControll = true; // ��� ������ ����
    private bool battle = false; // ���� ����
    public bool invincible = false; // ���� ����
    private float attackPrevTime = 0f; // ���������� ������ �ð�
    private float attackTime = 0f; // ���� �� �帥 �ð�
    private bool attackState = false; // false�� ������ ������ ����
    public Enemy enemy; // Ÿ���� ����
    private bool combo = false; // �ι�° ���� �ִϸ��̼� ��� ����

    private Animator animator;
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
        layerMask = (-1) - (1 << LayerMask.NameToLayer("Spawner")) - (1 << LayerMask.NameToLayer("Skill"));
    }
    
    void GetInput()
    {
        if (isControll && !isDead)
        {
            // RŰ�� ���� ���� ���¿� �޽� ���¸� ����
            if (Input.GetKeyDown(KeyCode.R))
                ChangeMode();
            if (battle)
            {// ���� ������ �� Q, W, E�� ���� ��ų�� ���
                RaycastHit click;
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
                // ȭ���� ��Ŭ�� ���� ��
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out click, Mathf.Infinity, layerMask))
                {
                    if (click.transform.gameObject.CompareTag("NPC"))
                    {
                        // NPC ��ȭ��
                        click.transform.GetComponent<NPC>().talk();
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
                enemy.GetDamage(damage + weapon + buffDamage);
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
        if (damage <= defense + armor + buffDefense)
            HP -= 1;
        else
            HP -= damage - (defense + armor + buffDefense);
        if (HP <= 0)
        {
            HP = 0;
            animator.SetTrigger("death");
            isDead = true;
            state = CharacterState.death;
            moveDir.SetActive(false);
            MoveStop();
        }
        //else
        //    animator.SetTrigger("hit");
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

    // ��ų ��Ÿ�� �ʱ�ȭ �ϴ� �Լ�
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
    {// ���� ��
        if (archerSkill1 == null)
            archerSkill1 = Instantiate(skills[0].effect, transform.position, Quaternion.identity);
        archerSkill1.SetActive(true);
        if (enemy != null)
            archerSkill1.GetComponent<Projectile>().SetTarget(enemy.transform.position, 2);
        else
            archerSkill1.GetComponent<Projectile>().SetTarget(maxRange.transform.position, 2);
    }

    private GameObject archerSkill2;
    void ArcherSkill2()
    {// �ַο� ����
        RaycastHit click;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out click, Mathf.Infinity, layerMask))
            if (archerSkill2 == null)
                archerSkill2 = Instantiate(skills[1].effect, click.point + new Vector3(0, 0.2f, 0), Quaternion.Euler(-90, 0, 0));
            else
                archerSkill2.transform.position = click.point + new Vector3(0, 0.2f, 0);
        archerSkill2.SetActive(true);
        archerSkill2.GetComponent<Installation>().SetTarget(3);
        Invoke("ArcherSkill2End", 2f);
    }

    void ArcherSkill2End()
    {
        archerSkill2.SetActive(false);
    }

    private GameObject archerSkill3;
    void ArcherSkill3()
    {// ��������
        if (archerSkill3 == null)
            archerSkill3 = Instantiate(skills[2].effect, transform.position, Quaternion.identity);
        archerSkill3.SetActive(true);
        if (enemy != null)
            archerSkill3.GetComponent<Projectile>().SetTarget(enemy.transform.position, 7, true);
        else
            archerSkill3.GetComponent<Projectile>().SetTarget(maxRange.transform.position, 7, true);
        invincible = false;

    }

    private GameObject warriorSkill1;
    void WarriorSkill1()
    {// ������Ʈ
        if (warriorSkill1 == null)
            warriorSkill1 = Instantiate(skills[0].effect, firePos.transform.position, Quaternion.identity, firePos.transform);
        warriorSkill1.SetActive(true);
        warriorSkill1.transform.rotation = firePos.transform.rotation;
        warriorSkill1.GetComponent<Installation>().SetTarget(2);
    }

    void WarriorSkill1End()
    {
        warriorSkill1.SetActive(false);
    }

    private GameObject warriorSkill2;
    void WarriorSkill2()
    {// �Ƹ� ũ����
        if (warriorSkill2 == null)
            warriorSkill2 = Instantiate(skills[1].effect, firePos.transform.position, Quaternion.identity);
        else
            warriorSkill2.transform.position = firePos.transform.position;
        warriorSkill2.transform.rotation = Quaternion.Euler(90, 0, 0);
        warriorSkill2.SetActive(true);
        warriorSkill2.GetComponent<Installation>().SetTarget(1, 3);
        Invoke("WarriorSkill2End", 1f);
    }

    void WarriorSkill2End()
    {
        warriorSkill2.SetActive(false);
    }

    private GameObject warriorSkill3;
    void WarriorSkill3()
    {// ����
        if (warriorSkill3 == null)
            warriorSkill3 = Instantiate(skills[2].effect, transform.position, Quaternion.identity, transform);
        warriorSkill3.transform.rotation = Quaternion.Euler(90, 0, 0);
        warriorSkill3.SetActive(true);
        buffDamage = 10;
        buffDefense = 10;
        Invoke("WarriorSkill3End", 30f);
        invincible = false;
    }

    void WarriorSkill3End()
    {
        warriorSkill3.SetActive(false);
        buffDamage = 0;
        buffDefense = 0;
    }

    private GameObject wizardSkill1;
    void WizardSkill1()
    {// �����Ʈ
        if (wizardSkill1 == null)
            wizardSkill1 = Instantiate(skills[0].effect, firePos.transform.position, Quaternion.identity, firePos.transform);
        wizardSkill1.SetActive(true);
        wizardSkill1.transform.rotation = firePos.transform.rotation;
        wizardSkill1.GetComponent<Installation>().SetTarget(3);
        Invoke("WizardSkill1End", 0.7f);
    }

    void WizardSkill1End()
    {
        wizardSkill1.SetActive(false);
    }

    private GameObject wizardSkill2;
    void WizardSkill2()
    {// ���ͽ���
        if (wizardSkill2 == null)
            wizardSkill2 = Instantiate(skills[1].effect, transform.position, Quaternion.identity, transform);
        wizardSkill2.transform.rotation = Quaternion.Euler(90, 0, 0);
        wizardSkill2.SetActive(true);
        invincible = true;
        Invoke("WizardSkill2End", 5f);
    }

    void WizardSkill2End()
    {
        wizardSkill2.SetActive(false);
        invincible = false;
    }

    private GameObject wizardSkill3;
    void WizardSkill3()
    {// �ͽ��÷���
        RaycastHit click;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out click, Mathf.Infinity, layerMask))
            if (wizardSkill3 == null)
                wizardSkill3 = Instantiate(skills[2].effect, click.point + new Vector3(0, 0.2f, 0), Quaternion.Euler(-90, 0, 0));
            else
                wizardSkill3.transform.position = click.point + new Vector3(0, 0.2f, 0);
        wizardSkill3.SetActive(true);
        wizardSkill3.GetComponent<Installation>().SetTarget(5);
        invincible = false;
        Invoke("WizardSkill3End", 1f);
    }

    void WizardSkill3End()
    {
        wizardSkill3.SetActive(false);
    }

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
