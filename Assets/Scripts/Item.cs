using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int uid; // ���� �ڵ�
    public int type; // 0 - ��� ������, 1 - �Һ� ������
    public string name; // �̸�
    public string iconImg; // �̹��� ���
    public int classNum; // 0 - �ü�, 1 - ����, 2 - ������
    public int price; // ����
    public int damage; // ���ݷ�
    public int armor; // ����
    public int speed; // �̵��ӵ�
}
