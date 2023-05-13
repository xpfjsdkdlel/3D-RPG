using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Skill : ScriptableObject
{
    public string name; // 스킬 이름
    public string explan; // 스킬 설명
    public Sprite iconImg; // 스킬 이미지
    public float coolDown; // 스킬 쿨타임
    public int cost; // 마나 비용
    public GameObject effect; // 스킬 이펙트
    public bool active = true; // 사용 가능 여부
}
