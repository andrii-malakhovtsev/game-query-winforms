using System;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class PlayersListForm : Form
    {
        private readonly PlayerListFormModel _playerListFormModel;

        public PlayersListForm(MainForm mainForm)
        {
            _playerListFormModel = new PlayerListFormModel(this, mainForm);
            InitializeComponent();
        }

        public CheckedListBox CheckBoxListGamesPlaying => checkedListBoxGamesPlaying;

        public ListBox ListBoxPlayers => listBoxPlayers;

        public string TextBoxSelectedPlayerText 
        { 
            get => textBoxSelectedPlayer.Text; 
            set => textBoxSelectedPlayer.Text = value; 
        }

        private void FormPlayerList_Load(object sender, EventArgs e)
        {
            _playerListFormModel.RefreshPlayersFromFile();
            FormsTheme.ColorControls(form: this);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
            => _playerListFormModel.TextBoxTextChanged();

        public void SetPlayersPanelEnables(bool playerExist)
        {
            SetPlayerButtonsEnables(enable: !playerExist);
            checkBoxSelectAll.Enabled = checkedListBoxGamesPlaying.Items.Count != 0;

            if (FilesReader.StringContainsBannedSymbols(TextBoxSelectedPlayerText))
            {
                buttonAddPlayer.Enabled = false;
            }
        }

        public void SetPlayerButtonsEnables(bool enable)
        {
            buttonAddPlayer.Enabled = enable;
            buttonDeletePlayer.Enabled = !enable;
            AcceptButton = enable ? buttonAddPlayer : buttonDeletePlayer;
        }

        private void CheckBoxSelectAll_CheckedChanged(object sender, EventArgs e)
            => _playerListFormModel.CheckBoxSelectAllCheckedChanged(selectAll: checkBoxSelectAll.Checked);

        private void ButtonAddPlayer_Click(object sender, EventArgs e)
            => _playerListFormModel.AddPlayer();

        private void ListBoxPlayers_SelectedIndexChanged(object sender, EventArgs e)
            => _playerListFormModel.ListBoxPlayersSelectedIndexChanged();

        private void ListBoxPlayers_DoubleClick(object sender, EventArgs e)
        {
            if (TextBoxSelectedPlayerText == string.Empty) return;
            _playerListFormModel.DeletePlayer();
        }

        private void ButtonDeletePlayer_Click(object sender, EventArgs e)
            => _playerListFormModel.DeletePlayer();

        private void PlayerListForm_FormClosing(object sender, FormClosingEventArgs e)
            => _playerListFormModel.PlayersListFormClosing();
    }
}
