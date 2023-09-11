using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public int uid; // 고유 코드
    public bool equip; // 0 - 소비 아이템, 1 - 장비 아이템
    public string name; // 이름
    public Sprite iconImg; // 이미지
    public int classNum; // 0 - 궁수, 1 - 전사, 2 - 마법사, 3 - 공용
    public int price; // 가격
    public int count = 1; // 개수
    public int maxCount = 1; // 최대 개수
    public int stat; // 성능
}
