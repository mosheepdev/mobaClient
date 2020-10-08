using System.Collections;
using System.Collections.Generic;
using ProtoMsg;
using UnityEngine;

namespace Game.Model {

    public class PlayerModel : Singleton<PlayerModel>
    {
        internal RolesInfo rolesInfo;
        internal RoomInfo roomInfo;

        /// <summary>
        /// 检查是否自己的角色
        /// </summary>
        /// <param name="rolesID"></param>
        /// <returns></returns>
        public bool CheckIsSelf(int rolesID) {
            return rolesInfo.RolesID == rolesID;
        }
    }
}

