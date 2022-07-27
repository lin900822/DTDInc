using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightCtrl3 : MonoBehaviour
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
        Shader.SetGlobalVector("_light3Dir", -transform.forward);
        Shader.SetGlobalColor("_light3Color", light.color);
    }
}