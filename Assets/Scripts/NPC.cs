using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCrole
{
    normal,
    quest,
    store,
}

public class NPC : MonoBehaviour
{
    private GameUI gameUI;
    public int uid;
    public string name;
    public string text;
    public NPCrole role = new NPCrole();

    private void Awake()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
    }
    public void talk()
    {
        gameUI.ActiveDialogue(this);
    }
}
