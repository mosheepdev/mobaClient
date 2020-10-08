using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : Singleton<ResManager>
{
    //加载UI窗体
    public GameObject LoadUI(string path) {
        GameObject go=   Resources.Load<GameObject>($"UIPrefab/{path}");
        if (go==null)
        {
            Debug.LogError($"没有找到UI窗体:{path}");
            return null;
        }
        GameObject obj= GameObject.Instantiate(go);
        return obj;
    }


    //加载圆形头像
    public Sprite LoadRoundHead(string path) {
       return Resources.Load<Sprite>($"Image/Round/{path}");
    }

    //加载英雄原画
    public Sprite LoadHeroTexture(int heroID)
    {
        return Resources.Load<Sprite>($"Image/HeroTexture/{heroID}");
    }

    //加载召唤师技能
    public Sprite LoadGeneralSkill(int skillID)
    {
        return Resources.Load<Sprite>($"Image/GeneralSkill/{skillID}");
    }
    //Assets/Resources/Hero/1001/Model/1001.prefab
    public GameObject LoadHero(int heroID) {
        heroID = 1001;
        GameObject go = Resources.Load<GameObject>($"Hero/{heroID}/Model/{heroID}");

        return GameObject.Instantiate(go);
    }


    public GameObject LoadHUD()
    {
        GameObject go = Resources.Load<GameObject>($"HUD/HeroHead");
        return GameObject.Instantiate(go);
    }

    public GameObject LoadEffect(int heroID,string skillName) {
        GameObject go = Resources.Load<GameObject>($"Hero/{heroID}/Effect/{skillName}");
        return GameObject.Instantiate(go);
    }

    public Sprite LoadHeroSkill(int heroID, string skillName) {
       return Resources.Load<Sprite>($"Hero/{heroID}/UI_Skill/{skillName}");
    }

    internal Sprite LoadHeadIcon(int heroID)
    {
        return Resources.Load<Sprite>($"Hero/{heroID}/UI_Head/{heroID}");
    }
}
