using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectSceneManager : MonoBehaviour
{
    private GameObject selectCharacter = null;
    private Animator animator;
    [SerializeField]
    private GameObject inputField;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private GameObject classImg; // 직업 이미지
    [SerializeField]
    private GameObject charInfo; // 캐릭터 정보
    private SelectedCharacter sc;

    private void Start()
    {
        GameManager.Instance.fade.FadeIn();
    }

    void LoadGameScene()
    {
        // 게임매니저에 정보 넘기기
        GameManager.Instance.number = sc.number;
        GameManager.Instance.name = nameText.text;
        GameManager.Instance.LoadScene("GameScene");
    }

    void LoadExit()
    {
        SceneManager.LoadSceneAsync("TitleScene");
    }

    public void LoadScene(string SceneName)
    {
        AudioManager.Instance.PlaySFX(GameManager.Instance.click);
        GameManager.Instance.fade.FadeOut();
        Invoke("Load" + SceneName, 2f);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    AudioManager.Instance.PlaySFX(GameManager.Instance.click);
                    inputField.SetActive(true);
                    for (int i = 0; i < 3; i++)
                        classImg.transform.GetChild(i).gameObject.SetActive(false);
                    if (selectCharacter != null) // 선택된 캐릭터가 있다면 이전에 선택된 캐릭터를 대기 자세로 변경
                        animator.SetBool("isSelected", false);
                    selectCharacter = hit.transform.gameObject; // 클릭한 캐릭터가 선택됨
                    sc = selectCharacter.GetComponent<SelectedCharacter>();
                    classImg.transform.GetChild(sc.number).gameObject.SetActive(true); // 선택한 캐릭터의 직업이미지를 켬
                    for (int i = 0; i < 3; i++)
                    {
                        Transform t = charInfo.transform.GetChild(i);
                        t.GetComponent<Image>().sprite = sc.skill[i].iconImg;
                        t.GetChild(0).GetComponent<TextMeshProUGUI>().text = "<size=120%>" + sc.skill[i].name + "</size>\n마나 : <color=blue>" + sc.skill[i].cost + "</color> 쿨타임 : <color=green>" + sc.skill[i].coolDown + "</color>\n" + sc.skill[i].explan;
                    }
                    if(selectCharacter.TryGetComponent<Animator>(out animator))
                        animator.SetBool("isSelected", true); // 선택된 캐릭터를 준비 자세로 변경
                }
            }
        }
    }
}
