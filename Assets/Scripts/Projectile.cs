using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject firePos;
    [SerializeField]
    private Vector3 targetPos;
    private int damage;
    Enemy enemy;
    private void OnEnable()
    {
        damage = GameObject.FindObjectOfType<CharacterController>().damage + GameObject.FindObjectOfType<CharacterController>().weapon;
        transform.position = GameObject.Find("FirePos").transform.position;
    }

    public void SetTarget(Vector3 pos, float scale = 1.0f)
    {// 이동할 경로 설정
        targetPos = pos + new Vector3(0, 0.5f, 0);
        damage = (int)((GameObject.FindObjectOfType<CharacterController>().damage + GameObject.FindObjectOfType<CharacterController>().weapon) * scale);
    }

    private void OnTriggerEnter(Collider other)
    {// 적과 충돌시 대미지를 줌
        if (other.CompareTag("Enemy"))
        {
            enemy = other.GetComponent<Enemy>();
            enemy.GetDamage(damage);
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
