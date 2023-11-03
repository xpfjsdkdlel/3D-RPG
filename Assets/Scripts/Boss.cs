using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private bool isCoolDown = true;
    private float coolDown = 10;
    private float prevTime = 0;
    private int skillNum = 0; // ������ ������ ��ų ��ȣ
    private Vector3 rushPosition = new Vector3(0, 0, 0);

    void bossSkill1()
    {// ���� ����
        rushPosition = enemy.transform.position;
        transform.LookAt(rushPosition);
        navMesh.ResetPath();
        navMesh.velocity = Vector3.zero;
        animator.SetBool("isWalk", false);
    }

    void Charge()
    {
        navMesh.SetDestination(rushPosition);
        animator.SetBool("isWalk", true);
        state = MonsterState.skill1;
        bodyAttack = true;
    }

    void bossSkill2()
    {// ���� ��ȯ

    }
    void bossSkill3()
    {// ȸ�� ����
        sceneManager.Refresh();
    }

    private void Update()
    {
        if (isCoolDown & coolDown < Time.time - prevTime)
        {
            isCoolDown = false;
        }
        switch (state)
        {
            case MonsterState.Idle:
                navMesh.ResetPath();
                navMesh.velocity = Vector3.zero;
                animator.SetBool("isWalk", false);
                break;
            case MonsterState.chase:
                if (!isCoolDown)
                {
                    if (skillNum >= 3)
                        skillNum = 0;
                    switch (skillNum)
                    {
                        case 0:
                            animator.SetTrigger("Skill1");
                            break;
                        case 1:
                            bossSkill2();
                            break;
                        case 2:
                            bossSkill3();
                            break;
                    }
                    isCoolDown = true;
                    prevTime = Time.time;
                }
                else if (!attackState)
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
                break;
            case MonsterState.skill1:
                if (Vector3.Distance(transform.position, rushPosition) < 1)
                {
                    bodyAttack = false;
                    animator.SetBool("isWalk", false);
                }
                break;
        }
    }
}
