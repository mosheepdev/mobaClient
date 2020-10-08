using System;
using System.Collections;
using System.Collections.Generic;
using Game.Model;
using ProtoMsg;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerCtrl : MonoBehaviour
{

    public SkillManager skillManager;
   public AnimatorManager animatorManager;
   public PlayerFSM playerFSM;

    Vector3 spawnPosition;
    Vector3 spawnRotaiton;
    public PlayerInfo playerInfo;
    bool isSelf = false;
    public HeroAttributeEntity currenAttribute;
    public HeroAttributeEntity totalAttribute;

    internal void Relive()
    {
        this.transform.position = spawnPosition;
        this.transform.eulerAngles = spawnRotaiton;
  
        //恢复血量和蓝量
        currenAttribute.HP = totalAttribute.HP;
        currenAttribute.MP = totalAttribute.MP;
        HUDUpdate();
    }

    GameObject HUD;

    Vector3 hudOffset = new Vector3(0,3.1f,0);

    Text HPText, MPText, LevelText, NickNameText;
    Image HPFill, MPFill;

    internal void Init(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
        //英雄是不是自己的角色
        isSelf=PlayerModel.Instance.CheckIsSelf(playerInfo.RolesInfo.RolesID);

        //复活的时候 会用到
        spawnPosition = transform.position;
        spawnRotaiton = transform.eulerAngles;

        //获取它的属性 当前的属性 还有总的属性
        currenAttribute= HeroAttributeConfig.GetInstance(playerInfo.HeroID);
        totalAttribute= HeroAttributeConfig.GetInstance(playerInfo.HeroID);

        RoomModel.Instance.SaveHeroAttribute(playerInfo.RolesInfo.RolesID,currenAttribute, totalAttribute);

        //人物的HUD 血条 蓝条 昵称 等级
        HUD= ResManager.Instance.LoadHUD();
        HUD.transform.position = transform.position + hudOffset;
        HUD.transform.eulerAngles = Camera.main.transform.eulerAngles;

        HPFill = HUD.transform.Find("HP/Fill").GetComponent<Image>();
        MPFill = HUD.transform.Find("MP/Fill").GetComponent<Image>();

        HPText = HUD.transform.Find("HP/Text").GetComponent<Text>();
        MPText = HUD.transform.Find("MP/Text").GetComponent<Text>();
        NickNameText = HUD.transform.Find("NickName").GetComponent<Text>();
        LevelText = HUD.transform.Find("Level/Text").GetComponent<Text>();
        HUDUpdate(true);


        //技能管理器
        skillManager= this.gameObject.AddComponent<SkillManager>();
        skillManager.Init(this);

        //动画管理器
        animatorManager = this.gameObject.AddComponent<AnimatorManager>();
        animatorManager.Init(this);

        //角色的状态机 FSM 
        playerFSM = this.gameObject.AddComponent<PlayerFSM>();
        playerFSM.Init(this);


        //相机的跟随->相机的脚本里去做
        if (isSelf==true)
        {
            
            if (playerInfo.TeamID==0)
            {
                Camera.main.transform.eulerAngles = new Vector3(45,180,0);
            }
            else
            {
                Camera.main.transform.eulerAngles = new Vector3(45, -180, 0);
            }
        }

    }

   public void HUDUpdate(bool init=false) {

        HPText.text = $"{currenAttribute.HP}/{totalAttribute.HP}";
        MPText.text = $"{currenAttribute.MP}/{totalAttribute.MP}";
        LevelText.text = currenAttribute.Level.ToString();
        NickNameText.text = playerInfo.RolesInfo.NickName;

        if (init==true)
        {
            HPFill.fillAmount = 1;
            MPFill.fillAmount = 1;
        }
        else
        {
            HPFill.DOFillAmount(currenAttribute.HP / totalAttribute.HP, 0.2f).SetAutoKill(true);
            MPFill.DOFillAmount(currenAttribute.MP / totalAttribute.MP, 0.2f).SetAutoKill(true);
        }
    }

    Vector3 cameraOffset = new Vector3(0,11,10);
    public void LateUpdate()
    {
        if (HUD != null)
        {
            HUD.transform.position = transform.position + hudOffset;
        }

        if (isSelf)
        {
            Camera.main.transform.position = this.transform.position + cameraOffset;
        }
    }

  /// <summary>
  /// 碰撞到技能触发器
  /// </summary>
  /// <param name="eConfig"></param>
  /// <param name="trrigerObject"></param>
    public void OnSkillTrriger(EConfig eConfig,GameObject trrigerObject) {
        bool isDestroy = false ;
        //角色
        if (trrigerObject.CompareTag("Player"))
        {
            PlayerCtrl hitPlayerCtrl= trrigerObject.transform.GetComponent<PlayerCtrl>();
          PlayerInfo hitPlayerInfo = hitPlayerCtrl.playerInfo;
            //判断是否同个阵营的
            if (hitPlayerInfo.TeamID!=playerInfo.TeamID)
            {
                hitPlayerCtrl.OnSkillHit(50);
                if (eConfig.destroyMode == DestroyMode.OnHit_DifferentCampPlayer || eConfig.destroyMode == DestroyMode.OnHit_AllPlayer)
                {
                    isDestroy = true;
                  
                }
            }
            else
            {
                //如果是同个阵营的
                return;
            }
        }
        ////野怪
        //else if (other.CompareTag("Monster"))
        //{
        //    //让他扣血 并且让它进入对应的状态
        //}
        ////兵
        //else if (other.CompareTag("Soldier"))
        //{
        //    //如果不是同个阵营 让他扣血 并且让它进入对应的状态
        //}
        ////防御塔或者水晶
        //else if (other.CompareTag("Tower"))
        //{
        //    //如果不是同个阵营 让他扣血 并且让它进入对应的状态
        //}
        ////水晶
        //else if (other.CompareTag("Crystal"))
        //{
        //    //如果不是同个阵营 让他扣血 并且让它进入对应的状态
        //}

        //克隆爆炸特效
        if (isDestroy && eConfig.hitLoad != null)
        {
            //克隆爆炸物
            GameObject hitObj = GameObject.Instantiate(eConfig.hitLoad);
            //hitObj.transform.position = trrigerObject.transform.position;
            hitObj.transform.position = eConfig.moveRoot.gameObject.transform.position;
            //并且销毁
            Destroy(eConfig.gameObject);
        }
    }

    /// <summary>
    /// 受到技能伤害 要减血
    /// </summary>
    /// <param name="hurt"></param>
    private void OnSkillHit(int hurt)
    {
        currenAttribute.HP -= hurt;
        if (currenAttribute.HP<=0)
        {
            currenAttribute.HP = 0;
            HUDUpdate();
            //进入到死亡状态
            playerFSM.ToNext(FSMState.Dead);
        }
        else
        {
            HUDUpdate();
        }
    }

    void Update()
    {
      
    }

   
}
