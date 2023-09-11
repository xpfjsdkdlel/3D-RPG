using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public int uid; // ���� �ڵ�
    public bool equip; // 0 - �Һ� ������, 1 - ��� ������
    public string name; // �̸�
    public Sprite iconImg; // �̹���
    public int classNum; // 0 - �ü�, 1 - ����, 2 - ������, 3 - ����
    public int price; // ����
    public int count = 1; // ����
    public int maxCount = 1; // �ִ� ����
    public int stat; // ����
}
