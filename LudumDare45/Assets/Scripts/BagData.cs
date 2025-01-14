﻿/*背包数据处理类*/
using UnityEngine;
using LudumDare.Model;
using UnityEngine.SceneManagement;
using ReadyGamerOne.Script;
using LudumDare.Scripts;

public class BagData {

    public enum Direction {
        up,
        down,
        left,
        right
    };
    private static int bagItemId = 0;  //背包物品的id
    private static int facedItemId;
    private static GameObject bagItem;   //背包物品
    private static GameObject facedItem;
    private static Direction playerFace = Direction.down;  //主角朝向
    private static Vector3 farAway = new Vector3(999, 999, 0);  //背包位置

    public static int BagItemId { get => bagItemId; set => bagItemId = value; }
    public static int FacedItemId { get => facedItemId; set => facedItemId = value; }
    public static GameObject BagItem { get => bagItem; set => bagItem = value; }
    public static GameObject FacedItem { get => facedItem; set => facedItem = value; }
    public static Direction PlayerFace { get => playerFace; set => playerFace = value; }

    //交换或拾取
    public static void SwitchItem(Transform Player) {

        //无效操作
        if (facedItem == null && bagItem == null) {
            AudioMgr.Instance.PlayEffect(AudioName.Error);
            return;
        }

        //扔掉物体
        if (facedItem == null) {
            AudioMgr.Instance.PlayEffect(AudioName.Ok);
            #region 扔掉

            bagItem.GetComponent<ItemIdentity>().OnLeaveBag();
            if (playerFace == Direction.up) {
                bagItem.transform.position = Player.position + new Vector3(0, 1, 0);
            }
            if (playerFace == Direction.down) {
                bagItem.transform.position = Player.position + new Vector3(0, -1, 0);
            }
            if (playerFace == Direction.left) {
                bagItem.transform.position = Player.position + new Vector3(-1, 0, 0);
            }
            if (playerFace == Direction.right) {
                bagItem.transform.position = Player.position + new Vector3(1, 0, 0);
            }
            bagItem = null;
            bagItemId = 0;  //nothing
            
            #endregion
            return;
        }

        //获得物体
        if (bagItem == null) {
            
            if (facedItemId == 18) {    //女神
                AudioMgr.Instance.PlayEffect(AudioName.Error);
                return;
            }
            AudioMgr.Instance.PlayEffect(AudioName.Ok);
            bagItem = facedItem;
            facedItem.transform.position = farAway;
            bagItemId = facedItemId;
            bagItem.GetComponent<ItemIdentity>().OnAddToBag();
            return;
        }

        //交换物体
        if(ItemMgr.IsExchangeOk(bagItemId, facedItemId)) {
            AudioMgr.Instance.PlayEffect(AudioName.Ok);
            if (bagItemId == 3 && facedItemId == 7) {   //奶牛和魔豆
                ItemMgr.GetItem(facedItemId).exchangeCondition = null;
            }
            if (facedItemId == 18) {    //女神
                AudioMgr.Instance.PlayEffect(AudioName.Error);
                return;
            }
            bagItem.GetComponent<ItemIdentity>().OnLeaveBag();
            facedItem.GetComponent<ItemIdentity>().OnAddToBag();
            bagItem.transform.position = facedItem.transform.position;
            facedItem.transform.position = farAway;
            bagItem = facedItem;
            bagItemId = facedItemId;
        }
        return;
    }

    public static void MergeItem() {
        AudioMgr.Instance.PlayEffect(AudioName.Ok);
        if (facedItem != null && bagItem != null) {
            if (bagItemId == 7 && facedItemId == 5) {  //合成魔豆和井，胜利！
                if (Vector3.Distance(facedItem.transform.position, new Vector3(6.5f, 6.5f, 0)) <= 1.0f) {
                    {
                        //把条件放回去
                        ItemMgr.GetItem(7).exchangeCondition = "3";
                        
                        SceneManager.LoadScene("Animation");
                        return;
                    }
                }
                else {
                    return;
                }
            }
            BasicItem item;
            if (ItemMgr.IsMergeOk(bagItemId, facedItemId, out item)) {
                GameObject.Instantiate(item.Prefab, facedItem.transform.position, new Quaternion());
                GameObject.Destroy(bagItem);
                GameObject.Destroy(facedItem);
                bagItemId = 0;
            }
        }
    }

    public static void UseItem() {
        if (bagItem == null) {
            AudioMgr.Instance.PlayEffect(AudioName.Error);
            return;
        }
        if (facedItem == null) {

            bagItem.GetComponent<ItemIdentity>().UseOnTo(null);
        }
        else {
            bagItem.GetComponent<ItemIdentity>().UseOnTo(facedItem.GetComponent<ItemIdentity>());
        }
    }

    public static void ClearBag() {
        bagItem = null;
        BagItemId = 0;
    }

}