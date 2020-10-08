using Game.Model;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolesCtrl : Singleton<RolesCtrl>
{
    public void SaveRolesInfo(RolesInfo rolesInfo) {
        PlayerModel.Instance.rolesInfo = rolesInfo;
    }

    public RolesInfo GetRolesInfo()
    {
        return PlayerModel.Instance.rolesInfo;
    }

    /// <summary>
    /// 保存房间信息
    /// </summary>
    /// <param name="roomInfo"></param>
    public void SaveRoomInfo(RoomInfo roomInfo) {
        PlayerModel.Instance.roomInfo = roomInfo;
    }

    //已经放到房间控制器去了
    /// <summary>
    /// 清除房间信息,房间解散的时候调用 
    /// </summary>
    /// <param name="roomInfo"></param>
    //public void RemoveRoomInfo()
    //{
    //    PlayerModel.Instance.roomInfo = null;
    //}

    /// <summary>
    /// 获取房间信息
    /// </summary>
    /// <param name="roomInfo"></param>
    public RoomInfo GetRoomInfo()
    {
        return PlayerModel.Instance.roomInfo;
    }


}
