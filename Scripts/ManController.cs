using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManController : MonoBehaviour {

    public GameObject man;
    public float speed = 100;
    public GameObject playButton;

    private Vector3 startFingerPos;
    private Vector3 nowFingerPos;
    private float xMoveDistance;
    private float yMoveDistance;
    public int backValue = 0;
    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2) 

    private UIController uiController;

    void Awake() {

        uiController = GameObject.Find("UIManager").GetComponent<UIController>();
    }

    void Update () {

        if (Input.touchCount <= 0)
        {
            return;
        }

        if (uiController.gS == UIController.GameState.Init)
        {
            transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        if (uiController.gS == UIController.GameState.AcupunctureMode)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        if (uiController.gS == UIController.GameState.PointMode)
        {

            if (Input.touchCount == 1 && !playButton.activeInHierarchy)
            {
                //检查手指是否在UI元素上
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    //手指在UI元素上
                }
                else
                {
                    ScreenMove();

                    if (backValue == -1)
                    {
                        man.transform.Rotate(Vector3.up * -1 * Time.deltaTime * (speed / (transform.localScale.x < 0 ? 1 : transform.localScale.x)), Space.World);
                    }
                    else if (backValue == 1)
                    {
                        man.transform.Rotate(Vector3.up * Time.deltaTime * (speed / (transform.localScale.x < 0 ? 1 : transform.localScale.x)), Space.World);
                    }
                    else if (backValue == 2)
                    {
                        man.transform.Rotate(Vector3.right * Time.deltaTime * (speed / (transform.localScale.x < 0 ? 1 : transform.localScale.x)), Space.World);
                    }
                    else if (backValue == -2)
                    {
                        man.transform.Rotate(Vector3.right * -1 * Time.deltaTime * (speed / (transform.localScale.x < 0 ? 1 : transform.localScale.x)), Space.World);
                    }
                    else
                    {
                        man.transform.Rotate(Vector3.zero);
                    }
                }
                
            }

            //两指控制放大缩小
            if (Input.touchCount > 1 && !playButton.activeInHierarchy)
            {
                //多点触摸, 放大缩小  
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                //第2点刚开始接触屏幕, 只记录，不做处理  
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }

                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = newDistance - oldDistance;

                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset / 100f;
                Vector3 localScale = transform.localScale;
                Vector3 scale = new Vector3(localScale.x + scaleFactor, localScale.y + scaleFactor, localScale.z + scaleFactor);

                //最小缩放到 0.7 倍  最大放大到 2.2 倍
                if (scale.x > 0.7f && scale.y > 0.7f && scale.z > 0.7f && scale.x < 2.2f && scale.y < 2.2f && scale.z < 2.2f)
                {
                    transform.localScale = scale;
                }

                //记住最新的触摸点，下次使用  
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
        }
    }

    public void ScreenMove() {

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //Debug.Log("======开始触摸=====");
            startFingerPos = Input.GetTouch(0).position;
        }

        nowFingerPos = Input.GetTouch(0).position;

        if ((Input.GetTouch(0).phase == TouchPhase.Stationary) || (Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            backValue = 0;
            startFingerPos = nowFingerPos;
            //Debug.Log("======释放触摸=====");    
            return;
        }
        if (startFingerPos == nowFingerPos)
        {
            return;
        }
        xMoveDistance = Mathf.Abs(nowFingerPos.x - startFingerPos.x);
        yMoveDistance = Mathf.Abs(nowFingerPos.y - startFingerPos.y);

        if (nowFingerPos.x - nowFingerPos.x == 0 || nowFingerPos.y - nowFingerPos.y == 0)
        {
        }

        if (xMoveDistance > yMoveDistance)
        {
            if (nowFingerPos.x - startFingerPos.x > 0)
            {
                //Debug.Log("=======沿着X轴负方向移动=====");    
                backValue = -1; //沿着X轴负方向移动    
            }
            else if (nowFingerPos.x - startFingerPos.x < 0) 
            {
                //Debug.Log("=======沿着X轴正方向移动=====");    
                backValue = 1; //沿着X轴正方向移动    
            }
        }
        else
        {
            if (nowFingerPos.y - startFingerPos.y > 0)
            {
                //Debug.Log("=======沿着Y轴正方向移动=====");    
                backValue = 2; //沿着Y轴正方向移动    
            }
            else if (nowFingerPos.y - startFingerPos.y < 0)
            {
                //Debug.Log("=======沿着Y轴负方向移动=====");    
                backValue = -2; //沿着Y轴负方向移动    
            }
        }
    }
}
