using Game.Model;
using Google.Protobuf.Collections;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Ctrl {
    public class RoomCtrl : Singleton<RoomCtrl>
    {
        /// <summary>
        /// 获取阵营信息
        /// </summary>
        /// <param name="rolesID"></param>
        /// <returns></returns>
        public int GetTeamID(int rolesID)
        {
            for (int i = 0; i < PlayerModel.Instance.roomInfo.TeamA.Count; i++)
            {
                if (PlayerModel.Instance.roomInfo.TeamA[i].RolesID == rolesID)
                {
                    return 0;
                }

                if (PlayerModel.Instance.roomInfo.TeamB[i].RolesID == rolesID)
                {
                    return 1;
                }
            }
            return -1;
        }

      

        /// <summary>
        /// 检查英雄是否自己的ID
        /// </summary>
        /// <param name="rolesID"></param>
        /// <returns></returns>
        public bool CheckIsSelfRoles(int rolesID) {
            return PlayerModel.Instance.rolesInfo.RolesID == rolesID;
        }


        /// <summary>
        /// 清除房间信息,房间解散的时候调用 
        /// </summary>
        /// <param name="roomInfo"></param>
        public void RemoveRoomInfo()
        {
            PlayerModel.Instance.roomInfo = null;
        }

        /// <summary>
        /// 获取角色昵称
        /// </summary>
        /// <param name="rolesID"></param>
        /// <returns></returns>
        public string GetNickName(int rolesID) {
            for (int i = 0; i < PlayerModel.Instance.roomInfo.TeamA.Count; i++)
            {
                if (PlayerModel.Instance.roomInfo.TeamA[i].RolesID == rolesID)
                {
                    return PlayerModel.Instance.roomInfo.TeamA[i].NickName;
                }

                if (PlayerModel.Instance.roomInfo.TeamB[i].RolesID == rolesID)
                {
                    return PlayerModel.Instance.roomInfo.TeamB[i].NickName;
                }
            }
            return "";
       }

        /// <summary>
        /// 保存所有角色信息
        /// </summary>
        /// <param name="playerInfos"></param>
        public void SavePlayerList(RepeatedField<PlayerInfo> playerInfos) {
            RoomModel.Instance.playerInfos=playerInfos;
        }

        public PlayerInfo GetSelfPlayerInfo() {
           return  RoomModel.Instance.playerObjects[PlayerModel.Instance.rolesInfo.RolesID].
                GetComponent<PlayerCtrl>().playerInfo;
        }

        internal PlayerCtrl GetSelfPlayerCtrl()
        {
            return RoomModel.Instance.playerObjects[PlayerModel.Instance.rolesInfo.RolesID].
                GetComponent<PlayerCtrl>();
        }
    }
}

