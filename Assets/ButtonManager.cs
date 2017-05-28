using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleButton(bool toggle)
    {
        if(toggle)
        {
            GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
        }
        else
        {
            GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
        }
    }
}
