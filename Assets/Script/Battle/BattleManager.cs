using Game.Model;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //BattleSceneConfig 
    //10个英雄的位置 角度
    //野区野怪的配置 防御塔的配置 ==

    /// <summary>
    /// 出生的位置
    /// </summary>
    public Vector3[] spawnPosition = new Vector3[10] { 
        //A队伍的位置 0-4
        new Vector3(-6.52f,0,-8.96f),
        new Vector3(-2.26f,0,-3.71f),
        new Vector3(-6.71f,0,-4.01f ),
        new Vector3(-4.28f,0,-5.89f ),
        new Vector3(-2.02f,0,-8.23f), 
        //B队伍的位置 5-9
        new Vector3(-95.198f,0,-96.542f),
        new Vector3(-99.892f,0,-101.4f ),
        new Vector3(-95.432f,0,-101.49f),
        new Vector3(-97.692f,0,-99.409f),
        new Vector3(-99.7443f,0,-96.884f), };

    /// <summary>
    /// 初始的角度
    /// </summary>
    public Vector3[] spawnRotation = new Vector3[10] {
        //A队伍的角度
        new Vector3(0,242.49f,0),
        new Vector3(0,-122.251f,0),
        new Vector3( 0,-152.659f,0 ),
        new Vector3(0,230.56f,0),
        new Vector3( 0,-149.089f,0 ),

        //B队伍的角度
        new Vector3(0,67.403f,0 ),
        new Vector3(0,-297.338f,0 ),
        new Vector3( 0,-327.746f,0),
        new Vector3(0,55.473f,0),
        new Vector3(0,-324.176f,0), };

    Dictionary<int, PlayerCtrl> playerCtrlDIC = new Dictionary<int, PlayerCtrl>();

    bool isInit = false;
    //初始化
    private void Awake()
    {
        //取得缓存的数据 房间里面的所有角色 
        foreach (var playerInfo in RoomModel.Instance.playerInfos)
        {
            //创建角色(模型)
            GameObject hero=  ResManager.Instance.LoadHero(playerInfo.HeroID);
            //设置它的位置
            hero.transform.position = spawnPosition[playerInfo.PosID];
            hero.transform.eulerAngles= spawnRotation[playerInfo.PosID];
            //添加控制器
            PlayerCtrl playerCtrl = hero.AddComponent<PlayerCtrl>();

            //缓存 角色物体
            playerCtrlDIC[playerInfo.RolesInfo.RolesID] = playerCtrl;
            RoomModel.Instance.playerObjects[playerInfo.RolesInfo.RolesID] = hero;

            //每个角色自己要做初始化 每个控制器
            playerCtrl.Init(playerInfo);
        }

        //加载战斗界面
        WindowManager.Instance.OpenWindow(WindowType.BattleWindow);

        //输入管理器的初始化
        this.gameObject.AddComponent<InputCtrl>()
            .Init(playerCtrlDIC[PlayerModel.Instance.rolesInfo.RolesID]);


        isInit = true;
    }

    //发送网络事件的玩家的FSM 处理它的事件
    public void HandleCMD(BattleUserInputS2C s2cMSG) {
        //先确定 这条操作命令是哪个玩家发的
        //然后调用它的角色控制器 -FSM状态机 去处理这个事件
        playerCtrlDIC[s2cMSG.CMD.RolesID].playerFSM.HandleCMD(s2cMSG);
    }

    //每帧取网络事件进行播放 
    void Update()
    {
        if (isInit)
        {
            //输出的控制 是从缓存的帧数据
            BattleListener.Instance.PlayerFrame(HandleCMD);
        }
    }

    private void OnDestroy()
    {
        //RoomModel.Instance.playerObjects.Clear();
        RoomModel.Instance.Clear();
    }
}
