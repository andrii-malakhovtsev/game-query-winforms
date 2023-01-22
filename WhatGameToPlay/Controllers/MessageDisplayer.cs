using System;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    sealed public class MessageDisplayer
    {
        private const string ConfirmationDialogTitle = "Confirmation";
        private readonly MainForm _mainForm;

        public MessageDisplayer(MainForm mainform)
        {
            _mainForm = mainform;
        }

        private void ShowMainFormMessage(string message)
        {
            _mainForm.AdvancedMessageBox.Show(message);
        }

        private void ShowOptionalMainFormMessage(string message)
        {
            if (_mainForm.ShowMessages) 
                ShowMainFormMessage(message);
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
            var random = new Random();
            int randomNumber = random.Next(1, 6);
            switch (randomNumber)
            {
                case 1: return $"Let's go play {gameToPlay}!";
                case 2: return $"Yoooo, is it {gameToPlay} that we gonna play?";
                case 3: return $"The best choice for today is...  {gameToPlay}!";
                case 4: return $"No way we are going to play {gameToPlay} rn!";
                case 5: return $"Hey chads, we are going to {gameToPlay}!";
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
            ShowOptionalMainFormMessage($"Game {gameName} is successfully added!");
        }

        public void ShowPlayerAddedToListMessage(string playerName)
        {
            ShowOptionalMainFormMessage(playerName + " is added to the list!");
        }

        private bool ShowDialog(string message)
        {
            DialogResult dialogResult = _mainForm.AdvancedMessageBox.Show(message, ConfirmationDialogTitle,
                MessageBoxButtons.YesNo);
            return dialogResult == DialogResult.Yes;
        }

        public bool ShowFirstMeetingDialog()
        {
            return ShowDialog("Seems like you are using " +
                "the program for the first time.\nIt does create " +
                "files in the same path as it is located!" +
                "\nMake sure it is in the separate folder. Continue?");
        }

        private bool ShowDeleteDialog(string objectToDelete, string listName)
        {
            return ShowDialog($"Are you sure you want to delete {objectToDelete} from {listName} list?");
        }

        public bool ShowDeleteAvailableGameDialog(string gameName)
        {
            return ShowDeleteDialog(gameName, listName: "available games");
        }

        public bool ShowDeleteGameDialog(string gameName)
        {
            return ShowDeleteDialog(gameName, listName: "games");
        }

        public bool ShowDeletePlayersLimitsFileDialog(string gameName)
        {
            return ShowDeleteDialog(gameName, listName: "limits");
        }

        public bool ShowDeletePlayerFromListDialog(string selectedPlayer)
        {
            return ShowDeleteDialog(selectedPlayer, listName: "player");
        }
    }
}
