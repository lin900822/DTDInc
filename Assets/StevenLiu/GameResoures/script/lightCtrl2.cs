using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightCtrl2 : MonoBehaviour
{
    public Color lightColor;
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_light2Dir", -transform.forward);
        Shader.SetGlobalColor("_light2Color", lightColor);
    }
}