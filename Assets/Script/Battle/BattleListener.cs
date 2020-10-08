using System;
using System.Collections;
using System.Collections.Generic;
using Game.Net;
using ProtoMsg;
using UnityEngine;

/// <summary>
/// 战斗监听器
/// </summary>
public class BattleListener : Singleton<BattleListener>
{
    //初始化的方法 监听战斗的网络消息
    public void Init() {
        awaitHandle = new Queue<BattleUserInputS2C>();
        NetEvent.Instance.AddEventListener(1500, HandleBattleUserInputS2C);
    }

    Queue<BattleUserInputS2C> awaitHandle;
    //处理存储网络事件的方法
    private void HandleBattleUserInputS2C(BufferEntity response)
    {
        BattleUserInputS2C s2cMSG = ProtobufHelper.FromBytes<BattleUserInputS2C>(response.proto);
        awaitHandle.Enqueue(s2cMSG);
    }


    //释放的方法 移除监听网络消息
    public void Relese()
    {
        NetEvent.Instance.RemoveEventListener(1500, HandleBattleUserInputS2C);
        awaitHandle.Clear();
    }

    //调度/播放网络事件的方法
    public void PlayerFrame(Action<BattleUserInputS2C> action) {
        if (action!=null&& awaitHandle.Count>0)
        {
            action(awaitHandle.Dequeue());
        }
    }
}
