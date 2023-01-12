using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public int uid; // 고유 코드
    public bool type; // 0 - 장비 아이템, 1 - 소비 아이템
    public string name; // 이름
    public Sprite iconImg; // 이미지 경로
    public int classNum; // 0 - 궁수, 1 - 전사, 2 - 마법사, 3 - 공용
    public int price; // 가격
}
