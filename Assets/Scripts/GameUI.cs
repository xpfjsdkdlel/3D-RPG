using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject inventory;
    public GameObject equipment;
    public GameObject menu;
    [SerializeField]
    private GameObject dialogue;
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

    public void ActiveDialogue()
    {

    }

    public void CloseDialogue()
    {

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
