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
    private Fade fade;
    [SerializeField]
    private GameObject inputField;
    [SerializeField]
    private GameObject classImg; // ���� �̹���
    private SelectedCharacter sc;

    private void Start()
    {
        fade = GameObject.FindObjectOfType<Fade>();
        if (fade == null)
        {
            fade = Instantiate(Resources.Load<Fade>("Prefabs/UI/Fade"));
            fade.Init();
        }
        fade.FadeIn();
    }

    void LoadGameScene()
    {
        // ���ӸŴ����� ���� �ѱ��
        // sc.number
        // sc.name
        SceneManager.LoadSceneAsync("TestScene");
    }

    void LoadExit()
    {
        SceneManager.LoadSceneAsync("TitleScene");
    }

    public void LoadScene(string SceneName)
    {
        fade.FadeOut();
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
                    inputField.SetActive(true);
                    for (int i = 0; i < 3; i++)
                        classImg.transform.GetChild(i).gameObject.SetActive(false);
                    if (selectCharacter != null) // ���õ� ĳ���Ͱ� �ִٸ� ������ ���õ� ĳ���͸� ��� �ڼ��� ����
                        animator.SetBool("isSelected", false);
                    selectCharacter = hit.transform.gameObject; // Ŭ���� ĳ���Ͱ� ���õ�
                    sc = selectCharacter.GetComponent<SelectedCharacter>();
                    classImg.transform.GetChild(sc.number).gameObject.SetActive(true); // ������ ĳ������ �����̹����� ��
                    if(selectCharacter.TryGetComponent<Animator>(out animator))
                    {
                        animator.SetBool("isSelected", true); // ���õ� ĳ���͸� �غ� �ڼ��� ����
                    }
                }
            }
        }
    }
}
