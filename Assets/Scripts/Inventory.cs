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
    [SerializeField]
    private AudioClip equip;
    [SerializeField]
    private AudioClip potion;
    [SerializeField]
    private AudioClip error;
    public Store store;
    private Item tempItem;

    public void Init()
    {
        slots = new Slot[15];
        for (int i = 0; i < slots.Length; i++)
        {
            GameObject obj = Instantiate(slotPrefab);
            obj.transform.parent = slotParent;
            slots[i] = obj.GetComponent<Slot>();
            slots[i].slotNumber = i;
        }
        gameScene = GameObject.FindObjectOfType<GameSceneManager>();
        store = GameObject.FindObjectOfType<Store>();
    }

    public void RefreshSlot()
    {// 각 슬롯 새로고침
        for (int i = 0; i < GameManager.Instance.items.Length; i++)
            slots[i].Refresh();
        goldText.text = GameManager.Instance.gold.ToString();
    }

    public bool GetItem(Item _item)
    {// 아이템 획득
        int n = GameManager.Instance.items.Length + 1;
        tempItem = null;
        tempItem = new Item();
        tempItem.uid = _item.uid;
        tempItem.type = _item.type;
        tempItem.name = _item.name;
        tempItem.iconImg = _item.iconImg;
        tempItem.classNum = _item.classNum;
        tempItem.price = _item.price;
        tempItem.count = _item.count;
        tempItem.maxCount = _item.maxCount;
        tempItem.damage = _item.damage;
        tempItem.armor = _item.armor;
        tempItem.speed = _item.speed;
        tempItem.range = _item.range;
        tempItem.heal =  _item.heal;

        for (int i = 0; i < GameManager.Instance.items.Length; i++)
        {
            if (GameManager.Instance.items[i] == null)
            {
                if (i < n)
                    n = i;
            }
            else if (GameManager.Instance.items[i].uid == tempItem.uid && GameManager.Instance.items[i].count < GameManager.Instance.items[i].maxCount)
            {// 같은 아이템이면 count 증가
                if (GameManager.Instance.items[i].maxCount < GameManager.Instance.items[i].count + tempItem.count)
                {// 획득한 아이템의 수와 기존 수의 합이 번들 최대 개수보다 많다면 최대갯수까지 채우고 남은수량은 다른 칸에 저장
                    tempItem.count = GameManager.Instance.items[i].count + tempItem.count - GameManager.Instance.items[i].maxCount;
                    GameManager.Instance.items[i].count = GameManager.Instance.items[i].maxCount;
                    n++;
                }
                else
                {
                    GameManager.Instance.items[i].count += tempItem.count;
                    return true;
                }
            }
        }
        if (n < GameManager.Instance.items.Length)
        {
            GameManager.Instance.items[n] = tempItem;
            GameManager.Instance.items[n].uid = tempItem.uid;
            GameManager.Instance.items[n].type = tempItem.type;
            GameManager.Instance.items[n].name = tempItem.name;
            GameManager.Instance.items[n].iconImg = tempItem.iconImg;
            GameManager.Instance.items[n].classNum = tempItem.classNum;
            GameManager.Instance.items[n].price = tempItem.price;
            GameManager.Instance.items[n].count = tempItem.count;
            GameManager.Instance.items[n].maxCount = tempItem.maxCount;
            GameManager.Instance.items[n].damage = tempItem.damage;
            GameManager.Instance.items[n].armor = tempItem.armor;
            GameManager.Instance.items[n].speed = tempItem.speed;
            GameManager.Instance.items[n].range = tempItem.range;
            GameManager.Instance.items[n].heal = tempItem.heal;
            return true;
        }
        return false;
    }

    public void UseItem(int num)
    {// 아이템 사용
        player = gameScene.player;
        Item _item = GameManager.Instance.items[num];
        if (_item.type)
        {// 장비일 경우
            if (_item.classNum == 3)
            {// 방어구 착용
                AudioManager.Instance.PlaySFX(equip);
                switch (_item.uid)
                {
                    case 201:
                        if(GameManager.Instance.equip[0] != null)
                        {
                            Item prevItem = GameManager.Instance.equip[0];
                            GameManager.Instance.equip[0] = _item;
                            GameManager.Instance.items[num] = prevItem;
                            Debug.Log(prevItem.name + "와(과) " + _item.name + "을(를) 교체");
                        }
                        else
                        {
                            GameManager.Instance.equip[0] = _item;
                            GameManager.Instance.items[num] = null;
                            Debug.Log(_item.name + " 착용");
                        }
                        break;
                    case 202:
                        if (GameManager.Instance.equip[1] != null)
                        {
                            Item prevItem = GameManager.Instance.equip[1];
                            GameManager.Instance.equip[1] = _item;
                            GameManager.Instance.items[num] = prevItem;
                            Debug.Log(prevItem.name + "와(과) " + _item.name + "을(를) 교체");
                        }
                        else
                        {
                            GameManager.Instance.equip[1] = _item;
                            GameManager.Instance.items[num] = null;
                            Debug.Log(_item.name + " 착용");
                        }
                        break;
                    case 203:
                        if (GameManager.Instance.equip[2] != null)
                        {
                            Item prevItem = GameManager.Instance.equip[2];
                            GameManager.Instance.equip[2] = _item;
                            GameManager.Instance.items[num] = prevItem;
                            Debug.Log(prevItem.name + "와(과) " + _item.name + "을(를) 교체");
                        }
                        else
                        {
                            GameManager.Instance.equip[2] = _item;
                            GameManager.Instance.items[num] = null;
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
                    AudioManager.Instance.PlaySFX(equip);
                    if (GameManager.Instance.equip[3] != null)
                    {
                        Item prevItem = GameManager.Instance.equip[3];
                        GameManager.Instance.equip[3] = _item;
                        GameManager.Instance.items[num] = prevItem;
                        Debug.Log(prevItem.name + "와(과) " + _item.name + "을(를) 교체");
                    }
                    else
                    {
                        GameManager.Instance.equip[3] = _item;
                        GameManager.Instance.items[num] = null;
                        Debug.Log(_item.name + " 착용");
                    }
                }
                else
                {// 보조무기 착용
                    AudioManager.Instance.PlaySFX(equip);
                    if (GameManager.Instance.equip[4] != null)
                    {
                        Item prevItem = GameManager.Instance.equip[4];
                        GameManager.Instance.equip[4] = _item;
                        GameManager.Instance.items[num] = prevItem;
                        Debug.Log(prevItem.name + "와(과) " + _item.name + "을(를) 교체");
                    }
                    else
                    {
                        GameManager.Instance.equip[4] = _item;
                        GameManager.Instance.items[num] = null;
                        Debug.Log(_item.name + " 착용");
                    }
                }
            }
            else
            {
                AudioManager.Instance.PlaySFX(error);
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
                    if (player.maxHP <= player.HP + _item.heal)
                        player.HP = player.maxHP;
                    else
                        player.HP += _item.heal;
                    Debug.Log("체력포션 사용");
                    break;
                case 402:
                    if (player.maxMP <= player.MP + _item.heal)
                        player.MP = player.maxMP;
                    else
                        player.MP += _item.heal;
                    Debug.Log("마나포션 사용");
                    break;
                default:
                    break;
            }
            --GameManager.Instance.items[num].count;
            AudioManager.Instance.PlaySFX(potion);
            if (GameManager.Instance.items[num].count <= 0)
                GameManager.Instance.items[num] = null;
            player.Refresh();
        }
        gameScene.RefreshStat(); // 장비 스탯 새로고침
        slots[num].Refresh(); // UI 새로고침
    }
}
