using System;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public class MessageController
    {
        private readonly MainForm _mainForm;
        private readonly static string _confirmationDialogTitle = "Confirmation";

        public MessageController(MainForm mainform)
        {
            _mainForm = mainform;
        }

        public void ShowRestrictionsErrorMessage()
        {
            ShowMainFormMessage("The Min value must not exceed the Max value");
        }

        public void ShowPeopleLimitSetMessage(string gameName)
        {
            ShowOptionalMainFormMessage("Limits for " + gameName + " set!");
        }

        public void ShowGameAddedMessage(string gameName)
        {
            ShowOptionalMainFormMessage("Game " + gameName + " is successfully added!");
        }

        public void ShowGameListSetForPlayerMessage(string playerName)
        {
            ShowOptionalMainFormMessage("Games list for " + playerName + " set!");
        }

        public void ShowPlayerAddedToListMessage(string playerName)
        {
            ShowOptionalMainFormMessage(playerName + " is added to the list!");
        }

        public void ShowNoGamesToPlayMessage()
        {
            ShowMainFormMessage("You don't have games to play (Bad ending)");
        }

        public void ShowGameToPlayMessage(string gameToPlay)
        {
            string message = "";
            Random random = new Random();
            int randomNumber = random.Next(1, 6);
            switch (randomNumber)
            {
                case 1: 
                    message = "Let's go to play " + gameToPlay + "!";
                    break;
                case 2:
                    message = "Yoooo, is it " + gameToPlay + " that we gonna play?";
                    break;
                case 3:
                    message = "The best choice for today is... " + gameToPlay + "!";
                    break;
                case 4:
                    message = "No way we are going to play " + gameToPlay + " rn!";
                    break;
                case 5:
                    message = "Hey chads, we are going to " + gameToPlay + "!";
                    break;
            }
            ShowMainFormMessage(message);
        }

        public void ShowTurnConfirmationMessagesError()
        {
            ShowMainFormMessage("You can't turn confirmation messages off while (all) messages are on");
        }

        private void ShowMainFormMessage(string message)
        {
            _mainForm.MyMessageBox.Show(message);
        }

        private void ShowOptionalMainFormMessage(string message)
        {
            if (_mainForm.ShowMessages) ShowMainFormMessage(message);
        }

        public static bool ShowFirstMeetDialog()
        {
            DialogResult dialogResult = MessageBox.Show("Seems like you are using " +
                "the program for the first time.\nIt does create files in the same path as it is located!" +
                "\nMake sure it is in the separate folder. Continue?", _confirmationDialogTitle,
                MessageBoxButtons.YesNo);
            return dialogResult == DialogResult.Yes;
        }

        private bool ShowDeleteConfirmationDialog(string objectToDelete, string listName) 
        {
            DialogResult dialogResult = _mainForm.MyMessageBox.Show("Are you sure you want to delete " + 
                objectToDelete + " from " + listName + " list?", _confirmationDialogTitle, 
                MessageBoxButtons.YesNo);
            return dialogResult == DialogResult.Yes;
        }

        public bool ShowDeleteGameDialog(string gameName)
        {
            return ShowDeleteConfirmationDialog(gameName, listName: "games");
        }

        public bool ShowDeleteAvailableGameDialog(string gameName)
        {
            return ShowDeleteConfirmationDialog(gameName, listName: "available games");
        }

        public bool ShowDeleteGameFileDialog(string gameName)
        {
            return ShowDeleteConfirmationDialog(gameName, listName: "limits");
        }

        public bool ShowDeletePlayerFromListDialog(string selectedPlayer)
        {
            return ShowDeleteConfirmationDialog(selectedPlayer, listName: "player");
        }
    }
}
