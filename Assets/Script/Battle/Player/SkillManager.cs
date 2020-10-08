﻿using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    PlayerCtrl playerCtrl;
    PlayerInfo playerInfo;

    Dictionary<KeyCode, int> skillID = new Dictionary<KeyCode, int>();
    Dictionary<KeyCode, float> coolingConfig = new Dictionary<KeyCode, float>();

    public void Init(PlayerCtrl playerCtrl)
    {
        this.playerCtrl = playerCtrl;
        playerInfo = playerCtrl.playerInfo;

        //技能的配置信息
        HeroSkillEntity skill= HeroSkillConfig.GetInstance(playerInfo.HeroID);
        skillID[KeyCode.Q] = skill.Q_ID;
        skillID[KeyCode.W] = skill.W_ID;
        skillID[KeyCode.E] = skill.E_ID;
        skillID[KeyCode.R] = skill.R_ID;
        skillID[KeyCode.A] = skill.A_ID;

        //d f
        skillID[KeyCode.D] = playerInfo.SkillA;
        skillID[KeyCode.F] = playerInfo.SkillB;
        skillID[KeyCode.B] = 4;

        //技能的冷却时间 shift+alt+鼠标左键选择
        coolingConfig[KeyCode.Q] = AllSkillConfig.Get(skill.Q_ID).CoolingTime;
        coolingConfig[KeyCode.W] = AllSkillConfig.Get(skill.W_ID).CoolingTime;
        coolingConfig[KeyCode.E] = AllSkillConfig.Get(skill.E_ID).CoolingTime;
        coolingConfig[KeyCode.R] = AllSkillConfig.Get(skill.R_ID).CoolingTime;
        coolingConfig[KeyCode.A] = 0.5f;

        //d f
        coolingConfig[KeyCode.D] = 180;
        coolingConfig[KeyCode.F] = 180;
        coolingConfig[KeyCode.B] = 4;//回城的时间

        //最近一次按下的时间 
    }

    Dictionary<KeyCode, float> keyDownTime = new Dictionary<KeyCode, float>();
    public void DoCooling(KeyCode key, Action<float> action) {
        keyDownTime[key] = Time.time;
        if (action!=null)
        {
            action(keyDownTime[key]);
        }
    }

    public float SurplusTime(KeyCode key) {
        //总的配置时间-按下到现在已经过去的时间 =剩余的冷却时间
        float time = coolingConfig[key] - (Time.time - keyDownTime[key]);
        if (time<=0)
        {
            time = 0;
        }
        return time;
    }

    public bool IsCooling(KeyCode key) {
        return SurplusTime(key) > 0;
    }
    public int GetID(KeyCode key) {
        return skillID[key];
    }
   
   
}
