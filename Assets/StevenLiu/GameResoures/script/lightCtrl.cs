using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightCtrl : MonoBehaviour
{
    public Color lightColor;

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_light1Dir", -transform.forward);
        Shader.SetGlobalColor("_light1Color", lightColor);
    }
}