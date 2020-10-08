using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoMsg;
using Google.Protobuf.Collections;
using System;

namespace Game.Model
{
    /// <summary>
    /// 保存房间里面的数据
    /// </summary>
    public class RoomModel : Singleton<RoomModel>
    {
        public RepeatedField<PlayerInfo> playerInfos;
        public Dictionary<int, GameObject> playerObjects = new Dictionary<int, GameObject>();
        public Dictionary<int, HeroAttributeEntity> heroCurrentAtt = new Dictionary<int, HeroAttributeEntity>();
        public Dictionary<int, HeroAttributeEntity> heroTotalAtt = new Dictionary<int, HeroAttributeEntity>();

       

        internal void SaveHeroAttribute(int rolesID, HeroAttributeEntity currenAttribute, HeroAttributeEntity totalAttribute)
        {
            heroCurrentAtt[rolesID] = currenAttribute;
            heroTotalAtt[rolesID] = totalAttribute;
        }

        public void Clear() {
            playerObjects.Clear();
            heroCurrentAtt.Clear();
            heroTotalAtt.Clear();
        }
    }
}
