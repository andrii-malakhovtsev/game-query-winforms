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
            if (_mainForm.ShowMessages) _mainForm.MyMessageBox.Show("Restrictions Set!");
            _mainForm.ClearAvailableGamesListBox();
        }

        public void ShowRestrictionsErrorMessage()
        {
            _mainForm.MyMessageBox.Show("The Min value must not exceed the Max value");
        }

        public void ShowGameAddedMessage(string gameName)
        {
            if (_mainForm.ShowMessages)
                _mainForm.MyMessageBox.Show("Game " + gameName + " is successfully added!");
        }

        public void ShowGameListSetForPlayerMessage()
        {
            if (_mainForm.ShowMessages)
                _mainForm.MyMessageBox.Show("Game list for specific player set!");
        }

        public void ShowPersonAddedToListMessage()
        {
            if (_mainForm.ShowMessages) 
                _mainForm.MyMessageBox.Show("New person added to the list!");
        }

        public bool ShowDeleteGameDialog(string gameName)
        {
            DialogResult dialogResult = _mainForm.MyMessageBox.Show("Are you sure you want to delete "
                    + gameName + " from games list?", "Confirmation", MessageBoxButtons.YesNo);
            return dialogResult == DialogResult.Yes;
        }

        public bool ShowDeleteGameFileDialog(string gameName)
        {
            DialogResult dialogResult =
                _mainForm.MyMessageBox.Show("Are you sure you want to delete "
                + gameName + "?", "Confirmation", MessageBoxButtons.YesNo);
            return dialogResult == DialogResult.Yes;
        }

        public bool ShowDeletePlayerFromListDialog(string selectedPlayer)
        {
            DialogResult dialogResult = _mainForm.MyMessageBox.Show("Are you sure you want to delete " 
                + selectedPlayer + " from player list?", "Confirmation", MessageBoxButtons.YesNo);
            return dialogResult == DialogResult.Yes;
        }
    }
}
