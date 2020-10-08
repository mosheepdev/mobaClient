﻿using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EMove : MonoBehaviour
{
   
    void FixedUpdate()
    {
        if (eConfig!=null)
        {
            if (eConfig.moveSpeed==0)
            {
                return;
            }
            //根据方向进行移动
            if (eConfig.moveType== MoveType.DirectMove)
            {
                //正前方*速度* Time.deltaTime 
                transform.position += (transform.forward) * (eConfig.moveSpeed * Time.deltaTime);
            }
            //锁定目标 进行追踪移动
            else
            {
                //如果距离小于等于0.01 追踪到敌方了
                //否则就是还未追踪到
                if (Vector3.Distance(eConfig.trackObject.position, transform.position) >= 0.01f)
                {
                    //物体朝向要追踪的物体
                    transform.transform.LookAt(eConfig.trackObject);
                    //正前方*速度
                    transform.position += (transform.forward) * (eConfig.moveSpeed * Time.deltaTime);
                }
            }
        }
        
       
    }

    EConfig eConfig;
    internal void Init(EConfig eConfig)
    {
        this.eConfig = eConfig;
    }
}
