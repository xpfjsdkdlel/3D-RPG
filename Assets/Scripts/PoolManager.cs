using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // �����յ��� ������ ����
    public GameObject[] prefabs;
    // Ǯ���� ����ϴ� ����Ʈ
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // ��Ȱ��ȭ�� ���ӿ�����Ʈ ����
        foreach(GameObject obj in pools[index])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.SetActive(true);
                break;
            }
        }
        if(select == null)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    }
}
