using HarmonyLib;
using UnityEngine;
using ScoreboardAttributes;
using Photon.Pun;
using GorillaNetworking;
using UnityEngine.UI;

namespace DisplayNames
{
    [HarmonyPatch]
    internal class Patches
    {
        public static GorillaScoreBoard scoreboardInstance;
        [HarmonyPatch(typeof(GorillaLocomotion.Player), "Awake"), HarmonyPostfix]
        private static void GameInitialized()
        {
            new GameObject("Callbacks").AddComponent<Behaviours.Callbacks>();
            Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { Main.Instance.ChannelId, Main.Instance.CustomName } });
        }

        // experimental support for using actual scoreboard lines rather than
        // a replacement string builder lives on the playerlines branch.
        // For now a replacement string builder is less buggy and looks
        // more like the base game scoreboard so that's what I'm using
        [HarmonyPatch(typeof(GorillaScoreBoard), "RedrawPlayerLines"), HarmonyPrefix]
        private static bool RedrawLines(GorillaScoreBoard __instance)
        {
            scoreboardInstance = __instance;
            __instance.lines.Sort((GorillaPlayerScoreboardLine line1, GorillaPlayerScoreboardLine line2) => line1.playerActorNumber.CompareTo(line2.playerActorNumber));
            __instance.boardText.text = __instance.GetBeginningString();
            __instance.buttonText.text = "";
            for (int i = 0; i < __instance.lines.Count; i++)
            {
                try
                {
                    __instance.lines[i].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, (float)(__instance.startingYValue - __instance.lineHeight * i), 0f);
                    if (__instance.lines[i].linePlayer != null)
                    {
                        Text text = __instance.boardText;
                        if (__instance.lines[i].linePlayer.CustomProperties.ContainsKey(Main.Instance.ChannelId))
                        {
                            if (GorillaComputer.instance.CheckAutoBanListForName(TruncateString((string)__instance.lines[i].linePlayer.CustomProperties[Main.Instance.ChannelId])))
                            {
                                text.text = text.text + "\n " + TruncateString((string)__instance.lines[i].linePlayer.CustomProperties[Main.Instance.ChannelId]);
                            }
                            else { text.text = text.text + "\n " + "Bitch"; }
                            PlayerTexts.RegisterAttribute(__instance.lines[i].linePlayer.NickName, __instance.lines[i].linePlayer);
                        }
                        else
                        {
                            text.text = text.text + "\n " + __instance.NormalizeName(true, __instance.lines[i].linePlayer.NickName);
                        }

                        if (__instance.lines[i].linePlayer != PhotonNetwork.LocalPlayer)
                        {
                            if (__instance.lines[i].reportButton.isActiveAndEnabled)
                            {
                                Text text2 = __instance.buttonText;
                                text2.text += "MUTE                                REPORT\n";
                            }
                            else
                            {
                                Text text3 = __instance.buttonText;
                                text3.text += "MUTE                HATE SPEECH    TOXICITY      CHEATING      CANCEL\n";
                            }
                        }
                        else
                        {
                            Text text4 = __instance.buttonText;
                            text4.text += "\n";
                        }
                    }
                }
                catch
                {
                }
            }
            return false;
        }

        [HarmonyPatch(typeof(VRRig), "InitializeNoobMaterialLocal"), HarmonyPostfix]
        private static void VRRigPatch(VRRig __instance)
        {

            if (!__instance.isOfflineVRRig)
            {
                PhotonView VRRigPhotonView = (PhotonView)AccessTools.Field(__instance.GetType(), "photonView").GetValue(__instance);
                if (!VRRigPhotonView.Owner.CustomProperties.TryGetValue(Main.Instance.ChannelId, out object value))
                    return;
                __instance.playerText.text = GorillaComputer.instance.CheckAutoBanListForName(TruncateString((string)value)) ? TruncateString((string)value) : "Bitch";
            }
            else
            {
                __instance.playerText.text = TruncateString(Main.Instance.CustomName);
            }
        }

        private static string TruncateString(string str)
        {
            if (str.Length > Main.MaxCharacters)
            {
                return str.Substring(0, Main.MaxCharacters);
            } else return str;
        }
    }
}
