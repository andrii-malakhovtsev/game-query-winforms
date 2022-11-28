using System;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public class MessageController
    {
        private readonly MainForm _mainForm;
        private readonly static string s_confirmationDialogTitle = "Confirmation";

        public MessageController(MainForm mainform)
        {
            _mainForm = mainform;
        }

        private void ShowMainFormMessage(string message)
        {
            _mainForm.MyMessageBox.Show(message);
        }

        private void ShowOptionalMainFormMessage(string message)
        {
            if (_mainForm.ShowMessages) ShowMainFormMessage(message);
        }

        public void ShowTurningConfirmationMessagesError()
        {
            ShowMainFormMessage("You can't turn showing confirmation messages off while showing " +
                "(all) messages are on");
        }

        public void ShowNoGamesToPlayMessage()
        {
            ShowMainFormMessage("You don't have games to play (Bad ending)");
        }

        private string GetRandomGameToPlayPhrase(string gameToPlay)
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 6);
            switch (randomNumber)
            {
                case 1: return "Let's go to play " + gameToPlay + "!";
                case 2: return "Yoooo, is it " + gameToPlay + " that we gonna play?";
                case 3: return "The best choice for today is... " + gameToPlay + "!";
                case 4: return "No way we are going to play " + gameToPlay + " rn!";
                case 5: return "Hey chads, we are going to " + gameToPlay + "!";
            }
            return string.Empty;
        }

        public void ShowGameToPlayMessage(string gameToPlay)
        {
            ShowMainFormMessage(GetRandomGameToPlayPhrase(gameToPlay));
        }

        public void ShowPlayersLimitsErrorMessage()
        {
            ShowMainFormMessage("The Min value must not exceed the Max value");
        }

        public void ShowGameAddedToListMessage(string gameName)
        {
            ShowOptionalMainFormMessage("Game " + gameName + " is successfully added!");
        }

        public void ShowPlayerAddedToListMessage(string playerName)
        {
            ShowOptionalMainFormMessage(playerName + " is added to the list!");
        }

        public static bool ShowFirstMeetingDialog()
        {
            DialogResult dialogResult = MessageBox.Show("Seems like you are using " +
                "the program for the first time.\nIt does create files in the same path as it is located!" +
                "\nMake sure it is in the separate folder. Continue?", s_confirmationDialogTitle,
                MessageBoxButtons.YesNo);
            return dialogResult == DialogResult.Yes;
        }

        private bool ShowDeleteConfirmationDialog(string objectToDelete, string listName)
        {
            DialogResult dialogResult = _mainForm.MyMessageBox.Show("Are you sure you want to delete " +
                objectToDelete + " from " + listName + " list?",
                s_confirmationDialogTitle, MessageBoxButtons.YesNo);
            return dialogResult == DialogResult.Yes;
        }


        public bool ShowDeleteAvailableGameDialog(string gameName)
        {
            return ShowDeleteConfirmationDialog(gameName, listName: "available games");
        }

        public bool ShowDeleteGameDialog(string gameName)
        {
            return ShowDeleteConfirmationDialog(gameName, listName: "games");
        }

        public bool ShowDeletePlayersLimitsFileDialog(string gameName)
        {
            return ShowDeleteConfirmationDialog(gameName, listName: "limits");
        }

        public bool ShowDeletePlayerFromListDialog(string selectedPlayer)
        {
            return ShowDeleteConfirmationDialog(selectedPlayer, listName: "player");
        }
    }
}
