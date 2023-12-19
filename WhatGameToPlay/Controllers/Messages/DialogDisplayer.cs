using System.Windows.Forms;

namespace WhatGameToPlay
{
    public class DialogDisplayer
    {
        private const string ConfirmationDialogTitle = "Confirmation";
        private readonly MainForm _mainForm;

        public DialogDisplayer(MainForm mainform) => _mainForm = mainform;

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
            return ShowDialog($"Are you sure you want to delete {objectToDelete} from {listName.ToLower()} list?");
        }

        public bool ShowDeleteAvailableGameDialog(string gameName)
        {
            return ShowDeleteDialog(gameName, listName: FilesNames.AvailableGamesListName);
        }

        public bool ShowDeleteGameDialog(string gameName)
        {
            return ShowDeleteDialog(gameName, listName: FilesNames.GamesListName);
        }

        public bool ShowDeleteGameLimitsFileDialog(string gameName)
        {
            return ShowDeleteDialog(gameName, listName: _mainForm.Model.Directories.GamesLimits.Name);
        }

        public bool ShowDeletePlayerFromListDialog(string selectedPlayer)
        {
            return ShowDeleteDialog(selectedPlayer, listName: _mainForm.Model.Directories.Players.Name);
        }
    }
}
