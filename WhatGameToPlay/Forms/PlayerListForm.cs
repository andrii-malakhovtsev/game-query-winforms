using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class PlayerListForm : Form
    {
        private readonly MainForm _mainForm;
        private readonly ThemeController _theme;
        private readonly MessageController _messageController;
        private string _startedSettingPlayerName;
        private bool _startedSetting = false;

        public PlayerListForm(MainForm MainForm, ThemeController theme)
        {
            _mainForm = MainForm;
            _messageController = new MessageController(_mainForm);
            _theme = theme;
            InitializeComponent();
        }

        private string GetSelectedPlayerName()
        {
            return textBoxSelectedPlayer.Text;
        }

        private void FormPlayerList_Load(object sender, EventArgs e)
        {
            foreach (Player player in FilesController.GetPlayersListFromDirectory())
                listBoxPlayers.Items.Add(player.Name);
            RefreshColors();
        }

        private void ListBoxPlayers_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPlayers.SelectedItem != null)
                textBoxSelectedPlayer.Text = listBoxPlayers.SelectedItem.ToString();
        }

        private void SelectPlayer()
        {
            listBoxPlayers.SelectedIndex = listBoxPlayers.FindString(GetSelectedPlayerName());
            checkedListBoxGamesPlaying.Items.Clear();
            List<string> gamesList = FilesController.GetGamesListFromFile();
            for (int index = 0; index < gamesList.Count; index++)
            {
                checkedListBoxGamesPlaying.Items.Add(gamesList[index]);
                checkedListBoxGamesPlaying.SetItemChecked(index, value: true);
            }
            string[] gamesPlayerDoesntPlay =
                FilesController.GetGamesPlayerDoesntPlay(GetSelectedPlayerName());
            if (gamesPlayerDoesntPlay.Length != 0 && gamesPlayerDoesntPlay != null)
            {
                foreach (string gameDoesntPlay in gamesPlayerDoesntPlay)
                {
                    for (int i = 0; i < gamesList.Count; i++)
                    {
                        if (gameDoesntPlay == checkedListBoxGamesPlaying.Items[i].ToString())
                            checkedListBoxGamesPlaying.SetItemChecked(i, false);
                    }
                }
            }
        }

        private void ButtonGamesPlayingSet_Click(object sender, EventArgs e)
        {
            List<string> checkedCheckboxes = new List<string>();
            foreach (var game in checkedListBoxGamesPlaying.CheckedItems)
                checkedCheckboxes.Add(game.ToString());
            List<string> gamesNotPlayingList =
                FilesController.GetGamesFromFile().Distinct().Except(checkedCheckboxes).ToList();
            FilesController.WriteGamesNotPlayingToFile(GetSelectedPlayerName(), gamesNotPlayingList);
            _messageController.ShowGameListSetForPlayerMessage(GetSelectedPlayerName());
            _mainForm.ClearInformation();
            _startedSetting = false;
        }

        private void CheckBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxGamesPlaying.Items.Count; i++)
                checkedListBoxGamesPlaying.SetItemChecked(i, checkBoxSelectAll.Checked);
        }

        private void CheckedListBoxGamesPlaying_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (MouseButtons == MouseButtons.Left)
            {
                _startedSetting = e.NewValue != CheckState.Indeterminate;
                if (_startedSetting) _startedSettingPlayerName = textBoxSelectedPlayer.Text;
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (_startedSetting && _mainForm.ShowConfirmationMessages)
            {
                if (!_messageController.ShowUnsavedDataWarining())
                {
                    _startedSetting = false;
                    textBoxSelectedPlayer.Text = _startedSettingPlayerName;
                    return;
                }
            }
            string selectedPlayer = GetSelectedPlayerName();
            bool playerExist = FilesController.DoesPlayerFileExist(selectedPlayer);
            if (playerExist) SelectPlayer();
            else
            {
                listBoxPlayers.ClearSelected();
                checkedListBoxGamesPlaying.Items.Clear();
            }
            SetPlayerButtonsEnables(enable: !playerExist);
            SetControlsEnables(enable: playerExist);
            if (FilesController.IsStringSpacesOnly(selectedPlayer))
                buttonAddPlayer.Enabled = false;
        }

        private void SetControlsEnables(bool enable)
        {
            checkBoxSelectAll.Enabled = enable;
            buttonSet.Enabled = enable;
        }

        private void SetPlayerButtonsEnables(bool enable)
        {
            buttonAddPlayer.Enabled = enable;
            buttonDeletePlayer.Enabled = !enable;
            AcceptButton = enable ? buttonAddPlayer : buttonDeletePlayer;
        }

        private void ButtonAddPlayer_Click(object sender, EventArgs e)
        {
            FilesController.CreatePlayerFile(GetSelectedPlayerName());
            _mainForm.ClearInformation();
            listBoxPlayers.Items.Clear();
            foreach (Player player in FilesController.GetPlayersListFromDirectory())
                listBoxPlayers.Items.Add(player.Name);
            SelectPlayer();
            _messageController.ShowPlayerAddedToListMessage(GetSelectedPlayerName());
            SetPlayerButtonsEnables(enable: false);
            SetControlsEnables(enable: true);
        }

        private void ButtonDeletePlayer_Click(object sender, EventArgs e)
        {
            if (_mainForm.ShowConfirmationMessages)
            {
                if (_messageController.ShowDeletePlayerFromListDialog(GetSelectedPlayerName()))
                    DeletePlayerFromList();
            }
            else DeletePlayerFromList();
            _mainForm.ClearInformation();
            textBoxSelectedPlayer.Clear();
        }

        private void DeletePlayerFromList()
        {
            foreach (Player player in FilesController.GetPlayersListFromDirectory())
            {
                if (player.Name == GetSelectedPlayerName())
                {
                    FilesController.DeleteSelectedPlayerFile(player.Name);
                    break;
                }
            }
            FilesController.DeleteSelectedPlayerFile(selectedPlayer: GetSelectedPlayerName());
            listBoxPlayers.Items.Clear();
            _mainForm.ClearInformation();
            foreach (Player player in FilesController.GetPlayersListFromDirectory())
                listBoxPlayers.Items.Add(player.Name);
            checkedListBoxGamesPlaying.Items.Clear();
            SetPlayerButtonsEnables(enable: true);
        }

        private void RefreshColors()
        {
            _theme.SetFormBackgroundColor(this);
            Control[] controls = {
                labelPlayerList,
                labelGamesPlaying,
                checkBoxSelectAll
            };
            _theme.SetControlsForeColor(controls);
            Control[] fullColorControls = {
                listBoxPlayers,
                checkedListBoxGamesPlaying,
                textBoxSelectedPlayer
            };
            _theme.SetControlsFullColor(fullColorControls);
            Button[] buttons = {
                buttonSet,
                buttonAddPlayer,
                buttonDeletePlayer
            };
            _theme.SetButtonsColor(buttons);
        }
    }
}
