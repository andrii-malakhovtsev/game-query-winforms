using System;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class GamesListForm : Form
    {
        private readonly MainForm _mainForm;
        private readonly MessageDisplayer _messageDisplayer;
        private string _currentSelectedGame;
        private bool _startedLimitsEntering;

        public GamesListForm(MainForm mainForm)
        {
            _mainForm = mainForm;
            _messageDisplayer = new MessageDisplayer(_mainForm);
            InitializeComponent();
            RefreshListBoxGames();
        }

        private bool PlayerLimitsExist { get => FilesReader.GetPlayersLimitsFromGameFile(SelectedGameName, out _); }

        private decimal[] PlayersLimits
        {
            get
            {
                FilesReader.GetPlayersLimitsFromGameFile(SelectedGameName, out decimal[] limits);
                return limits;
            }
        }

        private string SelectedGameName { get => textBoxGameName.Text; }

        private void GamesListForm_Load(object sender, EventArgs e)
        {
            ThemeController.SetFormControlsTheme(form: this);
        }

        private void RefreshListBoxGames()
        {
            listBoxGames.Items.Clear();
            foreach (string game in FilesReader.GamesListFromFile)
            {
                listBoxGames.Items.Add(game);
            }
        }

        private bool GameInList(string gameToCheck)
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

        private void ListBoxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxGames.SelectedItem != null) 
            {
                textBoxGameName.Text = listBoxGames.SelectedItem.ToString();
            }
        }

        private void TextBoxGameName_TextChanged(object sender, EventArgs e)
        {
            if (_startedLimitsEntering && !SavePlayersLimits())
            {
                _startedLimitsEntering = false;
                textBoxGameName.Text = _currentSelectedGame;
                listBoxGames.SelectedIndex = listBoxGames.FindString(_currentSelectedGame);
                return;
            }
            bool selectedGameInList = GameInList(SelectedGameName);
            SetNumericUpDownsStandartValues();
            if (selectedGameInList)
            {
                _currentSelectedGame = SelectedGameName;
                listBoxGames.SelectedIndex = listBoxGames.FindString(SelectedGameName);
            }
            else
            {
                listBoxGames.ClearSelected();
                SetNumericUpDownsEnables(enable: false);
            }
            checkBoxPlayersNumberLimit.Enabled = selectedGameInList;
            SetGameButtonsEnables(enable: !selectedGameInList);
            if (FilesReader.StringContainsBannedSymbols(SelectedGameName))
            {
                buttonAddGame.Enabled = false;
            }
            SetPlayersLimitsToNumericUpDowns();
        }

        private void SetGameButtonsEnables(bool enable)
        {
            buttonAddGame.Enabled = enable;
            buttonDeleteGame.Enabled = !enable;
            AcceptButton = enable ? buttonAddGame : buttonDeleteGame;
        }

        private void SwitchGameButtonsEnables()
        {
            buttonAddGame.Enabled = !buttonAddGame.Enabled;
            buttonDeleteGame.Enabled = !buttonDeleteGame.Enabled;
        }

        private void ButtonAddGame_Click(object sender, EventArgs e)
        {
            FilesWriter.AddGameToGameListFile(SelectedGameName);
            RefreshListBoxGames();
            if (!PlayerLimitsExist)
            {
                FilesWriter.AppendGameToPlayersFiles(SelectedGameName);
            }
            _messageDisplayer.ShowGameAddedToListMessage(SelectedGameName);
            SwitchGameButtonsEnables();
            SetGameButtonsEnables(enable: false);
            checkBoxPlayersNumberLimit.Enabled = true;
        }

        private void ButtonDeleteGame_Click(object sender, EventArgs e)
        {
            DeleteGame();
        }

        private void ListBoxGames_DoubleClick(object sender, EventArgs e)
        {
            if (SelectedGameName == string.Empty) return;
            DeleteGame();
        }

        private void DeleteGame()
        {
            if (_mainForm.ShowConfirmationMessages() && !_messageDisplayer.ShowDeleteGameDialog(SelectedGameName)) return;
            DeleteGameFromGameList();
        }

        private void DeleteGameFromGameList()
        {
            foreach (string game in FilesReader.GamesListFromFile)
            {
                if (game == SelectedGameName)
                {
                    FilesController.DeleteGameFromGameList(SelectedGameName);
                    break;
                }
            }
            RefreshListBoxGames();
            SwitchGameButtonsEnables();
            if (!_mainForm.SaveDeletedGamesData())
            {
                FilesController.DeletePlayersGameData(SelectedGameName);
            }
            textBoxGameName.Clear();
            SetNumericUpDownsStandartValues();
        }

        private void SetNumericUpDownsEnables(bool enable)
        {
            numericUpDownMax.Enabled = enable;
            numericUpDownMin.Enabled = enable;
        }

        private void SetNumericUpDownsStandartValues()
        {
            numericUpDownMin.Value = numericUpDownMin.Minimum;
            numericUpDownMax.Value = numericUpDownMax.Maximum;
        }

        private void CheckBoxPlayersNumberLimit_CheckedChanged(object sender, EventArgs e)
        {
            SetNumericUpDownsEnables(enable: checkBoxPlayersNumberLimit.Checked);
            _startedLimitsEntering = checkBoxPlayersNumberLimit.Checked;
            if (checkBoxPlayersNumberLimit.Checked
                || !FilesReader.PlayersLimitsFileExist(SelectedGameName)) return;
            if (_mainForm.ShowConfirmationMessages())
            {
                if (_messageDisplayer.ShowDeletePlayersLimitsFileDialog(SelectedGameName))
                {
                    DeletePlayerLimits();
                }
                else
                {
                    checkBoxPlayersNumberLimit.Checked = true;
                }
            }
            else DeletePlayerLimits();
        }

        private void DeletePlayerLimits()
        {
            FilesController.DeletePlayersLimitsFile(SelectedGameName);
            SetNumericUpDownsStandartValues();
        }

        private void SetPlayersLimitsToNumericUpDowns()
        {
            checkBoxPlayersNumberLimit.Checked = PlayerLimitsExist;
            if (PlayerLimitsExist)
            {
                numericUpDownMin.Value = PlayersLimits[0];
                numericUpDownMax.Value = PlayersLimits[1];
            }
        }

        private bool SavePlayersLimits()
        {
            bool limitsFit = numericUpDownMax.Value > numericUpDownMin.Value;
            if (limitsFit)
            {
                FilesWriter.WritePlayersLimitsToFile(_currentSelectedGame,
                numericUpDownMin.Value, numericUpDownMax.Value);
            }
            else if (_mainForm.ShowMessages)
            {
                _messageDisplayer.ShowPlayersLimitsErrorMessage();
            }
            return limitsFit;
        }

        private void GamesListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBoxPlayersNumberLimit.Checked) SavePlayersLimits();
            _mainForm.ClearInformation();
        }
    }
}
