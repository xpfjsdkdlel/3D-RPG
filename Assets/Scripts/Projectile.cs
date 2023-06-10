using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject firePos; // 생성될 좌표
    [SerializeField]
    private Vector3 targetPos; // 이동할 좌표
    private int damage; // 공격력
    private bool penetrate = false; // 관통 여부
    private CharacterController player;
    private Enemy enemy;

    private void OnEnable()
    {
        player = GameObject.FindObjectOfType<CharacterController>();
        damage = player.damage + player.weapon + player.buffDamage;
        transform.position = player.firePos.transform.position;
    }

    public void SetTarget(Vector3 pos, float scale = 1.0f, bool penet = false)
    {// 이동할 경로 설정
        targetPos = pos + new Vector3(0, 0.5f, 0);
        damage = (int)((player.damage + player.weapon + player.buffDamage) * scale);
        penetrate = penet;
    }

    private void OnTriggerEnter(Collider other)
    {// 적과 충돌시 대미지를 줌
        if (other.CompareTag("Enemy"))
        {
            enemy = other.GetComponent<Enemy>();
            enemy.GetDamage(damage);
            if(!penetrate)
                gameObject.SetActive(false);
        }
        else if (other.CompareTag("Destroy"))
            gameObject.SetActive(false);
    }

    void Update()
    {
        transform.LookAt(targetPos);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.2f);
        if (transform.position == targetPos)
            gameObject.SetActive(false);
    }
}
