using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightCtrl3 : MonoBehaviour
{
    public Color lightColor;

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_light3Dir", -transform.forward);
        Shader.SetGlobalColor("_light3Color", lightColor);
    }
}