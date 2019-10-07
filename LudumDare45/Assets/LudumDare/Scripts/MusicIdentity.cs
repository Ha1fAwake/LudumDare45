using LudumDare.Scripts;
using ReadyGamerOne.Common;
using ReadyGamerOne.Const;

namespace UnityEngine.UI
{
    public class MusicIdentity : EatableIdentity
    {
        public ConstStringChooser messageToShot;
        public override void UseOnTo(ItemIdentity identity)
        {
            if (identity)
                return;

            CEventCenter.BroadMessage(messageToShot.StringValue);
        }
    }
}