using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NextScene
{
    TitleScene,
    CharacterSelectScene,
    GameScene,
}
public static class GameData
{
    public static NextScene scene = NextScene.TitleScene;
    public static int level = 1; // ����
    public static int HP; // ���� ü��
    public static int maxHP; // �ִ� ü��
    public static int MP; // ���� ����
    public static int maxMP; // �ִ� ����
    public static int EXP; // ����ġ
}
