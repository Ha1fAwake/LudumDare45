using System;
using System.Collections.Generic;
using LudumDare.Model;
using ReadyGamerOne.Common;
using ReadyGamerOne.Const;
using ReadyGamerOne.EditorExtension;
using ReadyGamerOne.Script;
using UnityEngine;

namespace LudumDare.Scripts
{
    [RequireComponent(typeof(Timer))]
    public class FlowerIdentity:EatableIdentity
    {
        public ConstStringChooser playerDeadMessage;
        public AnimationNameChooser chewingAni;
        public AnimationNameChooser bitAni;
        public ConstStringChooser chewingFlower;
        public bool canEat = true;
        [Header("可以吃的物品ID")]
        public List<int> acceptableItemIds=new List<int>();
        [Header("消化时间")]
        public float eattingTime;

        public float chewingFlowerSprite;

        public float attackTime = 0.2f;
        public GameObject attacker;

        private Animator ani;

        public override BasicItem ItemInfo
        {
            get { return canEat ? ItemMgr.GetItem(itemName.StringValue) : ItemMgr.GetItem(chewingFlower.StringValue); }
        }

        public void AttackTrigger()
        {
            attacker.SetActive(true);
            MainLoop.Instance.ExecuteLater(()=>attacker.SetActive(false), attackTime);
        }

        protected override void Start()
        {
            base.Start();
            ani = GetComponent<Animator>();
        }

        /// <summary>
        /// 添加物品到背包回调
        /// </summary>
        public override void OnAddToBag()
        {
            base.OnAddToBag();
            if (canEat)
            {
                //你完了，这食人花都敢拿
                Debug.Log("把危险食人花拿走");
                CEventCenter.BroadMessage(playerDeadMessage.StringValue);
            }
        }

        private Timer timer;
        
        /// <summary>
        /// 尝试吃某个物品
        /// 如果物品在可吃列表就会吃，否则返回False
        /// 这函数应该在和食人花喂食的时候调用
        /// </summary>
        /// <param name="id"></param>
        public bool TryStartEating(int id)
        {
            if (!canEat)
                return false;
            if (!acceptableItemIds.Contains(id))
                return false;

            canEat = false;
            ani.Play(Animator.StringToHash(chewingAni.StringValue));
            timer = GetComponent<Timer>();
                timer.StartTimer(eattingTime);
                MainLoop.Instance.ExecuteLater(OnFinishEating, eattingTime);
            return true;
        }

        /// <summary>
        /// 结束消化回调
        /// </summary>
        private void OnFinishEating()
        {
            if (this == null)
                return;
            
            if (IsInBag)
            {
                //贪吃蛇居然还在身上，你完了
                CEventCenter.BroadMessage(playerDeadMessage.StringValue);
            }

            canEat = true;
            ani.Play(Animator.StringToHash(bitAni.StringValue));
        }


        protected override void OnEat()
        {
            base.OnEat();
            if(timer)
                Destroy(timer.timerObj);
            timer = null;
        }
    }
}