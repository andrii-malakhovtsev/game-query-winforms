using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class PlayerListForm : Form
    {
        private readonly MainForm _mainForm;
        private string _currentSelectedPlayerName;
        private bool _playerSelected;

        public PlayerListForm(MainForm mainForm)
        {
            _mainForm = mainForm;
            InitializeComponent();
        }

        private string SelectedPlayerName { get => textBoxSelectedPlayer.Text; }

        private void FormPlayerList_Load(object sender, EventArgs e)
        {
            RefreshPlayersFromFile();
            ThemeController.SetFormControlsTheme(form: this);
        }

        private void RefreshPlayersFromFile()
        {
            foreach (Player player in FilesReader.PlayersFromDirectory)
            {
                listBoxPlayers.Items.Add(player.Name);
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (_playerSelected)
            {
                SavePlayerGames(_currentSelectedPlayerName);
            }
            bool playerExist = FilesReader.PlayerFileExist(SelectedPlayerName);
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
            SetPlayersPanelEnables(playerExist);
        }

        private void SetPlayersPanelEnables(bool playerExist)
        {
            SetPlayerButtonsEnables(enable: !playerExist);
            checkBoxSelectAll.Enabled = checkedListBoxGamesPlaying.Items.Count != 0;
            if (FilesReader.StringContainsBannedSymbols(SelectedPlayerName))
            {
                buttonAddPlayer.Enabled = false;
            }
        }

        private void SelectPlayer()
        {
            _currentSelectedPlayerName = SelectedPlayerName;
            listBoxPlayers.SelectedIndex = listBoxPlayers.FindString(SelectedPlayerName);
            checkedListBoxGamesPlaying.Items.Clear();
            List<string> games = FilesReader.GamesListFromFile;
            SetGamesCheckedListBox(games);
            string[] gamesPlayerDoesNotPlay = FilesReader.GetGamesPlayerDoesNotPlay(SelectedPlayerName);
            if (gamesPlayerDoesNotPlay.Length == 0 || gamesPlayerDoesNotPlay == null) return;
            foreach (string gameDoesNotPlay in gamesPlayerDoesNotPlay)
            {
                UncheckGameNotPlayingCheckBox(games, gameDoesNotPlay);
            }
        }

        private void SetGamesCheckedListBox(List<string> games)
        {
            for (int index = 0; index < games.Count; index++)
            {
                checkedListBoxGamesPlaying.Items.Add(games[index]);
                checkedListBoxGamesPlaying.SetItemChecked(index, value: true);
            }
        }

        private void UncheckGameNotPlayingCheckBox(List<string> games, string gameDoesNotPlay)
        {
            for (int i = 0; i < games.Count; i++)
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
            FilesController.CreatePlayerFile(SelectedPlayerName);
            listBoxPlayers.Items.Clear();
            RefreshPlayersFromFile();
            SelectPlayer();
            _mainForm.MessageDisplayer.ShowPlayerAddedToListMessage(SelectedPlayerName);
            SetPlayerButtonsEnables(enable: false);
            checkBoxSelectAll.Enabled = true;
        }

        private void SavePlayerGames(string playerName)
        {
            List<string> checkedCheckBoxes = checkedListBoxGamesPlaying.CheckedItems.Cast<string>().ToList(),
                         gamesNotPlayingList = FilesReader.GamesFromFile.Distinct().Except(checkedCheckBoxes).ToList();
            FilesWriter.WriteGamesNotPlayingToFile(playerName, gamesNotPlayingList);
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
            if (SelectedPlayerName == string.Empty) return;
            DeletePlayer();
        }

        private void ButtonDeletePlayer_Click(object sender, EventArgs e)
        {
            DeletePlayer();
        }

        private void DeletePlayer()
        {
            if (_mainForm.ShowConfirmationMessages() 
                && !_mainForm.MessageDisplayer.ShowDeletePlayerFromListDialog(SelectedPlayerName)) return;
            DeletePlayerFromList();
        }

        private void DeletePlayerFromList()
        {
            _playerSelected = false;
            FilesController.DeleteSelectedPlayerFile(SelectedPlayerName);
            listBoxPlayers.Items.Clear();
            RefreshPlayersFromFile();
            checkedListBoxGamesPlaying.Items.Clear();
            SetPlayerButtonsEnables(enable: true);
            textBoxSelectedPlayer.Clear();
        }

        private void PlayerListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FilesReader.PlayerFileExist(SelectedPlayerName))
            {
                SavePlayerGames(SelectedPlayerName);
            }
            _mainForm.ClearInformation();
        }
    }
}
