using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject firePos;
    private Vector3 targetPos;
    private int damage;
    Enemy enemy;
    private void OnEnable()
    {
        Init();
    }

    void Init()
    {
        damage = GameObject.FindObjectOfType<CharacterController>().damage;
        transform.position = firePos.transform.position;
    }

    public void SetTarget(Vector3 pos)
    {
        targetPos = pos + new Vector3(0, 0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemy = other.GetComponent<Enemy>();
            enemy.GetDamage(damage);
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        transform.LookAt(targetPos);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.2f);
    }
}
