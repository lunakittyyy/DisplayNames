using Photon.Realtime;
using HarmonyLib;
using Photon.Pun;

namespace DisplayNames.Utils
{
    internal class RigTools
    {
        // thx dev <3
        public static PhotonView GetViewFromRig(VRRig vrRig)
        {
            if (!vrRig.isOfflineVRRig && Traverse.Create(vrRig).Field("creator").GetValue() is Player rigOwner && GorillaGameManager.instance != null)
            {
                return GorillaGameManager.instance.FindVRRigForPlayer(rigOwner);
            }

            return null;
        }
    }
}
