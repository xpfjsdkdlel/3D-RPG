using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public int uid; // ���� �ڵ�
    public bool type; // 0 - ��� ������, 1 - �Һ� ������
    public string name; // �̸�
    public Sprite iconImg; // �̹��� ���
    public int classNum; // 0 - �ü�, 1 - ����, 2 - ������, 3 - ����
    public int price; // ����
}
