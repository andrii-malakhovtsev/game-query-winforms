using System.Collections.Generic;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public class GamesListFormModel
    {
        private readonly GamesListForm _gamesListForm;
        private readonly MainForm _mainForm;
        private string _currentSelectedGame;
        private bool _startedLimitsEntering;

        public GamesListFormModel(GamesListForm gamesListForm, MainForm mainForm) 
        {
            _gamesListForm = gamesListForm;
            _mainForm = mainForm;
        }

        private ListBox ListBoxGames
        { 
            get => _gamesListForm.ListBoxGames; 
            set => _gamesListForm.ListBoxGames = value; 
        }

        public decimal[] PlayersLimits
        {
            get
            {
                _mainForm.Model.Directories.PlayersLimits.GetPlayersLimits(SelectedGame, out decimal[] limits);
                return limits;
            }
        }

        public string SelectedGame => _gamesListForm.TextBoxGameNameText;

        public bool PlayerLimitsExist => _mainForm.Model.Directories.PlayersLimits.GetPlayersLimits(SelectedGame, out _);

        public bool PlayerLimitsFileExists => _mainForm.Model.Directories.PlayersLimits.PlayersLimitsFileExist(SelectedGame);

        public bool StartedLimitsEntering { set => _startedLimitsEntering = value; }

        private List<string> CurrentGamesList => _mainForm.Model.Files.GamesList.CurrentGamesList;

        private bool GameInList(string gameToCheck)
        {
            foreach (string game in CurrentGamesList)
            {
                if (gameToCheck == game)
                {
                    return true;
                }
            }
            return false;
        }

        private void DeletePlayerLimits()
        {
            DeletePlayersGameData(SelectedGame);
            _gamesListForm.SetNumericUpDownsStandartValues();
        }

        private void DeletePlayersGameData(string gameToDelete)
        {
            _mainForm.Model.Directories.Players.DeleteGameFromPlayersFiles(gameToDelete);
            _mainForm.Model.Directories.PlayersLimits.DeletePlayersLimitsFile(gameName: gameToDelete);
        }

        private void DeleteGameFromGameListFile()
        {
            foreach (string game in CurrentGamesList)
            {
                if (game == SelectedGame)
                {
                    _mainForm.Model.Files.GamesList.DeleteFromFile(SelectedGame);
                    break;
                }
            }
        }

        private void SelectGameInListBox(bool selectedGameInList)
        {
            if (selectedGameInList)
            {
                _currentSelectedGame = SelectedGame;
                ListBoxGames.SelectedIndex = ListBoxGames.FindString(SelectedGame);
            }
            else
            {
                ListBoxGames.ClearSelected();
                _gamesListForm.SetNumericUpDownsEnables(enable: false);
            }
        }

        public void RefreshListBoxGames()
        {
            _gamesListForm.ListBoxGames.Items.Clear();
            foreach (string game in CurrentGamesList)
            {
                _gamesListForm.ListBoxGames.Items.Add(game);
            }
        }

        public void TextBoxGameNameTextChanged()
        {
            if (_startedLimitsEntering && !SavePlayersLimits())
            {
                _startedLimitsEntering = false;
                _gamesListForm.TextBoxGameNameText = _currentSelectedGame;
                ListBoxGames.SelectedIndex = ListBoxGames.FindString(_currentSelectedGame);
                return;
            }
            SelectGameInListBoxFormReaction();
        }

        private void SelectGameInListBoxFormReaction()
        {
            _gamesListForm.SetNumericUpDownsStandartValues();

            bool selectedGameInList = GameInList(SelectedGame);
            SelectGameInListBox(selectedGameInList);
            _gamesListForm.SetGameRelatedControlsEnables(enable: !selectedGameInList);

            if (FilesReader.StringContainsBannedSymbols(SelectedGame))
            {
                _gamesListForm.UnableButtonAddgame();
            }
            _gamesListForm.SetPlayersLimitsToNumericUpDowns();
        }

        public void AddGame()
        {
            _mainForm.Model.Files.GamesList.WriteToFile(SelectedGame);
            RefreshListBoxGames();

            //if (!PlayerLimitsExist)
            //{
            //    _mainForm.Model.Files.GamesList.WriteToFile(SelectedGame);
            //}

            _mainForm.MessageDisplayer.ShowGameAddedToListMessage(SelectedGame);
            SelectGameInListBox(selectedGameInList: true);
            _gamesListForm.SetGameRelatedControlsEnables(enable: false);
        }

        private bool SavePlayersLimits()
        {
            bool limitsFit = _gamesListForm.NumericUpDownMaxValue > _gamesListForm.NumericUpDownMinValue;
            if (limitsFit)
            {
                _mainForm.Model.Directories.PlayersLimits.WriteToFiles(
                    _currentSelectedGame,
                    _gamesListForm.NumericUpDownMinValue,
                    _gamesListForm.NumericUpDownMaxValue);
            }
            else if (_mainForm.ShowMessages)
            {
                _mainForm.MessageDisplayer.ShowPlayersLimitsErrorMessage();
            }
            return limitsFit;
        }

        private void DeleteGameFromGameList()
        {
            DeleteGameFromGameListFile();
            RefreshListBoxGames();
            _gamesListForm.SwitchGameButtonsEnables();

            if (!_mainForm.SaveDeletedGamesData)
            {
                DeletePlayersGameData(SelectedGame);
            }

            _gamesListForm.CheckBoxPlayersNumberLimitChecked = false;
            _gamesListForm.SetNumericUpDownsStandartValues();
            _gamesListForm.TextBoxGameNameText = string.Empty;
        }

        public void DeleteGameFromListBox()
        {
            if (SelectedGame == string.Empty) return;
            DeleteGameConfirmation();
        }

        public void DeleteGameConfirmation()
        {
            if (_mainForm.ShowConfirmationMessages && !_mainForm.DialogDisplayer.ShowDeleteGameDialog(SelectedGame)) return;
            DeleteGameFromGameList();
        }

        public void DeletePlayerLimitsDialog()
        {
            if (_mainForm.ShowConfirmationMessages)
            {
                if (_mainForm.DialogDisplayer.ShowDeletePlayersLimitsFileDialog(SelectedGame))
                {
                    DeletePlayerLimits();
                }
                else
                {
                    _gamesListForm.CheckBoxPlayersNumberLimitChecked = true;
                }
            }
            else DeletePlayerLimits();
        }

        public void GamesListFormClosing()
        {
            if (_gamesListForm.CheckBoxPlayersNumberLimitChecked) SavePlayersLimits();
            _mainForm.ClearInformation();
        }
    }
}
