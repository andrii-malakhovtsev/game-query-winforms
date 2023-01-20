using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class PlayerListForm : Form
    {
        private readonly MainForm _mainForm;
        private readonly MessageController _messageController;
        private string _currentSelectedPlayerName;
        private bool _playerSelected;

        public PlayerListForm(MainForm MainForm)
        {
            _mainForm = MainForm;
            _messageController = new MessageController(_mainForm);
            InitializeComponent();
        }

        private void FormPlayerList_Load(object sender, EventArgs e)
        {
            RefreshPlayersListFromFile();
            ThemeController.SetFormControlsTheme(form: this);
        }

        private void RefreshPlayersListFromFile()
        {
            foreach (Player player in FilesController.GetPlayersListFromDirectory())
            {
                listBoxPlayers.Items.Add(player.Name);
            }
        }

        private string GetSelectedPlayerName()
        {
            return textBoxSelectedPlayer.Text;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (_playerSelected)
            {
                SavePlayerGames(_currentSelectedPlayerName);
            }
            string selectedPlayer = GetSelectedPlayerName();
            bool playerExist = FilesController.PlayerFileExist(selectedPlayer);
            _playerSelected = playerExist;
            if (playerExist)
            {
                SelectPlayer();
            }
            else
            {
                listBoxPlayers.ClearSelected();
                checkedListBoxGamesPlaying.Items.Clear();
            }
            SetPlayerButtonsEnables(enable: !playerExist);
            checkBoxSelectAll.Enabled = checkedListBoxGamesPlaying.Items.Count != 0;
            if (FilesController.StringContainsBannedSymbols(selectedPlayer))
            {
                buttonAddPlayer.Enabled = false;
            }
        }

        private void SelectPlayer()
        {
            _currentSelectedPlayerName = GetSelectedPlayerName();
            listBoxPlayers.SelectedIndex = listBoxPlayers.FindString(_currentSelectedPlayerName);
            checkedListBoxGamesPlaying.Items.Clear();
            List<string> gamesList = FilesController.GetGamesListFromFile();
            for (int index = 0; index < gamesList.Count; index++)
            {
                checkedListBoxGamesPlaying.Items.Add(gamesList[index]);
                checkedListBoxGamesPlaying.SetItemChecked(index, value: true);
            }
            string[] gamesPlayerDoesNotPlay =
                FilesController.GetGamesPlayerDoesntPlay(GetSelectedPlayerName());
            if (gamesPlayerDoesNotPlay.Length == 0 || gamesPlayerDoesNotPlay == null) return;
            foreach (string gameDoesNotPlay in gamesPlayerDoesNotPlay)
            {
                UncheckGameNotPlayingCheckBox(gamesList, gameDoesNotPlay);
            }
        }

        private void UncheckGameNotPlayingCheckBox(List<string> gamesList, string gameDoesNotPlay)
        {
            for (int i = 0; i < gamesList.Count; i++)
            {
                if (gameDoesNotPlay == checkedListBoxGamesPlaying.Items[i].ToString())
                {
                    checkedListBoxGamesPlaying.SetItemChecked(i, false);
                }
            }
        }

        private void CheckBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxGamesPlaying.Items.Count; i++)
            {
                checkedListBoxGamesPlaying.SetItemChecked(i, checkBoxSelectAll.Checked);
            }
        }

        private void ButtonAddPlayer_Click(object sender, EventArgs e)
        {
            FilesController.CreatePlayerFile(GetSelectedPlayerName());
            listBoxPlayers.Items.Clear();
            RefreshPlayersListFromFile();
            SelectPlayer();
            _messageController.ShowPlayerAddedToListMessage(GetSelectedPlayerName());
            SetPlayerButtonsEnables(enable: false);
            checkBoxSelectAll.Enabled = true;
        }

        private void SavePlayerGames(string playerName)
        {
            List<string> checkedCheckboxes = new List<string>();
            foreach (var game in checkedListBoxGamesPlaying.CheckedItems)
            {
                checkedCheckboxes.Add(game.ToString());
            }
            List<string> gamesNotPlayingList =
                FilesController.GetGamesFromFile().Distinct().Except(checkedCheckboxes).ToList();
            FilesController.WriteGamesNotPlayingToFile(playerName, gamesNotPlayingList);
        }

        private void SetPlayerButtonsEnables(bool enable)
        {
            buttonAddPlayer.Enabled = enable;
            buttonDeletePlayer.Enabled = !enable;
            AcceptButton = enable ? buttonAddPlayer : buttonDeletePlayer;
        }

        private void ListBoxPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPlayers.SelectedItem != null)
            {
                textBoxSelectedPlayer.Text = listBoxPlayers.SelectedItem.ToString();
            }
        }

        private void ListBoxPlayers_DoubleClick(object sender, EventArgs e)
        {
            if (GetSelectedPlayerName() == string.Empty) return;
            DeletePlayer();
        }

        private void ButtonDeletePlayer_Click(object sender, EventArgs e)
        {
            DeletePlayer();
        }

        private void DeletePlayer()
        {
            if (_mainForm.ShowConfirmationMessages() 
                && !_messageController.ShowDeletePlayerFromListDialog(GetSelectedPlayerName())) return;
            DeletePlayerFromList();
        }

        private void DeletePlayerFromList()
        {
            _playerSelected = false;
            FilesController.DeleteSelectedPlayerFile(GetSelectedPlayerName());
            listBoxPlayers.Items.Clear();
            RefreshPlayersListFromFile();
            checkedListBoxGamesPlaying.Items.Clear();
            SetPlayerButtonsEnables(enable: true);
            textBoxSelectedPlayer.Clear();
        }

        private void PlayerListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FilesController.PlayerFileExist(GetSelectedPlayerName()))
            {
                SavePlayerGames(GetSelectedPlayerName());
            }
            _mainForm.ClearInformation();
        }
    }
}
