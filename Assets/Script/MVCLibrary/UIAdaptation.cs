using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAdaptation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //CanvasScaler canvasScaler = transform.GetComponent<CanvasScaler>();
        //if ((float)Screen.width/ (float)Screen.height>1920f/1080f)
        //{
        //    canvasScaler.matchWidthOrHeight = 1;
        //}
        //else if ((float)Screen.width / (float)Screen.height < 1920f / 1080f)
        //{
        //    canvasScaler.matchWidthOrHeight = 0;
        //}

       // GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("UIPrefab/Lobby/LobbyWindow"),transform);
        StartCoroutine(PosAdaptation());
        Debug.Log("第一帧的时候画布的宽是：" + transform.GetComponent<RectTransform>().rect.width);
    }
    
    IEnumerator PosAdaptation()
    {
        yield return null;//等待一帧是为了能正确的获取到画布的宽高，可以在游戏开始的时候等待一帧再来加载第一个预设，就不用每个预设都等待一帧再设置位置了。
        Debug.Log("第二帧的时候画布的宽是：" + transform.GetComponent<RectTransform>().rect.width);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform Child = transform.GetChild(i);
            Child.localPosition = new Vector2(Child.localPosition.x * (transform.GetComponent<RectTransform>().rect.width / 1920f), Child.localPosition.y * (transform.GetComponent<RectTransform>().rect.height / 1080f));
        }

    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
