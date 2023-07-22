using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace DisplayNames
{
    [BepInPlugin(ID, NAME, VERSION)]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            ID = "luna.displaynames",
            NAME = "Display Names",
            VERSION = "1.1.0";

        internal const int MaxCharacters = 20;
        internal static Main Instance;

        internal ManualLogSource manualLogSource => Logger;
        internal string CustomName = "Nickname";
        internal string ChannelId = "DisplayName";

        internal Main()
        {
            Instance = this;
            CustomName = PlayerPrefs.GetString("Nickname", "Nickname");

            Bepinject.Zenjector.Install<Interface.MainInstaller>().OnProject();
            new HarmonyLib.Harmony(ID).PatchAll();
        }
    }
}
