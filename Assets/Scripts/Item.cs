using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public int uid; // ���� �ڵ�
    public bool type; // 0 - �Һ� ������, 1 - ��� ������
    public string name; // �̸�
    public Sprite iconImg; // �̹���
    public int classNum; // 0 - �ü�, 1 - ����, 2 - ������, 3 - ����
    public int price; // ����
    public int count; // ����
    public int maxCount; // �ִ� ����
    public int damage; // ���ݷ�
    public int armor; // ����
    public int speed; // �̵��ӵ�
    public int range; // �����Ÿ�
    public int heal; // ȸ����

}
