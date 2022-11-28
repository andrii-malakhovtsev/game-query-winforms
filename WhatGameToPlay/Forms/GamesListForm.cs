using System;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class GamesListForm : Form
    {
        private readonly MainForm _mainForm;
        private readonly MessageController _messageController;
        private string _currentSelectedGame;
        private bool _startedLimitsEntering;
        private bool _playerLimitsExist;

        public GamesListForm(MainForm mainForm)
        {
            _mainForm = mainForm;
            _messageController = new MessageController(_mainForm);
            InitializeComponent();
            RefreshListBoxGames();
        }

        private void GamesListForm_Load(object sender, EventArgs e)
        {
            ThemeController.SetFormControlsTheme(form: this);
        }

        private string GetSelectedGameName()
        {
            return textBoxGameName.Text;
        }

        private void RefreshListBoxGames()
        {
            listBoxGames.Items.Clear();
            foreach (string game in FilesController.GetGamesListFromFile())
                listBoxGames.Items.Add(game);
        }

        private bool GameInList(string gameToCheck)
        {
            foreach (string game in FilesController.GetGamesListFromFile())
                if (gameToCheck == game) return true;
            return false;
        }

        private void ListBoxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxGames.SelectedItem != null)
                textBoxGameName.Text = listBoxGames.SelectedItem.ToString();
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
            bool selectedGameInList = GameInList(GetSelectedGameName());
            if (selectedGameInList)
            {
                _currentSelectedGame = GetSelectedGameName();
                listBoxGames.SelectedIndex = listBoxGames.FindString(_currentSelectedGame);
                SetNumericUpDownsStandartValues();
            }
            else
            {
                SetNumericUpDownsStandartValues();
                listBoxGames.ClearSelected();
                SetNumericUpDownsEnables(enable: false);
                buttonAddGame.Enabled = !FilesController.StringSpacesOnly(GetSelectedGameName());
            }
            checkBoxPlayersNumberLimit.Enabled = selectedGameInList;
            SetGameButtonsEnables(enable: !selectedGameInList);
            SetPlayersLimitsToNumericUpDowns();
        }

        private decimal[] GetPlayersLimits()
        {
            int limitsCount = 2;
            decimal[] limits = new decimal[limitsCount];
            _playerLimitsExist =
                FilesController.GetPlayersLimitsFromGameFile(GetSelectedGameName(), ref limits);
            return limits;
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
            FilesController.AddGameToGameListFile(GetSelectedGameName());
            RefreshListBoxGames();
            GetPlayersLimits();
            if (!_playerLimitsExist) 
                FilesController.AppendGameToPlayersFiles(GetSelectedGameName());
            _messageController.ShowGameAddedToListMessage(GetSelectedGameName());
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
            DeleteGame();
        }

        private void DeleteGame()
        {
            if (_mainForm.ShowConfirmationMessages)
            {
                if (_messageController.ShowDeleteGameDialog(GetSelectedGameName()))
                    DeleteGameFromGameList();
            }
            else DeleteGameFromGameList();
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

        private void DeleteGameFromGameList()
        {
            foreach (string game in FilesController.GetGamesListFromFile())
            {
                if (game == GetSelectedGameName())
                {
                    FilesController.DeleteGameFromGameList(GetSelectedGameName());
                    break;
                }
            }
            RefreshListBoxGames();
            SwitchGameButtonsEnables();
            if (!_mainForm.SaveDeletedGamesData)
                FilesController.DeletePlayersGameData(GetSelectedGameName());
            textBoxGameName.Clear();
            SetNumericUpDownsStandartValues();
        }

        private void CheckBoxPlayersNumberLimit_CheckedChanged(object sender, EventArgs e)
        {
            SetNumericUpDownsEnables(enable: checkBoxPlayersNumberLimit.Checked);
            _startedLimitsEntering = checkBoxPlayersNumberLimit.Checked;
            if (!checkBoxPlayersNumberLimit.Checked && _mainForm.ShowConfirmationMessages &&
                FilesController.PlayersLimitsExist(GetSelectedGameName()))
            {
                if (_messageController.ShowDeletePlayersLimitsFileDialog(GetSelectedGameName()))
                {
                    FilesController.DeletePlayersLimitsFile(GetSelectedGameName());
                    SetNumericUpDownsStandartValues();
                }
                else checkBoxPlayersNumberLimit.Checked = true;
            }
        }

        private void SetPlayersLimitsToNumericUpDowns()
        {
            checkBoxPlayersNumberLimit.Checked = false;
            decimal[] limits = GetPlayersLimits();
            if (_playerLimitsExist)
            {
                numericUpDownMin.Value = limits[0];
                numericUpDownMax.Value = limits[1];
                checkBoxPlayersNumberLimit.Checked = true;
            }
        }

        private bool SavePlayersLimits()
        {
            bool limitsFit = numericUpDownMax.Value > numericUpDownMin.Value;
            if (limitsFit)
            {
                FilesController.WritePlayersLimitsToFile(_currentSelectedGame, numericUpDownMin.Value,
                    numericUpDownMax.Value);
            }
            else if (_mainForm.ShowMessages)
            {
                _messageController.ShowPlayersLimitsErrorMessage();
            }
            return limitsFit;
        }

        private void GamesListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _mainForm.ClearInformation();
        }
    }
}
