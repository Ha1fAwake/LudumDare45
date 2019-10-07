using System;
using ReadyGamerOne.Common;
using ReadyGamerOne.Const;
using ReadyGamerOne.Script;
using UnityEngine;

namespace LudumDare.Scripts
{
    /// <summary>
    /// 炸弹信息
    /// 已出现就开始计时
    /// </summary>
    [RequireComponent(typeof(Timer))]
    public class BoomIdentity :ItemIdentity
    {
        public ConstStringChooser messagePlayDead;
        public float timeToBoom;
        private Timer timer;
        

        public override void OnAddToBag()
        {
            base.OnAddToBag();
            
        }

        private void Start()
        {
            
            timer = GetComponent<Timer>();
            timer.StartTimer(timeToBoom);
            MainLoop.Instance.ExecuteLater(OnBoom, timeToBoom);
        }

        private void OnBoom()
        {
            Debug.Log("炸弹爆炸");
            if (IsInBag)
                CEventCenter.BroadMessage(messagePlayDead.StringValue);
        }
    }
}