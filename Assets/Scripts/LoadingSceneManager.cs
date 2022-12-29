using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField]
    private Image loadingBar;

    private void Awake()
    {
        GameManager.Instance.fade.FadeIn();
        loadingBar.fillAmount = 0f;
        StartCoroutine("LoadAsyncScene");
    }
    IEnumerator LoadAsyncScene()
    {
        yield return null;

        yield return YieldInstructionCache.WaitForSeconds(2f);
        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(GameManager.Instance.nextScene);
        asyncScene.allowSceneActivation = false;
        float timeC = 0f;
        while (!asyncScene.isDone)
        {
            yield return null;
            timeC += Time.deltaTime;

            if (asyncScene.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1f, timeC);
                if (loadingBar.fillAmount >= 0.999f)
                    asyncScene.allowSceneActivation = true;
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, asyncScene.progress, timeC);
                if (loadingBar.fillAmount >= asyncScene.progress)
                    timeC = 0f;
            }
        }
    }
}
