using System;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class GamesListForm : Form
    {
        private readonly MainForm _mainForm;
        private readonly ThemeController _theme;
        private readonly MessageController _messageController;

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
            FilesController.AppendGameToPlayerFile(GetSelectedGameName());
            _mainForm.RefreshPeopleList();
            _messageController.ShowGameAddedMessage(GetSelectedGameName());
            SwitchButtonsEnables();
            SetGameButtonsEnables(enable: false);
            checkBoxPeopleNumberLimit.Enabled = true;
        }

        private void ButtonDeleteGame_Click(object sender, EventArgs e)
        {
            if (_mainForm.ShowConfirmingMessages)
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
            _mainForm.RefreshPeopleList();
            RefreshListBoxGames();
            SwitchButtonsEnables();
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
                checkBoxPeopleNumberLimit.Checked = false;
                checkBoxPeopleNumberLimit.Enabled = true;
                SetNumericUpDownsStandartValues();
                SetGameButtonsEnables(enable: false);
                int restrictionsCount = 2;
                decimal[] restrictions = new decimal[restrictionsCount];
                bool restrictionsExist = 
                    FilesController.GetRestrictionsFromGameFile(GetSelectedGameName(), ref restrictions);
                if (restrictionsExist)
                {
                    numericUpDownMin.Value = restrictions[0];
                    numericUpDownMax.Value = restrictions[1];
                    checkBoxPeopleNumberLimit.Checked = true;
                }
            }
            else
            {
                buttonSet.Enabled = false;
                SetNumericUpDownsEnables(enable: false);
                checkBoxPeopleNumberLimit.Enabled = false;
                SetGameButtonsEnables(enable: true);
                checkBoxPeopleNumberLimit.Checked = false;
            }
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
        }

        private void SwitchButtonsEnables()
        {
            buttonAddGame.Enabled = !buttonAddGame.Enabled;
            buttonDeleteGame.Enabled = !buttonDeleteGame.Enabled;
        }

        private void CheckBoxPeopleNumberLimit_CheckedChanged(object sender, EventArgs e)
        {
            SetNumericUpDownsEnables(enable: checkBoxPeopleNumberLimit.Checked);
            buttonSet.Enabled = checkBoxPeopleNumberLimit.Checked;
        }

        private void CheckBoxPeopleNumberLimit_Click(object sender, EventArgs e)
        {
            if (!checkBoxPeopleNumberLimit.Checked && _mainForm.ShowConfirmingMessages)
            {
                string gameFullName = "";
                if (FilesController.RestrictionExist(GetSelectedGameName(), ref gameFullName))
                {
                    if (_messageController.ShowDeleteGameFileDialog(GetSelectedGameName()))
                        FilesController.DeleteFile(gameFullName);
                    else checkBoxPeopleNumberLimit.Checked = true;
                }
            }
        }

        private void ButtonSet_Click(object sender, EventArgs e)
        {
            if (numericUpDownMax.Value > numericUpDownMin.Value)
            {
                FilesController.AddRestrictionsToFile(GetSelectedGameName(), numericUpDownMin.Value, 
                    numericUpDownMax.Value);
                _messageController.ShowRestrictionsMessage();
                _mainForm.ClearAvailableGamesListBox();
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
                checkBoxPeopleNumberLimit,
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
