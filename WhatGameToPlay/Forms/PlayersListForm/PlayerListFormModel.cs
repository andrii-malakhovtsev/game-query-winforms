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

        private string SelectedPlayerName => _playerListForm.TextBoxSelectedPlayerText;

        public void AddPlayer()
        {
            new PlayerFile(SelectedPlayerName, directory: _mainForm.Model.Directories.Players);
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
                && !_mainForm.DialogDisplayer.ShowDeletePlayerFromListDialog(SelectedPlayerName)) return;

            DeletePlayerFromList();
        }

        private void DeletePlayerFromList()
        {
            _playerSelected = false;
            new PlayerFile(SelectedPlayerName, directory: _mainForm.Model.Directories.Players).Delete();
            // mb change later to list of playerFiles from some model to not create PlayerFiles and search by name
            _playerListForm.ListBoxPlayers.Items.Clear();
            RefreshPlayersFromFile();
            _playerListForm.CheckBoxListGamesPlaying.Items.Clear();
            _playerListForm.SetPlayerButtonsEnables(enable: true);
            _playerListForm.TextBoxSelectedPlayerText = string.Empty;
        }

        public void RefreshPlayersFromFile()
        {
            foreach (Player player in _mainForm.Model.Directories.Players.PlayersList)
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

            bool playerExist = _mainForm.Model.Directories.Players.FileExists(SelectedPlayerName);
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
                         gamesNotPlayingList = _mainForm.Model.Files.GamesList.
                            CurrentGames.Distinct().Except(checkedCheckBoxes).ToList();
            _mainForm.Model.Directories.Players.WriteGamesNotPlayingToFile(playerName, gamesNotPlayingList);
        }

        private void SelectPlayer()
        {
            _currentSelectedPlayerName = SelectedPlayerName;
            _playerListForm.ListBoxPlayers.SelectedIndex = _playerListForm.ListBoxPlayers.FindString(SelectedPlayerName);
            _playerListForm.CheckBoxListGamesPlaying.Items.Clear();

            List<string> games = _mainForm.Model.Files.GamesList.CurrentGamesList;
            SetGamesCheckedListBox(games);

            string[] gamesPlayerDoesNotPlay = _mainForm.Model.Directories.Players.GetGamesPlayerDoesNotPlay(SelectedPlayerName);
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

        public void PlayersListFormClosing()
        {
            if (_mainForm.Model.Directories.Players.FileExists(SelectedPlayerName))
            {
                SavePlayerGames(SelectedPlayerName);
            }
            _mainForm.ClearInformation();
        }
    }
}
