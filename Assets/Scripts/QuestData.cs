using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestData : ScriptableObject
{
    public int questId; // 퀘스트 id
    public int targetId; // 목표물의 uid
    public string questName; // 퀘스트 이름
    public string questInfo; // 퀘스트 내용
    public int startLevel; // 퀘스트 시작 레벨
    public string[] text; // 퀘스트 대화문
    public int progress = 0; // 진행도
    public int complete; // 목표
    public int result; // 보상
    public string[] progText; // 퀘스트를 진행중일 때 NPC가 출력할 대사
    public string[] clearText; // 퀘스트 완료시 NPC가 출력할 대사
    public bool isProgress = false; // 수행 여부
    public bool isClear = false; // 클리어 여부
}
