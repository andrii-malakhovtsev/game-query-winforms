using System.Windows.Forms;

namespace WhatGameToPlay
{
    public class MessageController
    {
        private readonly MainForm _mainForm;

        public MessageController(MainForm mainform)
        {
            _mainForm = mainform;
        }

        public void ShowRestrictionsMessage()
        {
            ShowOptionalMainFormMessage("Restrictions Set!");
        }

        public void ShowRestrictionsErrorMessage()
        {
            ShowMainFormMessage("The Min value must not exceed the Max value");
        }

        public void ShowGameAddedMessage(string gameName)
        {
            ShowOptionalMainFormMessage("Game " + gameName + " is successfully added!");
        }

        public void ShowGameListSetForPlayerMessage()
        {
            ShowOptionalMainFormMessage("Game list for specific player set!");
        }

        public void ShowPersonAddedToListMessage()
        {
            ShowOptionalMainFormMessage("New person added to the list!");
        }

        public void ShowNoGamesToPlayMessage()
        {
            ShowMainFormMessage("You don't have games to play (Bad ending)");
        }

        public void ShowGameToPlayMessage(string gameToPlay)
        {
            ShowMainFormMessage("Let's go play " + gameToPlay + "!");
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

        private bool ShowDeleteConfirmationDialog(string objectToDelete, string listName) 
        {
            DialogResult dialogResult = _mainForm.MyMessageBox.Show("Are you sure you want to delete " + 
                objectToDelete + " from " + listName + " list?", "Confirmation", MessageBoxButtons.YesNo);
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
            return ShowDeleteConfirmationDialog(gameName, listName: "restrictions");
        }

        public bool ShowDeletePlayerFromListDialog(string selectedPlayer)
        {
            return ShowDeleteConfirmationDialog(selectedPlayer, listName: "player");
        }
    }
}
