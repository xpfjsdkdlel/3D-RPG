using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Skill : ScriptableObject
{
    public string name; // ��ų �̸�
    public string explan; // ��ų ����
    public Sprite iconImg; // ��ų �̹���
    public float coolDown; // ��ų ��Ÿ��
    public int cost; // ���� ���
    public GameObject effect; // ��ų ����Ʈ
    public bool active = true; // ��� ���� ����
}
