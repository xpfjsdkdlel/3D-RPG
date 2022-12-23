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
    public static int level = 1; // 레벨
    public static int HP; // 현재 체력
    public static int maxHP; // 최대 체력
    public static int MP; // 현재 마나
    public static int maxMP; // 최대 마나
    public static int EXP; // 경험치
}
