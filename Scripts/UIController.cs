using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public enum GameState
    {
        Init, AcupunctureMode,PointMode
    }

    public GameObject needle_1;

    public GameObject UIName;
    public GameObject playButton;
    //public GameObject helpButton;
    //public GameObject helpText;
    public GameObject chooseUI;
    public GameObject detectUI;
    public GameObject toolsBG;
    public Text infoText;

    public GameState gS = GameState.Init;

    private Transform pointTransform;
    private GameObject[] lines;

    private int flag = 0;

    void Awake() {

        lines = GameObject.FindGameObjectsWithTag("Line");
    }

    void Start() {

        //chooseUI.SetActive(true);
    }

    void Update() {

        toolsBG.SetActive(gS == GameState.AcupunctureMode);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !playButton.activeInHierarchy && gS == GameState.Init)
        {
            //检查手指是否在UI元素上
            if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                //手指在UI元素上
            }
            else
            {
                //手指不在UI元素上

                //导航栏的显示或消失
                if (chooseUI.activeInHierarchy)
                {
                    chooseUI.SetActive(false);
                }
                else if (!chooseUI.activeInHierarchy)
                {
                    chooseUI.SetActive(true);
                }
            }
        }
    }

    public void ClickPlayButton() {

        UIName.SetActive(false);
        playButton.SetActive(false);
        chooseUI.SetActive(true);
    }

    public void ClickNeedle1Button()
    {
        if (gS==GameState.AcupunctureMode)
        {
            if (pointTransform.childCount == 1)
            {
                Instantiate(needle_1, pointTransform.position + new Vector3(0.0f, 0.0f, -0.1f), Quaternion.Euler(90.0f, 0.0f, 0.0f), pointTransform.transform);
            }
        }
    }

    public void ClickAcupunctureButton()
    {
        infoText.text = "";
        detectUI.SetActive(true);
        gS = GameState.AcupunctureMode;
    }

    public void ClickPointButton()
    {
        Camera.main.transform.position = new Vector3(0.0f, -0.5f, -7.5f);
        gS = GameState.PointMode;
        detectUI.SetActive(false);
    }

    public void ClickQuitButton()
    {
        gS = GameState.Init;
        infoText.text = "";
        detectUI.SetActive(false);
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].GetComponent<MeshRenderer>().enabled = true;
            lines[i].GetComponent<MeshCollider>().enabled = true;
        }
    }

    public void GetPointTransfrom(Transform t)
    {
        pointTransform = t;
    }
}
