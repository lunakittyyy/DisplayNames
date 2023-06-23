using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace DisplayNames.Behaviours
{
    internal class Callbacks : MonoBehaviourPunCallbacks
    {
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            try
            {
                base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
                if (!changedProps.TryGetValue(Main.Instance.ChannelId, out object value))
                    return;

                Main.Instance.manualLogSource.LogInfo($"Player {targetPlayer.NickName} has changed their display name to {value}");
            } catch {}
        }
    }
}
