using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointController : MonoBehaviour {

    [TextArea(5,8)]
    public string info;
    public string length;
    public string[] angle;
    public bool beSelected = false;

    private Text infoText;
    private UIController uiController;
    private Vector3 offset = new Vector3(0.05f, 0.05f, 0.7f);
    private GameObject[] points;

    void Awake () {
        
        infoText = GameObject.Find("InfoText").GetComponent<Text>();
        uiController = GameObject.Find("UIManager").GetComponent<UIController>();
        points = GameObject.FindGameObjectsWithTag("Point");
    }

    void Update () {
        
    }

    void OnMouseDown() {

        for (int i = 0; i < points.Length; i++)
        {
            points[i].GetComponentInChildren<PointController>().beSelected = false;
        }
        beSelected = true;

        if (uiController.gS == UIController.GameState.PointMode)
        {
            string i = info;
            infoText.text = i.Replace("n", "\n\n");
            Camera.main.transform.position = transform.position - offset;
        }

        if (uiController.gS == UIController.GameState.AcupunctureMode)
        {
            Camera.main.transform.position = transform.position - offset;
            uiController.GetPointTransfrom(transform.parent.transform);
        }
    }
}
