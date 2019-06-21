using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedleController : MonoBehaviour {

    private Text angleText;
    private string angle;
    private Text lengthText;
    private Text warningText1;
    private Text warningText2;
    private float timer = 0.0f;
    private bool warningState = false;

    private Vector3 targetScreenSpace;
    private Vector3 targetWorldSpace;
    private Transform targetTransform;
    private Vector3 mouseScreenSpace;
    private Vector3 offset;

    public bool beSelected = false;

    public ManController mC;
    private UIController uC;

    void Awake()
    {
        mC = GameObject.Find("Man_Handle").GetComponent<ManController>();
        uC = GameObject.Find("UIManager").GetComponent<UIController>();
    }

    void Start() {

        targetTransform = GetComponent<Transform>();

        angleText = GameObject.Find("AngleText").GetComponent<Text>();
        lengthText = GameObject.Find("LengthText").GetComponent<Text>();
        warningText1 = GameObject.Find("WarningText1").GetComponent<Text>();
        warningText2 = GameObject.Find("WarningText2").GetComponent<Text>();
    }

    void Update() {

        if (uC.gS != UIController.GameState.AcupunctureMode)
        {
            Destroy(gameObject);
        }

        beSelected = transform.parent.GetComponentInChildren<PointController>().beSelected;

        if ((Mathf.Round(targetTransform.eulerAngles.y)) > 270)
        {
            angle = ((Mathf.Round(targetTransform.eulerAngles.y)) - 270).ToString();
            angleText.text = "角度：" + angle + " ° ";
        }
        else
        {
            angle = (90 - (Mathf.Round(targetTransform.eulerAngles.y))).ToString();
            angleText.text = "角度：" + angle + " ° ";
        }

        if (beSelected)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                targetTransform.Translate(0, 0, 1.0f * Time.deltaTime, Space.World);
            }
        }

        if (beSelected)
        {
            //控制针的旋转
            if (Input.touchCount == 1)
            {
                mC.ScreenMove();

                if (mC.backValue == -1)
                {
                    targetTransform.Rotate(0.0f, 0.0f, 0.1f);
                }
                else if (mC.backValue == 1)
                {
                    targetTransform.Rotate(0.0f, 0.0f, -0.1f);
                }

                if ((Mathf.Round(targetTransform.eulerAngles.y)) > 270)
                {
                    angleText.text = "角度：" + ((Mathf.Round(targetTransform.eulerAngles.y)) - 270) + " ° ";
                }
                else
                {
                    angleText.text = "角度：" + (90 - (Mathf.Round(targetTransform.eulerAngles.y))) + " ° ";
                }

                //控制针的刺入
                if (mC.backValue == 2)
                {
                    targetTransform.Translate(0, -0.1f * Time.deltaTime, 0);
                }
                else if (mC.backValue == -2)
                {
                    targetTransform.Translate(0, 0.1f * Time.deltaTime, 0);
                }
            }
        }

        if (warningState)
        {
            timer += Time.deltaTime;
            if (timer > 1.0f)
            {
                warningText1.text = "";
                warningText2.text = "";
                timer = 0;
                warningState = false;
            }
        }
    }

    void OnTriggerStay(Collider collider) {

        if (collider.tag == "Point")
        {
            float l = (float)Math.Round((Vector3.Distance(transform.position, collider.ClosestPoint(transform.position)) * 10 + 0.14f), 1);
            lengthText.text = "长度：" + l + "寸";
            if (l > float.Parse(collider.transform.GetComponentInChildren<PointController>().length))
            {
                warningText1.text = "刺入过深";
                warningState = true;
            }
        }

        switch (collider.transform.GetComponentInChildren<PointController>().angle.Length)
        {
            case 1:
                if (angle != collider.transform.GetComponentInChildren<PointController>().angle[0])
                {
                    warningText2.text = "角度不正确";
                    warningState = true;
                }
                break;
            case 2:
                if (angle != collider.transform.GetComponentInChildren<PointController>().angle[0] &&
                    angle != collider.transform.GetComponentInChildren<PointController>().angle[1])
                {
                    warningText2.text = "角度不正确";
                    warningState = true;
                }
                break;
            default:
                break;
        }
    }
}
