using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoMsg
{
    public class PBConfig: Singleton<PBConfig>
    {
       // S server  C client
        public  Dictionary<int, Type> PBC2SDic = new Dictionary<int, Type>();
        public  Dictionary<int, Type> PBS2CDic = new Dictionary<int, Type>();

        /// <summary>
        /// 初始化 两个字典
        /// </summary>
        public PBConfig()
        {
            //工具自动化生成
            C2SmsgInit();
            S2CmsgInit();
        }

        void C2SmsgInit() {
            
            PBC2SDic.Add(1000,typeof(UserRegisterC2S));
            PBC2SDic.Add(1001,typeof(UserLoginC2S));
            PBC2SDic.Add(1003,typeof(UserQuitC2S));
            PBC2SDic.Add(1201,typeof(RolesCreateC2S));
            PBC2SDic.Add(1300,typeof(LobbyToMatchC2S));
            PBC2SDic.Add(1301,typeof(LobbyUpdateMatchStateC2S));
            PBC2SDic.Add(1302,typeof(LobbyQuitMatchC2S));
            PBC2SDic.Add(1400,typeof(RoomSelectHeroC2S));
            PBC2SDic.Add(1401,typeof(RoomSelectHeroSkillC2S));
            PBC2SDic.Add(1402,typeof(RoomCreateC2S));
            PBC2SDic.Add(1403,typeof(RoomCloseC2S));
            PBC2SDic.Add(1404,typeof(RoomSendMsgC2S));
            PBC2SDic.Add(1405,typeof(RoomLockHeroC2S));
            PBC2SDic.Add(1406,typeof(RoomLoadProgressC2S));
            PBC2SDic.Add(1407,typeof(RoomToBattleC2S));
            PBC2SDic.Add(1500,typeof(BattleUserInputC2S));
        }

        void S2CmsgInit() {
            
            PBS2CDic.Add(1000,typeof(UserRegisterS2C));
            PBS2CDic.Add(1001,typeof(UserLoginS2C));
            PBS2CDic.Add(1003,typeof(UserQuitS2C));
            PBS2CDic.Add(1201,typeof(RolesCreateS2C));
            PBS2CDic.Add(1300,typeof(LobbyToMatchS2C));
            PBS2CDic.Add(1301,typeof(LobbyUpdateMatchStateS2C));
            PBS2CDic.Add(1302,typeof(LobbyQuitMatchS2C));
            PBS2CDic.Add(1400,typeof(RoomSelectHeroS2C));
            PBS2CDic.Add(1401,typeof(RoomSelectHeroSkillS2C));
            PBS2CDic.Add(1402,typeof(RoomCreateS2C));
            PBS2CDic.Add(1403,typeof(RoomCloseS2C));
            PBS2CDic.Add(1404,typeof(RoomSendMsgS2C));
            PBS2CDic.Add(1405,typeof(RoomLockHeroS2C));
            PBS2CDic.Add(1406,typeof(RoomLoadProgressS2C));
            PBS2CDic.Add(1407,typeof(RoomToBattleS2C));
            PBS2CDic.Add(1500,typeof(BattleUserInputS2C));
        }

        public bool CheckC2SPB(int id) {
            return PBC2SDic.Keys.Contains(id);
        }

        public bool CheckS2CPB(int id)
        {
            return PBS2CDic.Keys.Contains(id);
        }
    }
}