using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightCtrl : MonoBehaviour
{
    private Light light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_light1Dir", -transform.forward);
        Shader.SetGlobalColor("_light1Color", light.color);
    }
}