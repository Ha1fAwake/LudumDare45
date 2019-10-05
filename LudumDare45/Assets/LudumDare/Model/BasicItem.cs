using System;
using ReadyGamerOne.Const;
using UnityEngine;
#if UNITY_EDITOR

#endif
namespace LudumDare.Model
{
    [Serializable]
    public class BasicItem//:ScriptableObject
    {
            public int id;
            public ConstStringChooser itemName;
            public string descriptions;
            public Sprite sprite;
            public string ItemName => itemName.StringValue;
            public override string ToString()
            {
                var text = "===<BaseItem>===\n";
                text += "ItemName: " + itemName + "\n";
                text += "Description: " + descriptions + "\n";
                text += "Sprite: " + sprite.name + "\n";
                return text;
            }
    }
}