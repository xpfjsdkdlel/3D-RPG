using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestManager : MonoBehaviour
{
    private Fade fade;
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

    void Update()
    {    

    }
}
