using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputName : MonoBehaviour
{
    private string name = null;
    private Button btn_Create;
    [SerializeField]
    private GameObject inputText;
    private void Start()
    {
        btn_Create = GetComponent<Button>();
    }
    void Update()
    {// �Էµ� ���ڰ� 2���� �̻��̸� ĳ���� ���� ��ư Ȱ��ȭ
        name = inputText.GetComponent<TextMeshProUGUI>().text.ToString();
        if (name.Length > 2)
            btn_Create.interactable = true;
        else
            btn_Create.interactable = false;
    }
}
