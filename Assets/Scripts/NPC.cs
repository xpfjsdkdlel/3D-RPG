using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCrole
{
    nomal,
    store,
    quest,
}

public class NPC : MonoBehaviour
{
    private GameUI gameUI;
    public string name;
    public string[] text;
    [SerializeField]
    private NPCrole role = new NPCrole();

    private void Awake()
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
    }
    public void talk()
    {
        gameUI.ActiveDialogue(name, text, role);
    }
}
