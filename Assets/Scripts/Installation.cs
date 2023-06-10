using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Installation : MonoBehaviour
{
    private int damage; // ���ݷ�
    [SerializeField]
    private float delay = 0; // ��ų�� ��������
    private float stunTime = 0; // ���� �ð�
    private CharacterController player;
    private Enemy enemy;
    private CapsuleCollider capsuleCollider;

    private void OnEnable()
    {
        player = GameObject.FindObjectOfType<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        if(delay > 0)
        {
            capsuleCollider.enabled = false;
            Invoke("endDelay", delay);
        }
    }

    private void endDelay()
    {
        capsuleCollider.enabled = true;
    }

    public void SetTarget(float scale = 1.0f, float stun = 0)
    {
        damage = (int)((player.damage + player.weapon + player.buffDamage) * scale);
        stunTime = stun;
    }

    private void OnTriggerEnter(Collider other)
    {// ���� �浹�� ������� ��
        if (other.CompareTag("Enemy"))
        {
            enemy = other.GetComponent<Enemy>();
            enemy.GetDamage(damage, stunTime);
        }
    }
}
