using Game.Model;
using Game.Net;
using Game.View;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCtrl : MonoBehaviour
{
    //只需要监听用户的按键 然后发送网络消息就可以了
    PlayerCtrl playerCtrl;
    public void Init(PlayerCtrl playerCtrl) {
        this.playerCtrl = playerCtrl;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SendInputCMD(KeyCode.Q);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            SendInputCMD(KeyCode.W);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SendInputCMD(KeyCode.E);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SendInputCMD(KeyCode.R);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SendInputCMD(KeyCode.A);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SendInputCMD(KeyCode.D);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            SendInputCMD(KeyCode.F);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            SendInputCMD(KeyCode.B);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MouseDownEvent(KeyCode.Mouse0);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SendInputCMD(KeyCode.Mouse1);
        }
        //回放的时候 看到用户所有操作 第一人称的视角 
        //把用户的所有输入 都发送给服务器 鼠标移动的位置
        //缓存起来 转发
    }

    Ray ray;
    void SendInputCMD(KeyCode keyCode) {
        BattleUserInputC2S c2sMSG = new BattleUserInputC2S();
        c2sMSG.RolesID = PlayerModel.Instance.rolesInfo.RolesID;
        c2sMSG.RoomID = PlayerModel.Instance.roomInfo.ID;

        c2sMSG.Key= keyCode.GetHashCode();

        Ray ray;
        RaycastHit hit;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out hit))
        {

            c2sMSG.MousePosition = hit.point.ToV3Info();
            if (lockTransform!=null)
            {
                c2sMSG.LockID = lockObjectID;
                c2sMSG.LockTag = lockTransform.tag;
            }
        }
        BufferFactory.CreateAndSendPackage(1500,c2sMSG);
    }
    Transform lockTransform;
    int lockObjectID;

    public void MouseDownEvent(KeyCode inputType) {
        Ray ray;
        RaycastHit hit;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            if(hit.transform.gameObject.CompareTag("Ground"))
            {

            }
            else if (hit.transform.CompareTag("Player"))
            {
                this.lockTransform = hit.transform;
                this.lockObjectID = hit.transform.GetComponent<PlayerCtrl>().playerInfo.RolesInfo.RolesID;
                //点击到人物
                //通知战斗面板 进行更新战斗界面锁定的人物 TODO
                BattleWindow battleWindow=(BattleWindow)WindowManager.Instance.GetWindow(WindowType.BattleWindow);
                battleWindow.ShowSelectObjectInfo(hit.transform.gameObject);

            }


        }
    }
}
