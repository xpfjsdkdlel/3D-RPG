using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    public int damage;
    private Animation animation;
    private SphereCollider collider;
    [SerializeField]
    private AudioClip audioClip;

    private void Awake()
    {
        animation = GetComponent<Animation>();
        collider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        animation.Play();
    }

    private void SkillSound()
    {
        AudioManager.Instance.PlaySFX(audioClip);
    }

    private void EndSkill()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().GetDamage(damage);
        }
    }
}
