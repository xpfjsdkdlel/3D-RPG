using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestData : ScriptableObject
{
    public int questId; // ����Ʈ id
    public int targetId; // ��ǥ���� uid
    public string questName; // ����Ʈ �̸�
    public string questInfo; // ����Ʈ ����
    public int startLevel; // ����Ʈ ���� ����
    public string[] text; // ����Ʈ ��ȭ��
    public int progress = 0; // ���൵
    public int complete; // ��ǥ
    public int result; // ����
    public string[] progText; // ����Ʈ�� �������� �� NPC�� ����� ���
    public string[] clearText; // ����Ʈ �Ϸ�� NPC�� ����� ���
    public bool isProgress = false; // ���� ����
    public bool isClear = false; // Ŭ���� ����
}
