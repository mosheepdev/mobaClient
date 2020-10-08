using System;
using System.Collections;
using System.Collections.Generic;
using Game.Net;
using ProtoMsg;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class LobbyWindow : BaseWindow
    {
        public LobbyWindow()
        {
            selfType = WindowType.LobbyWindow;
            scenesType = ScenesType.Login;//
            resident = false;
            resName = "Lobby/LobbyWindow";
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        Transform MatchModeBtn, QualifyingBtn, StopMatchBtn;
        Text RolesName, Duan, GoldCount, DiamondsCount, MatchTips;
        protected override void Awake()
        {
            base.Awake();
            //昵称 段位 金币 钻石 
            RolesName = transform.Find("LobbyBG/RolesName").GetComponent<Text>();
            Duan = transform.Find("LobbyBG/Duan").GetComponent<Text>();
            GoldCount = transform.Find("LobbyBG/GoldCount").GetComponent<Text>();
            DiamondsCount = transform.Find("LobbyBG/DiamondsCount").GetComponent<Text>();

            //匹配按钮 排位按钮
            MatchModeBtn = transform.Find("LobbyBG/MatchModeBtn");
            QualifyingBtn = transform.Find("LobbyBG/QualifyingBtn");
            StopMatchBtn = transform.Find("LobbyBG/StopMatchBtn");

            //提示
            MatchTips = transform.Find("LobbyBG/MatchTips").GetComponent<Text>();

        }

        protected override void OnAddListener()
        {
            base.OnAddListener();
            NetEvent.Instance.AddEventListener(1300, HandleLobbyToMatchS2C);
            NetEvent.Instance.AddEventListener(1301, HandleLobbyUpdateMatchStateS2C);
            NetEvent.Instance.AddEventListener(1302, HandleLobbyQuitMatchS2C);
        }

        /// <summary>
        /// 退出匹配的结果
        /// </summary>
        /// <param name="obj"></param>
        private void HandleLobbyQuitMatchS2C(BufferEntity response)
        {
            LobbyQuitMatchS2C s2cMSG = ProtobufHelper.FromBytes<LobbyQuitMatchS2C>(response.proto);
            if (s2cMSG.Result==0)
            {
                //匹配和排位 激活
                MatchModeBtn.gameObject.SetActive(true);
                QualifyingBtn.gameObject.SetActive(true);
                //停止匹配和匹配提示的物体隐藏掉
                StopMatchBtn.gameObject.SetActive(false);
                MatchTips.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 更新匹配的状态 
        /// </summary>
        /// <param name="obj"></param>
        private void HandleLobbyUpdateMatchStateS2C(BufferEntity response)
        {
            LobbyUpdateMatchStateS2C s2cMSG = ProtobufHelper.FromBytes<LobbyUpdateMatchStateS2C>(response.proto);
            if (s2cMSG.Result==0)
            {
                MatchModeBtn.gameObject.SetActive(true);
                QualifyingBtn.gameObject.SetActive(true);
                StopMatchBtn.gameObject.SetActive(false);
                MatchTips.gameObject.SetActive(false);

                //房间信息 
                RolesCtrl.Instance.SaveRoomInfo(s2cMSG.RoomInfo);

                Close();
                WindowManager.Instance.OpenWindow(WindowType.RoomWindow);
                
            }
        
        }

        /// <summary>
        /// 进入匹配的结果
        /// </summary>
        /// <param name="obj"></param>
        private void HandleLobbyToMatchS2C(BufferEntity response)
        {
            LobbyToMatchS2C s2cMSG = ProtobufHelper.FromBytes<LobbyToMatchS2C>(response.proto);
            if (s2cMSG.Result==0)
            {
                MatchModeBtn.gameObject.SetActive(false);
                QualifyingBtn.gameObject.SetActive(false);
                StopMatchBtn.gameObject.SetActive(true);
                MatchTips.gameObject.SetActive(true);
            }
            else
            {
                 //无法进行匹配 可能是被惩罚 需要等待
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            //获取到角色信息 然后进行更新
           RolesInfo roles= RolesCtrl.Instance.GetRolesInfo();
            //RolesName, Duan, GoldCount, DiamondsCount
            RolesName.text = roles.NickName;
            Duan.text = roles.VictoryPoint.ToString();//胜点
            GoldCount.text = roles.GoldCoin.ToString();
            DiamondsCount.text = roles.Diamonds.ToString();
        }

        protected override void OnRemoveListener()
        {
            base.OnRemoveListener();
            NetEvent.Instance.RemoveEventListener(1300, HandleLobbyToMatchS2C);
            NetEvent.Instance.RemoveEventListener(1301, HandleLobbyUpdateMatchStateS2C);
            NetEvent.Instance.RemoveEventListener(1302, HandleLobbyQuitMatchS2C);
        }

        protected override void RegisterUIEvent()
        {
            base.RegisterUIEvent();
            for (int i = 0; i < buttonList.Length; i++)
            {
                switch (buttonList[i].name)
                {
                    case "MatchModeBtn":
                        buttonList[i].onClick.AddListener(MatchModeBtnOnClick);
                        break;
                    case "StopMatchBtn":
                        buttonList[i].onClick.AddListener(StopMatchBtnOnClick);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 停止匹配
        /// </summary>
        private void StopMatchBtnOnClick()
        {
            BufferFactory.CreateAndSendPackage(1302, new LobbyQuitMatchC2S());
        }

        /// <summary>
        /// 点击了匹配按钮
        /// </summary>
        private void MatchModeBtnOnClick()
        {
            BufferFactory.CreateAndSendPackage(1300, new LobbyToMatchC2S());
        }
    }
}
