using System;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class GamesListForm : Form
    {
        private readonly MainForm _mainForm;
        private readonly ThemeController _theme;
        private readonly MessageController _messageController;
        private bool _playerLimitsExist;

        public GamesListForm(MainForm mainForm, ThemeController theme)
        {
            _mainForm = mainForm;
            _messageController = new MessageController(_mainForm);
            _theme = theme;
            InitializeComponent();
            RefreshListBoxGames();
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

        private void ButtonAddGame_Click(object sender, EventArgs e)
        {
            FilesController.AddGameToGameListFile(GetSelectedGameName());
            RefreshListBoxGames();
            GetRestrictions();
            if (!_playerLimitsExist) FilesController.AppendGameToPlayersFiles(GetSelectedGameName());
            _mainForm.ClearInformation();
            _messageController.ShowGameAddedMessage(GetSelectedGameName());
            SwitchGameButtonsEnables();
            SetGameButtonsEnables(enable: false);
            checkBoxPlayersNumberLimit.Enabled = true;
        }

        private void ButtonDeleteGame_Click(object sender, EventArgs e)
        {
            if (_mainForm.ShowConfirmationMessages)
            {
                if (_messageController.ShowDeleteGameDialog(GetSelectedGameName()))
                    DeleteGameFromGameList();
            }
            else DeleteGameFromGameList();
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
            _mainForm.ClearInformation();
            RefreshListBoxGames();
            SwitchGameButtonsEnables();
            if (!_mainForm.SaveDeletedGamesData)
                FilesController.DeletePlayersGameData(GetSelectedGameName());
            textBoxGameName.Clear();
            SetNumericUpDownsStandartValues();
        }

        private void ListBoxGames_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxGames.SelectedItem != null)
                textBoxGameName.Text = listBoxGames.SelectedItem.ToString();
        }

        public bool CheckGameInGamesList(string gameToCheck)
        {
            foreach (string game in FilesController.GetGamesListFromFile())
                if (gameToCheck == game) return true;
            return false;
        }

        private void TextBoxGameName_TextChanged(object sender, EventArgs e)
        {
            if (CheckGameInGamesList(GetSelectedGameName()))
            {
                listBoxGames.SelectedIndex = 
                    listBoxGames.FindString(GetSelectedGameName());
                checkBoxPlayersNumberLimit.Enabled = true;
                SetNumericUpDownsStandartValues();
                SetGameButtonsEnables(enable: false);
            }
            else
            {
                SetNumericUpDownsStandartValues();
                listBoxGames.ClearSelected();
                SetGameButtonsEnables(enable: true);
                SetNumericUpDownsEnables(enable: false);
                buttonSet.Enabled = false;
                checkBoxPlayersNumberLimit.Enabled = false;
                buttonAddGame.Enabled = 
                    !FilesController.IsStringSpacesOnly(GetSelectedGameName());
            }
            checkBoxPlayersNumberLimit.Checked = false;
            decimal[] restrictions = GetRestrictions();
            if (_playerLimitsExist)
            {
                numericUpDownMin.Value = restrictions[0];
                numericUpDownMax.Value = restrictions[1];
                checkBoxPlayersNumberLimit.Checked = true;
            }
        }

        private decimal[] GetRestrictions()
        {
            int restrictionsCount = 2;
            decimal[] restrictions = new decimal[restrictionsCount];
            _playerLimitsExist =
                FilesController.GetRestrictionsFromGameFile(GetSelectedGameName(), ref restrictions);
            return restrictions;
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

        private void CheckBoxPlayersNumberLimit_CheckedChanged(object sender, EventArgs e)
        {
            SetNumericUpDownsEnables(enable: checkBoxPlayersNumberLimit.Checked);
            buttonSet.Enabled = checkBoxPlayersNumberLimit.Checked;
        }

        private void CheckBoxPlayersNumberLimit_Click(object sender, EventArgs e)
        {
            if (!checkBoxPlayersNumberLimit.Checked && _mainForm.ShowConfirmationMessages &&
                FilesController.RestrictionExist(GetSelectedGameName()))
            {
                if (_messageController.ShowDeleteGameFileDialog(GetSelectedGameName()))
                {
                    FilesController.DeleteGameRestrictionsFile(GetSelectedGameName());
                    SetNumericUpDownsStandartValues();
                    _mainForm.ClearInformation();
                }
                else checkBoxPlayersNumberLimit.Checked = true;
            }
        }

        private void ButtonSetPlayersLimit_Click(object sender, EventArgs e)
        {
            if (numericUpDownMax.Value > numericUpDownMin.Value)
            {
                FilesController.AddRestrictionsToFile(GetSelectedGameName(), numericUpDownMin.Value,
                    numericUpDownMax.Value);
                _messageController.ShowPlayersLimitSetMessage(GetSelectedGameName());
                _mainForm.ClearInformation();
            }
            else if (_mainForm.ShowMessages)
            {
                _messageController.ShowRestrictionsErrorMessage();
            }
        }

        private void FormGamesList_Load(object sender, EventArgs e)
        {
            RefreshColors();
        }

        private void RefreshColors()
        {
            _theme.SetFormBackgroundColor(this);
            Control[] foreColorControls = {
                checkBoxPlayersNumberLimit,
                labelGamesList,
                labelEnterGameName,
                labelMin,
                labelMax,
                buttonAddGame,
                buttonDeleteGame,
                buttonSet,
            };
            _theme.SetControlsForeColor(foreColorControls);
            Control[] fullColorControls = {
                listBoxGames,
                textBoxGameName,
                numericUpDownMax,
                numericUpDownMin
            };
            _theme.SetControlsFullColor(fullColorControls);
            Button[] buttons = {
                buttonAddGame,
                buttonDeleteGame,
                buttonSet
            };
            _theme.SetButtonsColor(buttons);
        }
    }
}
