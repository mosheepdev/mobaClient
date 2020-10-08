using Game.Net;
using Game.View;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View {
    public class RolesWindow : BaseWindow
    {
        public RolesWindow()
        {
            selfType = WindowType.RolesWindow;
            scenesType = ScenesType.Login;//
            resident = false;
            resName = "Roles/RolesWindow";
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        InputField InputField;
        protected override void Awake()
        {
            base.Awake();
            InputField = transform.Find("RolesBG/InputField").GetComponent<InputField>();
        }

        protected override void OnAddListener()
        {
            base.OnAddListener();
            NetEvent.Instance.AddEventListener(1201, HandleRolesCreateS2C);
        }

        private void HandleRolesCreateS2C(BufferEntity response)
        {
            //处理角色是否创建成功的逻辑
            RolesCreateS2C s2cMSG = ProtobufHelper.FromBytes<RolesCreateS2C>(response.proto);
            if (s2cMSG.Result==0)
            {
                //缓存角色 
                RolesCtrl.Instance.SaveRolesInfo(s2cMSG.RolesInfo);
                //关闭掉当前这个窗口
                Close();
                //打开大厅窗口
                WindowManager.Instance.OpenWindow(WindowType.LobbyWindow);
            }
            else
            {
                //角色已经存在 创建失败
                Debug.Log("角色已经存在 创建失败");
                WindowManager.Instance.ShowTips("已经存在相同的角色名,创建失败!");
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnRemoveListener()
        {
            base.OnRemoveListener();
            NetEvent.Instance.RemoveEventListener(1201, HandleRolesCreateS2C);
        }

        protected override void RegisterUIEvent()
        {
            base.RegisterUIEvent();
            for (int i = 0; i < buttonList.Length; i++)
            {
                switch (buttonList[i].name)
                {
                    case "StartBtn":
                        //Alt+Enter
                        buttonList[i].onClick.AddListener(StartBtnOnClick);
                        break;

                    default:
                        break;
                }
            }

        }

        /// <summary>
        /// 点击创建角色
        /// </summary>
        private void StartBtnOnClick()
        {
            RolesCreateC2S c2sMSG = new RolesCreateC2S();
            c2sMSG.NickName = InputField.text;
            BufferFactory.CreateAndSendPackage(1201, c2sMSG);
        }
    }

}
