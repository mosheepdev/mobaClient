using Game.Ctrl;
using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View {

    public class LoginWindow : BaseWindow
    {
        public LoginWindow() {
            selfType = WindowType.LobbyWindow;
            scenesType = ScenesType.Login;
            resident = false;
            resName = "User/LoginWindow";
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        InputField AccountInput;
        InputField PwdInput;
        //初始化
        protected override void Awake()
        {
            base.Awake();
            AccountInput = transform.Find("UserBack/AccountInput").GetComponent<InputField>();
            PwdInput = transform.Find("UserBack/PwdInput").GetComponent<InputField>();
        }

        protected override void OnAddListener()
        {
            base.OnAddListener();
            NetEvent.Instance.AddEventListener(1000, HandleUserRegisterS2C);
            NetEvent.Instance.AddEventListener(1001, HandleUserLoginS2C);
        }

        /// <summary>
        /// 返回了登录的结果
        /// </summary>
        /// <param name="obj"></param>
        private void HandleUserLoginS2C(BufferEntity p)
        {
            UserLoginS2C s2cMSG = ProtobufHelper.FromBytes<UserLoginS2C>(p.proto);
            switch (s2cMSG.Result)
            {
                case 0:
                    Debug.Log("登录成功!");
                    //保存数据
                    if (s2cMSG.RolesInfo!=null)
                    {
                        //保存数据   
                        LoginCtrl.Instance.SaveRolesInfo(s2cMSG.RolesInfo);
                        //打开大厅界面
                        WindowManager.Instance.OpenWindow(WindowType.LobbyWindow);
                    }
                    else
                    {
                        //跳转到角色界面
                        WindowManager.Instance.OpenWindow(WindowType.RolesWindow);
                    }
                    Close();//关闭自己
                    break;
                case 2:
                    Debug.Log("帐号密码不匹配!");
                    WindowManager.Instance.ShowTips("帐号密码不匹配!");
                    //打开提示窗体
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 返回了注册结果 
        /// </summary>
        /// <param name="obj"></param>
        private void HandleUserRegisterS2C(BufferEntity p)
        {
            UserRegisterS2C s2cMSG = ProtobufHelper.FromBytes<UserRegisterS2C>(p.proto);
            switch (s2cMSG.Result)
            {
                case 0:
                    Debug.Log("注册成功!");
                    //打开提示窗体 提示
                    WindowManager.Instance.ShowTips("注册成功!");
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    Debug.Log("帐号已被注册!");
                    WindowManager.Instance.ShowTips("帐号已被注册!");
                    //打开提示窗体
                    break;
                default:
                    break;
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
        }

        protected override void RegisterUIEvent()
        {
            base.RegisterUIEvent();
            for (int i = 0; i < buttonList.Length; i++)
            {
                switch (buttonList[i].name)
                {
                    case "RegisterBtn":
                        buttonList[i].onClick.AddListener(RegisterBtnOnClick);
                        break;
                    case "LoginBtn":
                        buttonList[i].onClick.AddListener(LoginBtnOnClick);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 登录按钮点击的事件
        /// </summary>
        private void LoginBtnOnClick()
        {
            if (string.IsNullOrEmpty(AccountInput.text))
            {
                Debug.Log("帐号为空 ...");
                return;
            }

            if (string.IsNullOrEmpty(PwdInput.text))
            {
                Debug.Log("密码为空 ...");
                return;
            }
            //Debug.Log("发哦少年宫登录消息");
            UserLoginC2S c2sMSG = new UserLoginC2S();
            c2sMSG.UserInfo = new UserInfo();
            c2sMSG.UserInfo.Account = AccountInput.text;
            c2sMSG.UserInfo.Password = PwdInput.text;
            BufferFactory.CreateAndSendPackage(1001, c2sMSG);
        }

        /// <summary>
        /// 注册按钮点击的事件
        /// </summary>
        private void RegisterBtnOnClick()
        {
            if (string.IsNullOrEmpty(AccountInput.text))
            {
                Debug.Log("帐号为空 ...");
                return;
            }

            if (string.IsNullOrEmpty(PwdInput.text))
            {
                Debug.Log("密码为空 ...");
                return;
            }

            UserRegisterC2S c2sMSG = new UserRegisterC2S();
            c2sMSG.UserInfo = new UserInfo();
            c2sMSG.UserInfo.Account = AccountInput.text;
            c2sMSG.UserInfo.Password = PwdInput.text;

            BufferFactory.CreateAndSendPackage(1000, c2sMSG);
        }
    }

}
