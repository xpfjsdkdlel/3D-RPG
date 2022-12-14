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
    {// 입력된 글자가 2글자 이상이면 캐릭터 생성 버튼 활성화
        name = inputText.GetComponent<TextMeshProUGUI>().text.ToString();
        if (name.Length > 2)
            btn_Create.interactable = true;
        else
            btn_Create.interactable = false;
    }
}
