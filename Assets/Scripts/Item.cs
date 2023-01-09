using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int uid; // 고유 코드
    public int type; // 0 - 장비 아이템, 1 - 소비 아이템
    public string name; // 이름
    public string iconImg; // 이미지 경로
    public int classNum; // 0 - 궁수, 1 - 전사, 2 - 마법사
    public int price; // 가격
    public int damage; // 공격력
    public int armor; // 방어력
    public int speed; // 이동속도
}
