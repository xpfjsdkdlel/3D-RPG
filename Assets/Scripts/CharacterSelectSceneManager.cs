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
    private GameObject classImg; // ���� �̹���
    [SerializeField]
    private GameObject charInfo; // ĳ���� ����
    private SelectedCharacter sc;

    private void Start()
    {
        GameManager.Instance.fade.FadeIn();
    }

    void LoadGameScene()
    {
        // ���ӸŴ����� ���� �ѱ��
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
                    if (selectCharacter != null) // ���õ� ĳ���Ͱ� �ִٸ� ������ ���õ� ĳ���͸� ��� �ڼ��� ����
                        animator.SetBool("isSelected", false);
                    selectCharacter = hit.transform.gameObject; // Ŭ���� ĳ���Ͱ� ���õ�
                    sc = selectCharacter.GetComponent<SelectedCharacter>();
                    classImg.transform.GetChild(sc.number).gameObject.SetActive(true); // ������ ĳ������ �����̹����� ��
                    for (int i = 0; i < 3; i++)
                    {
                        Transform t = charInfo.transform.GetChild(i);
                        t.GetComponent<Image>().sprite = sc.skill[i].iconImg;
                        t.GetChild(0).GetComponent<TextMeshProUGUI>().text = "<size=120%>" + sc.skill[i].name + "</size>\n���� : <color=blue>" + sc.skill[i].cost + "</color> ��Ÿ�� : <color=green>" + sc.skill[i].coolDown + "</color>\n" + sc.skill[i].explan;
                    }
                    if(selectCharacter.TryGetComponent<Animator>(out animator))
                        animator.SetBool("isSelected", true); // ���õ� ĳ���͸� �غ� �ڼ��� ����
                }
            }
        }
    }
}
