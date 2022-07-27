using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightCtrl2 : MonoBehaviour
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
        Shader.SetGlobalVector("_light2Dir", -transform.forward);
        Shader.SetGlobalColor("_light2Color", light.color);
    }
}