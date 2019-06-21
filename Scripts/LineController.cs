using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour {

    [TextArea(5, 8)]
    public string info;

    private Text infoText;
    private GameObject[] lines;

    //private Color c;

    void Awake () {

        infoText = GameObject.Find("InfoText").GetComponent<Text>();
        lines = GameObject.FindGameObjectsWithTag("Line");
        //c = GetComponent<MeshRenderer>().material.color;
    }
	
	void Update () {
		
	}

    void OnMouseDown()
    {
        infoText.text = info;
        for (int i = 0; i < lines.Length; i++)
        {
            //lines[i].GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, 0.3f);
            lines[i].GetComponent<MeshRenderer>().enabled = false;
            lines[i].GetComponent<MeshCollider>().enabled = false;
        }
        //GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, 1.0f);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<MeshCollider>().enabled = true;
    }
}
