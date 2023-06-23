using ComputerInterface;
using ComputerInterface.Interfaces;
using ComputerInterface.ViewLib;
using System;
using System.Text;
using UnityEngine;

namespace DisplayNames.Interface.Views
{
    internal class SetNameView : ComputerView
    {
        internal class Entry : IComputerModEntry
        {
            public string EntryName => "Display Name";

            public Type EntryViewType => typeof(SetNameView);
        }

        private UITextInputHandler textInputHandler;
        private const string GrayColorHex = "#ffffff50";
        private bool CapsLock = false;
        private bool SpecialLock = false;

        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            textInputHandler = new UITextInputHandler();
            textInputHandler.Text = Main.Instance.CustomName;

            DrawPage();
        }

        private void DrawPage()
        {
            StringBuilder stringBuilder = new StringBuilder();

            //// Header
            stringBuilder
                .BeginCenter()
                .MakeBar('=', SCREEN_WIDTH, 0)
                .AppendLines(1)
                .AppendLine(Main.NAME)
                .AppendLine($"<color={GrayColorHex}>Press Enter to save or Back to exit</color>")
                .AppendLine($"<color={GrayColorHex}>Press Option 1 to toggle capslock</color>")
                .AppendLine($"<color={GrayColorHex}>Press Option 2 to toggle speciallock</color>")
                .MakeBar('=', SCREEN_WIDTH, 0)
                .EndAlign()
                ;

            //// Body
            stringBuilder
                .AppendLines(2)
                .Append($"Your Name:<color={GrayColorHex}>" + textInputHandler.Text)
                ;

            //// Footer
            int Lines = stringBuilder.ToString().Length / SCREEN_WIDTH;
            stringBuilder
                .AppendLines(SCREEN_HEIGHT - Lines - 1)
                .BeginAlign("right")
                .Append($"{textInputHandler.Text.Length}/" + Main.MaxCharacters)
                ;

            SetText(stringBuilder);
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            string keyString = key.ToString();
            if (textInputHandler.HandleKey(key))
            {
                if (!CapsLock && key != EKeyboardKey.Delete && key != EKeyboardKey.Space && key > EKeyboardKey.NUM9)
                {
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + keyString.ToLower();
                }
                if (SpecialLock)
                {
                    HandleKeyToSpecial(key);
                }
                if (textInputHandler.Text.Length > Main.MaxCharacters) 
                {
                    textInputHandler.Text = textInputHandler.Text.Substring(0, Main.MaxCharacters);
                }
                DrawPage();
            }
            
            else if (key == EKeyboardKey.Back)
            {
                ReturnToMainMenu();
            }
            else if (key == EKeyboardKey.Enter)
            {
                Main.Instance.CustomName = textInputHandler.Text;
                PlayerPrefs.SetString("Nickname", textInputHandler.Text);
                Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { Main.Instance.ChannelId, Main.Instance.CustomName } });
                BaseGameInterface.SetColor(BaseGameInterface.GetColor());
            }
            else if (key == EKeyboardKey.Option1)
            {
                CapsLock = !CapsLock;
                SpecialLock = false;
            }
            else if (key == EKeyboardKey.Option2)
            {
                SpecialLock = !SpecialLock;
                CapsLock = false;
            }
        }
        private void HandleKeyToSpecial(EKeyboardKey key)
        {
            switch (key)
            {
                case EKeyboardKey.NUM1:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + "!";
                    return;
                case EKeyboardKey.NUM2:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + "@";
                    return;
                case EKeyboardKey.NUM3:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + "#";
                    return;
                case EKeyboardKey.NUM4:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + "$";
                    return;
                case EKeyboardKey.NUM5:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + "%";
                    return;
                case EKeyboardKey.NUM6:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + "^";
                    return;
                case EKeyboardKey.NUM7:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + "&";
                    return;
                case EKeyboardKey.NUM8:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + "*";
                    return;
                case EKeyboardKey.NUM9:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + "(";
                    return;
                case EKeyboardKey.NUM0:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + ")";
                    return;
                case EKeyboardKey.H:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + ":";
                    return;
                case EKeyboardKey.J:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + ";";
                    return;
                case EKeyboardKey.K:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + "'";
                    return;
                case EKeyboardKey.L:
                    textInputHandler.Text = textInputHandler.Text.Remove(textInputHandler.Text.Length - 1, 1) + '"';
                    return;
            }
        }
    }
}
