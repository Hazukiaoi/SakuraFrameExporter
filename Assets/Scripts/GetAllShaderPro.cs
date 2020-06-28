using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllShaderPro : MonoBehaviour
{
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        int pcount = material.shader.GetPropertyCount();
        for(int i = 0; i < pcount; i++)
        {
            Debug.Log(material.shader.GetPropertyName(i) + "|" + material.shader.GetPropertyType(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
