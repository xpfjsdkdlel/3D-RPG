using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public GameObject inventory;
    public GameObject equipment;
    public GameObject menu;
    [SerializeField]
    private GameObject dialogue;
    private TextMeshProUGUI npcName;
    private TextMeshProUGUI Dtext;
    private string[] npcText;
    private NPCrole npcRole;
    private int n = 0;
    [SerializeField]
    private GameObject store;
    [SerializeField]
    private Slider sliderBGM;
    [SerializeField]
    private Slider sliderSFX;
    private bool invenActive = false;
    private bool equipActive = false;
    private bool menuActive = false;

    private void Start()
    {
        sliderBGM.value = AudioManager.Instance.BGMVolume;
        sliderSFX.value = AudioManager.Instance.SFXVolume;
        inventory.SetActive(invenActive);
        equipment.SetActive(equipActive);
        menu.SetActive(menuActive);
        npcName = dialogue.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Dtext = dialogue.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetBGMVolume()
    {
        AudioManager.Instance.SetBGMVolume(sliderBGM.value);
    }

    public void SetSFXVolume()
    {
        AudioManager.Instance.SetSFXVolume(sliderSFX.value);
    }

    public void CloseInventory()
    {
        invenActive = false;
        inventory.SetActive(false);
    }

    public void CloseEquipment()
    {
        equipActive = false;
        equipment.SetActive(false);
    }

    public void CloseStore()
    {
        store.SetActive(false);
    }

    public void ActiveDialogue(string name, string[] text, NPCrole role)
    {
        n = 0;
        dialogue.SetActive(true);
        npcName.text = name;
        Dtext.text = text[0];
        npcText = text;
        npcRole = role;
    }

    public void NextDialogue()
    {
        if(n < npcText.Length - 1)
        {
            ++n;
            Dtext.text = npcText[n];
        }
        else
        {
            switch (npcRole)
            {
                case NPCrole.nomal:
                    CloseDialogue();
                    break;
                case NPCrole.store:
                    store.SetActive(true);
                    CloseDialogue();
                    break;
                case NPCrole.quest:
                    // Äù½ºÆ® ÁÖ±â
                    break;
            }
        }
    }

    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }

    private void CloseTab()
    {
        invenActive = false;
        inventory.SetActive(false);
        equipActive = false;
        equipment.SetActive(false);
        dialogue.SetActive(false);
        store.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !dialogue.activeSelf && !store.activeSelf)
        {
            invenActive = !invenActive;
            inventory.SetActive(invenActive);
            inventory.GetComponent<Inventory>().RefreshSlot();
        }
        if (Input.GetKeyDown(KeyCode.E) && !dialogue.activeSelf && !store.activeSelf)
        {
            equipActive = !equipActive;
            equipment.SetActive(equipActive);
            equipment.GetComponent<Equipment>().RefreshSlot();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(invenActive || equipActive || dialogue.activeSelf || store.activeSelf)
                CloseTab();
            else
            {
                menuActive = !menuActive;
                menu.SetActive(menuActive);
            }
        }
    }
}
