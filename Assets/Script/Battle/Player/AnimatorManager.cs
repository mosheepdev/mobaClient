using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimationClip
{
    None,
    Idle,
    Run,
    Dance,
    Dead,
    SkillQ,
    SkillW,
    SkillE,
    SkillR,
    NormalAttack,
}

public class AnimatorManager : MonoBehaviour
{
    PlayerCtrl playerCtrl;
    PlayerInfo playerInfo;
    Animator animator;
    public void Init(PlayerCtrl playerCtrl)
    {
        this.playerCtrl = playerCtrl;
        playerInfo = playerCtrl.playerInfo;
        animator=transform.GetComponent<Animator>();
    }


    //播放动画 
    public void Play(PlayerAnimationClip clip) {
        ResetState();
        animator.SetBool(clip.ToString(), true);
    }

    public string[] clips = new string[] { "None","Idle","Run", "Dead" , "SkillQ",
        "SkillW","SkillE","SkillR","NormalAttack"};
    //重置状态
    void ResetState() {
        for (int i = 0; i < clips.Length; i++)
        {
            animator.SetBool(clips[i], false);
        }
    }

    //Q事件
    public void DoSkillQEvent() {
        SpawnEffect("Q");
    }

    //W事件
    public void DoSkillWEvent()
    {
        SpawnEffect("W");
    }

    //E
    public void DoSkillEEvent()
    {
        SpawnEffect("E");
    }

    //R
    public void DoSkillREvent()
    {
        SpawnEffect("R");
    }

    //A
    public void DoSkillAEvent()
    {
        SpawnEffect("A");
    }


    //生成特效
    public void SpawnEffect(string key) {

      GameObject effect=  ResManager.Instance.LoadEffect(playerInfo.HeroID, key);
        effect.transform.position = transform.position;
        effect.transform.eulerAngles = transform.eulerAngles;
        effect.gameObject.SetActive(true);
        EConfig eConfig = effect.transform.GetComponent<EConfig>();
        BattleUserInputC2S skillCMD= playerCtrl.playerFSM.skillCMD.CMD;
        //playerCtrl.OnSkillTrriger 技能触发回调 由释放者
        eConfig.Init(skillCMD.RolesID, skillCMD.LockTag, skillCMD.LockID,
            transform.forward, transform.position,playerCtrl.OnSkillTrriger);

    }

    //技能结束的事件
    public void EndSkill() {
        Debug.Log("技能释放结束!");
        playerCtrl.playerFSM.ToNext(FSMState.Idle);
        
    }

}
