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
                FilesReader.GetPlayersLimitsFromGameFile(SelectedGameName, out decimal[] limits);
                return limits;
            }
        }

        public string SelectedGameName { get => _gamesListForm.TextBoxGameNameText; }

        public bool PlayerLimitsExist { get => FilesReader.GetPlayersLimitsFromGameFile(SelectedGameName, out _); }

        public bool StartedLimitsEntering { set => _startedLimitsEntering = value; }

        private static bool GameInList(string gameToCheck)
        {
            foreach (string game in FilesReader.GamesListFromFile)
            {
                if (gameToCheck == game)
                {
                    return true;
                }
            }
            return false;
        }

        public void DeleteGameConfirmation()
        {
            if (_mainForm.ShowConfirmationMessages && !_mainForm.MessageDisplayer.ShowDeleteGameDialog(SelectedGameName)) return;
            DeleteGameFromGameList();
        }

        private void DeleteGameFromGameList()
        {
            DeleteGameFromGameListFile();
            RefreshListBoxGames();
            _gamesListForm.SwitchGameButtonsEnables();
            if (!_mainForm.SaveDeletedGamesData)
            {
                FilesController.DeletePlayersGameData(SelectedGameName);
            }
            _gamesListForm.TextBoxGameNameText = string.Empty;
            _gamesListForm.SetNumericUpDownsStandartValues();
        }

        private void DeletePlayerLimits()
        {
            FilesController.DeletePlayersLimitsFile(SelectedGameName);
            _gamesListForm.SetNumericUpDownsStandartValues();
        }

        private void DeleteGameFromGameListFile()
        {
            foreach (string game in FilesReader.GamesListFromFile)
            {
                if (game == SelectedGameName)
                {
                    FilesController.DeleteGameFromGameList(SelectedGameName);
                    break;
                }
            }
        }

        private void SelectGameInListBox(bool selectedGameInList)
        {
            if (selectedGameInList)
            {
                _currentSelectedGame = SelectedGameName;
                ListBoxGames.SelectedIndex = ListBoxGames.FindString(SelectedGameName);
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
            foreach (string game in FilesReader.GamesListFromFile)
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
            bool selectedGameInList = GameInList(SelectedGameName);
            _gamesListForm.SetNumericUpDownsStandartValues();
            SelectGameInListBox(selectedGameInList);
            _gamesListForm.SetGameRelatedControlsEnables(enable: !selectedGameInList);
            if (FilesReader.StringContainsBannedSymbols(SelectedGameName))
            {
                _gamesListForm.UnableButtonAddgame();
            }
            _gamesListForm.SetPlayersLimitsToNumericUpDowns();
        }

        public void AddGame()
        {
            FilesWriter.AddGameToGameListFile(SelectedGameName);
            RefreshListBoxGames();
            if (!PlayerLimitsExist)
            {
                FilesWriter.AppendGameToPlayersFiles(SelectedGameName);
            }
            _mainForm.MessageDisplayer.ShowGameAddedToListMessage(SelectedGameName);
            _gamesListForm.SwitchGameButtonsEnables();
            _gamesListForm.SetGameRelatedControlsEnables(enable: false);
        }

        private bool SavePlayersLimits()
        {
            bool limitsFit = _gamesListForm.NumericUpDownMaxValue > _gamesListForm.NumericUpDownMinValue;
            if (limitsFit)
            {
                FilesWriter.WritePlayersLimitsToFile(_currentSelectedGame,
                _gamesListForm.NumericUpDownMinValue, _gamesListForm.NumericUpDownMaxValue);
            }
            else if (_mainForm.ShowMessages)
            {
                _mainForm.MessageDisplayer.ShowPlayersLimitsErrorMessage();
            }
            return limitsFit;
        }

        public void DeleteGameFromListBox()
        {
            if (SelectedGameName == string.Empty) return;
            DeleteGameConfirmation();
        }

        public void DeletePlayerLimitsDialog()
        {
            if (_mainForm.ShowConfirmationMessages)
            {
                if (_mainForm.MessageDisplayer.ShowDeletePlayersLimitsFileDialog(SelectedGameName))
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
