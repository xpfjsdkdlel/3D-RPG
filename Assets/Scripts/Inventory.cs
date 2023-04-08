using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private TextMeshProUGUI goldText;
    public Slot[] slots; // ui로 나타낼 슬롯 리스트
    private GameSceneManager gameScene;
    [SerializeField]
    private CharacterController player;
    [SerializeField]
    private Equipment equipment;

    private void Awake()
    {
        slots = new Slot[GameManager.Instance.row * 3];
        for (int i = 0; i < slots.Length; i++)
        {
            GameObject obj = Instantiate(slotPrefab);
            obj.transform.parent = slotParent;
            slots[i] = obj.GetComponent<Slot>();
            slots[i].slotNumber = i;
        }
        gameScene = GameObject.FindObjectOfType<GameSceneManager>();
    }

    public void RefreshSlot()
    {// 각 슬롯 새로고침
        for (int i = 0; i < GameManager.Instance.items.Length; i++)
        {
            if(GameManager.Instance.items[i] != null)
                slots[i].item = GameManager.Instance.items[i];
            slots[i].Refresh();
        }
        goldText.text = GameManager.Instance.gold.ToString();
    }

    public bool GetItem(Item _item)
    {
        for (int i = 0; i < GameManager.Instance.items.Length; i++)
        {
            if (GameManager.Instance.items[i] == null)
            {
                GameManager.Instance.items[i] = _item;
                slots[i].Refresh();
                return true;
            }
            else if (GameManager.Instance.items[i].uid == _item.uid)
            {
                if (!GameManager.Instance.items[i].equip && GameManager.Instance.items[i].count < 99)
                {
                    ++GameManager.Instance.items[i].count;
                    slots[i].Refresh();
                    return true;
                }
            }
        }
        return false;
    }

    public void UseItem(int num)
    {// 아이템 사용
        player = gameScene.player;
        Item _item = slots[num].item;
        if (_item.equip)
        {// 장비일 경우
            if (_item.classNum == 3)
            {// 방어구 착용
                switch (_item.uid)
                {
                    case 201:
                        if(GameManager.Instance.equip[0] != null)
                        {
                            Item prevItem = GameManager.Instance.equip[0];
                            GameManager.Instance.equip[0] = _item;
                            GameManager.Instance.items[num] = prevItem;
                            slots[num].item = prevItem;
                            Debug.Log(prevItem.name + "와(과) " + _item.name + "을(를) 교체");
                        }
                        else
                        {
                            GameManager.Instance.equip[0] = _item;
                            GameManager.Instance.items[num] = null;
                            slots[num].item = null;
                            Debug.Log(_item.name + " 착용");
                        }
                        break;
                    case 202:
                        if (GameManager.Instance.equip[1] != null)
                        {
                            Item prevItem = GameManager.Instance.equip[1];
                            GameManager.Instance.equip[1] = _item;
                            GameManager.Instance.items[num] = prevItem;
                            slots[num].item = prevItem;
                            Debug.Log(prevItem.name + "와(과) " + _item.name + "을(를) 교체");
                        }
                        else
                        {
                            GameManager.Instance.equip[1] = _item;
                            GameManager.Instance.items[num] = null;
                            slots[num].item = null;
                            Debug.Log(_item.name + " 착용");
                        }
                        break;
                    case 203:
                        if (GameManager.Instance.equip[2] != null)
                        {
                            Item prevItem = GameManager.Instance.equip[2];
                            GameManager.Instance.equip[2] = _item;
                            GameManager.Instance.items[num] = prevItem;
                            slots[num].item = prevItem;
                            Debug.Log(prevItem.name + "와(과) " + _item.name + "을(를) 교체");
                        }
                        else
                        {
                            GameManager.Instance.equip[2] = _item;
                            GameManager.Instance.items[num] = null;
                            slots[num].item = null;
                            Debug.Log(_item.name + " 착용");
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (GameManager.Instance.number == _item.classNum)
            {
                if(_item.uid < 200)
                {// 무기 착용
                    if (GameManager.Instance.equip[3] != null)
                    {
                        Item prevItem = GameManager.Instance.equip[3];
                        GameManager.Instance.equip[3] = _item;
                        GameManager.Instance.items[num] = prevItem;
                        slots[num].item = prevItem;
                        Debug.Log(prevItem.name + "와(과) " + _item.name + "을(를) 교체");
                    }
                    else
                    {
                        GameManager.Instance.equip[3] = _item;
                        GameManager.Instance.items[num] = null;
                        slots[num].item = null;
                        Debug.Log(_item.name + " 착용");
                    }
                }
                else
                {// 보조무기 착용
                    if (GameManager.Instance.equip[4] != null)
                    {
                        Item prevItem = GameManager.Instance.equip[4];
                        GameManager.Instance.equip[4] = _item;
                        GameManager.Instance.items[num] = prevItem;
                        slots[num].item = prevItem;
                        Debug.Log(prevItem.name + "와(과) " + _item.name + "을(를) 교체");
                    }
                    else
                    {
                        GameManager.Instance.equip[4] = _item;
                        GameManager.Instance.items[num] = null;
                        slots[num].item = null;
                        Debug.Log(_item.name + " 착용");
                    }
                }
            }
            else
            {
                Debug.Log("착용할 수 없는 장비입니다.");
            }
            if(equipment.gameObject.activeSelf)
                equipment.RefreshSlot();
        }
        else
        {// 물약일 경우
            switch (_item.uid)
            {
                case 401:
                    if (player.maxHP <= player.HP + _item.stat)
                    {
                        player.HP = player.maxHP;
                        --_item.count;
                    }
                    else
                        player.HP += _item.stat;
                    Debug.Log("체력포션 사용");
                    break;
                case 402:
                    if (player.maxMP <= player.MP + _item.stat)
                    {
                        player.MP = player.maxMP;
                        --_item.count;
                    }
                    else
                        player.MP += _item.stat;
                    Debug.Log("마나포션 사용");
                    break;
                default:
                    break;
            }
            if (_item.count <= 0)
                slots[num].item = null;
            player.Refresh();
        }
        gameScene.RefreshStat();
        slots[num].Refresh();
    }
}
