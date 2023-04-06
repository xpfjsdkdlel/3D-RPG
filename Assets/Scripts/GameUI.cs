using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject inventory;
    public GameObject equipment;
    public Transform slotHolder;
    public GameObject menu;
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

    public void CloseTab()
    {
        invenActive = false;
        inventory.SetActive(false);
        equipActive = false;
        equipment.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            invenActive = !invenActive;
            inventory.SetActive(invenActive);
            inventory.GetComponent<Inventory>().RefreshSlot();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            equipActive = !equipActive;
            equipment.SetActive(equipActive);
            equipment.GetComponent<Equipment>().RefreshSlot();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTab();
            menuActive = !menuActive;
            menu.SetActive(menuActive);
        }
    }
}
