using System.Collections.Generic;
using System.Linq;

namespace WhatGameToPlay
{
    public class PlayerListFormModel
    {
        private readonly PlayersListForm _playerListForm;
        private readonly MainForm _mainForm;
        private string _currentSelectedPlayerName;
        private bool _playerSelected;

        public PlayerListFormModel(PlayersListForm playersListForm, MainForm mainForm) 
        {
            _playerListForm = playersListForm;
            _mainForm = mainForm;
        }

        public string SelectedPlayerName { get => _playerListForm.TextBoxSelectedPlayerText; }

        public void RefreshPlayersFromFile()
        {
            foreach (Player player in FilesReader.PlayersFromDirectory)
            {
                _playerListForm.ListBoxPlayers.Items.Add(player.Name);
            }
        }

        public void TextBoxTextChanged()
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
                _playerListForm.ListBoxPlayers.ClearSelected();
                _playerListForm.CheckBoxListGamesPlaying.Items.Clear();
            }
            _playerListForm.SetPlayersPanelEnables(playerExist);
        }

        private void SavePlayerGames(string playerName)
        {
            List<string> checkedCheckBoxes = _playerListForm.CheckBoxListGamesPlaying.CheckedItems.Cast<string>().ToList(),
                         gamesNotPlayingList = FilesReader.GamesFromFile.Distinct().Except(checkedCheckBoxes).ToList();
            FilesWriter.WriteGamesNotPlayingToFile(playerName, gamesNotPlayingList);
        }

        private void SelectPlayer()
        {
            _currentSelectedPlayerName = SelectedPlayerName;
            _playerListForm.ListBoxPlayers.SelectedIndex = _playerListForm.ListBoxPlayers.FindString(SelectedPlayerName);
            _playerListForm.CheckBoxListGamesPlaying.Items.Clear();
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
                _playerListForm.CheckBoxListGamesPlaying.Items.Add(games[index]);
                _playerListForm.CheckBoxListGamesPlaying.SetItemChecked(index, value: true);
            }
        }

        private void UncheckGameNotPlayingCheckBox(List<string> games, string gameDoesNotPlay)
        {
            for (int i = 0; i < games.Count; i++)
            {
                if (gameDoesNotPlay == _playerListForm.CheckBoxListGamesPlaying.Items[i].ToString())
                {
                    _playerListForm.CheckBoxListGamesPlaying.SetItemChecked(i, false);
                }
            }
        }

        public void CheckBoxSelectAllCheckedChanged(bool selectAll)
        {
            for (int i = 0; i < _playerListForm.CheckBoxListGamesPlaying.Items.Count; i++)
            {
                _playerListForm.CheckBoxListGamesPlaying.SetItemChecked(i, selectAll);
            }
        }

        public void ListBoxPlayersSelectedIndexChanged()
        {
            if (_playerListForm.ListBoxPlayers.SelectedItem != null)
            {
                _playerListForm.TextBoxSelectedPlayerText = _playerListForm.ListBoxPlayers.SelectedItem.ToString();
            }
        }

        public void AddPlayer()
        {
            FilesController.CreatePlayerFile(SelectedPlayerName);
            _playerListForm.ListBoxPlayers.Items.Clear();
            RefreshPlayersFromFile();
            SelectPlayer();
            _mainForm.MessageDisplayer.ShowPlayerAddedToListMessage(SelectedPlayerName);
            _playerListForm.SetPlayerButtonsEnables(enable: false);
            _playerListForm.CheckBoxListGamesPlaying.Enabled = true;
        }

        public void DeletePlayer()
        {
            if (_mainForm.ShowConfirmationMessages
                && !_mainForm.MessageDisplayer.ShowDeletePlayerFromListDialog(SelectedPlayerName)) return;
            DeletePlayerFromList();
        }

        private void DeletePlayerFromList()
        {
            _playerSelected = false;
            FilesController.DeleteSelectedPlayerFile(SelectedPlayerName);
            _playerListForm.ListBoxPlayers.Items.Clear();
            RefreshPlayersFromFile();
            _playerListForm.CheckBoxListGamesPlaying.Items.Clear();
            _playerListForm.SetPlayerButtonsEnables(enable: true);
            _playerListForm.TextBoxSelectedPlayerText = string.Empty;
        }

        public void PlayersListFormClosing()
        {
            if (FilesReader.PlayerFileExist(SelectedPlayerName))
            {
                SavePlayerGames(SelectedPlayerName);
            }
            _mainForm.ClearInformation();
        }
    }
}
