using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private static Image background;

    public void Init()
    {
        background = GetComponentInChildren<Image>(true);
        // 신이 변경되더라도 파괴되지 않도록 처리
        DontDestroyOnLoad(gameObject);
        DeactiveImage();
    }

    public IEnumerator IChangeColor(Color start, Color end, float targetTime = 1.0f)
    {
        float elapsedTime = 0;
        background.color = start;
        background.gameObject.SetActive(true);
        while (true)
        {
            elapsedTime += Time.deltaTime / targetTime;
            Color color = Color.Lerp(start, end, elapsedTime);
            background.color = color;
            if(elapsedTime >= 1.0f)
                break;
            yield return null;
        }
    }
    void DeactiveImage()
    {
        background.gameObject.SetActive(false);
    }

    private void ChangeColor(Color start, Color end, float targetTime)
    {
        StartCoroutine(IChangeColor(start, end, targetTime));
    }

    public void FadeIn(float targetTime = 1.0f)
    {
        ChangeColor(Color.black, new Color(0, 0, 0, 0), targetTime);
        Invoke("DeactiveImage", targetTime);
    }
    public void FadeOut(float targetTime = 1.0f)
    {
        ChangeColor(new Color(0, 0, 0, 0), Color.black, targetTime);
    }
}
