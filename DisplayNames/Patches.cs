using HarmonyLib;
using UnityEngine;
using ScoreboardAttributes;
using Photon.Pun;
using GorillaNetworking;
using Photon.Realtime;

namespace DisplayNames
{
    [HarmonyPatch]
    internal class Patches
    {
        [HarmonyPatch(typeof(GorillaLocomotion.Player), "Awake"), HarmonyPostfix]
        private static void GameInitialized()
        {
            new GameObject("Callbacks").AddComponent<Behaviours.Callbacks>();
            Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { Main.Instance.ChannelId, Main.Instance.CustomName } });
        }
        
        [HarmonyPatch(typeof(GorillaScoreBoard), "RedrawPlayerLines"), HarmonyPostfix]
        private static void RedrawLines(GorillaScoreBoard __instance)
        {
            foreach (GorillaPlayerScoreboardLine line in __instance.lines)
            {
                if (!line.linePlayer.CustomProperties.TryGetValue(Main.Instance.ChannelId, out object value))
                    return;
                line.playerName.text = (string)value;
                ScoreboardAttributes.PlayerTexts.RegisterAttribute(line.linePlayer.NickName, line.linePlayer);
            }
        }

        [HarmonyPatch(typeof(VRRig), "InitializeNoobMaterialLocal"), HarmonyPostfix]
        private static void VRRigPatch(VRRig __instance)
        {
            if (!__instance.isOfflineVRRig) 
            { 
                PhotonView VRRigPhotonView = (PhotonView)AccessTools.Field(__instance.GetType(), "photonView").GetValue(__instance);
                if (!VRRigPhotonView.Owner.CustomProperties.TryGetValue(Main.Instance.ChannelId, out object value))
                    return;
                __instance.playerText.text = GorillaComputer.instance.CheckAutoBanListForName((string)value) ? (string)value : "Bitch";
            }
            else
            {
                __instance.playerText.text = Main.Instance.CustomName;
            }
        }
    }
}
