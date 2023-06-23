using HarmonyLib;
using UnityEngine;
using ScoreboardAttributes;
using Photon.Pun;
using GorillaNetworking;
using Photon.Realtime;
using System.Threading.Tasks;

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
            => RedrawLinesInternal(__instance);

        [HarmonyPatch(typeof(GorillaScoreBoard), "InfrequentUpdate"), HarmonyPostfix]
        private static void InfreqUpdate(GorillaScoreBoard __instance)
            => RedrawLinesInternal(__instance);

        internal static void RedrawLinesInternal(GorillaScoreBoard __instance)
        {
            __instance.boardText.text = __instance.GetBeginningString();
            foreach (GorillaPlayerScoreboardLine line in __instance.lines)
            {
                if (!line.playerName.gameObject.activeSelf)
                    line.playerName.gameObject.SetActive(true);

                if (!line.linePlayer.CustomProperties.TryGetValue(Main.Instance.ChannelId, out object value) && !line.linePlayer.IsLocal)
                    continue;

                string playerText = !line.linePlayer.IsLocal ? (string)value : Main.Instance.CustomName;
                if (playerText.Length > 12)
                    playerText = playerText.Substring(0, 10) + "..";

                line.playerName.text = playerText;
                ScoreboardAttributes.PlayerTexts.RegisterAttribute(line.linePlayer.NickName, line.linePlayer);
            }
        }

        [HarmonyPatch(typeof(VRRig), "Start"), HarmonyPostfix]
        private async static void VRRigPatch(VRRig __instance)
        {
            await Task.Delay(400);
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
