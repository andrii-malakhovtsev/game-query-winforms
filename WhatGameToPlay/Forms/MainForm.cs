using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class MainForm : Form
    {
        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();
        private readonly List<CheckBox> _checkBoxesCopy = new List<CheckBox>();
        private readonly List<ToolStripMenuItem> _colorThemeItems = new List<ToolStripMenuItem>();
        private readonly List<ToolStripMenuItem> _optionToolStrips = new List<ToolStripMenuItem>();
        private readonly MessageController _messageController;
        public List<Player> Players { get; set; } = new List<Player>();
        public MyMessageBox MyMessageBox { get; set; }

        public MainForm()
        {
            InitializeComponent();
            _messageController = new MessageController(this);
            foreach (ToolStripMenuItem toolStripMenuItem in menuStrip.Items)
            {
                if (toolStripMenuItem.DropDownItems.Count != 0)
                {
                    toolStripMenuItem.DropDownOpening += new EventHandler(MenuToolStripItem_DropDownOpening);
                    toolStripMenuItem.DropDownClosed += new EventHandler(MenuToolStripItem_DropDownClosed);
                    SetToolStripTheme(toolStripMenuItem);
                }
            }
        }

        private void SetToolStripTheme(ToolStripMenuItem toolStripMenuItem)
        {
            bool isToolStripThemeMenuItem = toolStripMenuItem == themeToolStripMenuItem,
                 isToolStripOptionsMenuItem = toolStripMenuItem == optionsToolStripMenuItem;
            if (isToolStripThemeMenuItem || isToolStripOptionsMenuItem)
            {
                foreach (ToolStripMenuItem toolStripItem in toolStripMenuItem.DropDownItems)
                {
                    if (isToolStripThemeMenuItem)
                    {
                        _colorThemeItems.Add(toolStripItem);
                        toolStripItem.Click += new EventHandler(SetThemeFromToolStrip);
                    }
                    else _optionToolStrips.Add(toolStripItem);
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MyMessageBox = new MyMessageBox();
            if (!FilesController.StandartFilesExist())
            {
                if (_messageController.ShowFirstMeetingDialog())
                    FilesController.CreateStartingFiles();
                else Close();
            }
            RefreshPlayersList();
            SetSavedOptionsFromFile();
            SetSavedColors();
            ThemeController.SetChosenThemeColors();
            RefreshTheme();
        }

        private void MenuToolStripItem_DropDownOpening(object sender, EventArgs e)
        {
            ThemeController.SetBackgroundForeColor(sender as ToolStripMenuItem);
        }

        private void MenuToolStripItem_DropDownClosed(object sender, EventArgs e)
        {
            ThemeController.SetTextForeColor(sender as ToolStripMenuItem);
        }

        public bool ShowMessages()
        {
            return showMessagesToolStripMenuItem.Checked;
        }

        public bool ShowConfirmationMessages()
        {
            return showConfirmationMessagesToolStripMenuItem.Checked;
        }

        public bool SaveDeletedGamesData()
        {
            return saveDeletedGamesDataToolStripMenuItem.Checked;
        }

        private void SetSavedColors()
        {
            foreach (ToolStripMenuItem colorTheme in _colorThemeItems)
                if (FilesController.GetCurrentTheme() == colorTheme.Text)
                    colorTheme.Checked = true;
        }

        private void SetSavedOptionsFromFile()
        {
            string[] currentOptions = FilesController.GetOptionsFromFile();
            for (int i = 0; i < currentOptions.Length; i++)
                _optionToolStrips[i].Checked = Convert.ToBoolean(currentOptions[i]);
        }

        private void RefreshOptionsToFiles()
        {
            string[] options = new string[_optionToolStrips.Count];
            for (int i = 0; i < _optionToolStrips.Count; i++)
                options[i] = Convert.ToString(_optionToolStrips[i].Checked);
            FilesController.WriteOptionsToFile(options);
        }

        private void RefreshThemeToFile()
        {
            foreach (ToolStripMenuItem colorTheme in _colorThemeItems)
                if (colorTheme.Checked) FilesController.WriteThemeToFile(colorTheme.Text);
        }

        private bool FormHasExtraCheckBoxes()
        {
            int maximumCheckBoxesOnForm = 11;
            return Players.Count > maximumCheckBoxesOnForm;
        }

        private void RefreshPlayersList()
        {
            Players.Clear();
            foreach (CheckBox checkbox in _checkBoxesCopy) checkbox.Dispose();
            Players = FilesController.GetPlayersListFromDirectory();
            _checkBoxesCopy.Clear();
            playersPanel.Visible = FormHasExtraCheckBoxes();
            playersGroupBox.Visible = FormHasExtraCheckBoxes();
            foreach (Player player in Players)
                AddPlayerCheckBox(player);
            _checkBoxes.AddRange(_checkBoxesCopy);
            pictureBoxFireworks.SendToBack();
            RefreshTheme();
        }

        public void AddPlayerCheckBox(Player player)
        {
            int topMeasure = FormHasExtraCheckBoxes() ? 5 : 70,
                leftMeasure = FormHasExtraCheckBoxes() ? 20 : 25;
            CheckBox checkBox = new CheckBox
            {
                Top = topMeasure + (_checkBoxesCopy.Count * leftMeasure),
                Left = leftMeasure,
                Name = "checkBox " + player.Name,
                Text = player.Name
            };
            checkBox.AutoSize = true;
            checkBox.CheckedChanged += new EventHandler(CheckBox_CheckedChanged);
            player.CheckBox = checkBox;
            checkBox.BringToFront();
            if (FormHasExtraCheckBoxes()) playersPanel.Controls.Add(checkBox);
            else Controls.Add(checkBox);
            _checkBoxesCopy.Add(checkBox);
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PlayerCheckBoxCheckedChange();
        }

        private void PlayerCheckBoxCheckedChange()
        {
            List<string> gamesAvailable = FilesController.GetGamesListFromFile();
            listBoxAvailableGames.Items.Clear();
            foreach (Player player in Players)
                if (player.CheckBox.Checked)
                    for (int i = 0; i < gamesAvailable.Count; i++)
                        foreach (string gameNotPlaying in player.GamesNotPlaying)
                            gamesAvailable.Remove(gameNotPlaying);
            if (ConsiderGamePlayersLimitsToolStripMenuItem.Checked)
                foreach (string limitedGame in
                    FilesController.GetLimitedGamesFromDirectory(GetCheckedPlayersCount()))
                        gamesAvailable.Remove(limitedGame);
            foreach (string game in gamesAvailable)
                listBoxAvailableGames.Items.Add(game);
            if (GetCheckedPlayersCount() == 0) listBoxAvailableGames.Items.Clear();
        }

        private int GetCheckedPlayersCount()
        {
            int checkedCheckBoxesCount = 0;
            foreach (CheckBox checkbox in _checkBoxesCopy)
                if (checkbox.Checked) checkedCheckBoxesCount++;
            return checkedCheckBoxesCount;
        }

        private void ListBoxAvailableGames_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAvailableGames.SelectedItem == null) return;
            if (!showConfirmationMessagesToolStripMenuItem.Checked ||
                _messageController.ShowDeleteAvailableGameDialog(
                    listBoxAvailableGames.SelectedItem.ToString()))
                        DeleteGameFromListBox();
        }

        private void DeleteGameFromListBox()
        {
            while (listBoxAvailableGames.SelectedItems.Count > 0)
                listBoxAvailableGames.Items.Remove(listBoxAvailableGames.SelectedItems[0]);
        }

        private void ShowConfirmationMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showMessagesToolStripMenuItem.Checked)
                _messageController.ShowTurningConfirmationMessagesError();
            else showConfirmationMessagesToolStripMenuItem.Checked =
                    !showConfirmationMessagesToolStripMenuItem.Checked;
            RefreshOptionsToFiles();
        }

        private void ConsiderPlayersLimitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsiderGamePlayersLimitsToolStripMenuItem.Checked =
                !ConsiderGamePlayersLimitsToolStripMenuItem.Checked;
            PlayerCheckBoxCheckedChange();
            RefreshOptionsToFiles();
        }

        private void GamesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GamesListForm formGamesList = new GamesListForm(this);
            formGamesList.ShowDialog();
        }

        private void RouletteInsteadProgressbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rouletteInsteadProgressbarToolStripMenuItem.Checked =
                !rouletteInsteadProgressbarToolStripMenuItem.Checked;
            RefreshOptionsToFiles();
        }

        private void PlayersListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayerListForm formPlayerList = new PlayerListForm(this);
            formPlayerList.ShowDialog();
        }

        private void CelebrateRandomGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CelebrateRandomGameToolStripMenuItem.Checked =
                !CelebrateRandomGameToolStripMenuItem.Checked;
            HidePictureBoxes();
            RefreshOptionsToFiles();
        }

        private void SaveDeletedGamesDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDeletedGamesDataToolStripMenuItem.Checked
                = !saveDeletedGamesDataToolStripMenuItem.Checked;
            RefreshOptionsToFiles();
        }

        private void ShowMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showMessagesToolStripMenuItem.Checked
                = !showMessagesToolStripMenuItem.Checked;
            showConfirmationMessagesToolStripMenuItem.Checked = showMessagesToolStripMenuItem.Checked;
            RefreshOptionsToFiles();
        }

        private void SetThemeFromToolStrip(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem colorTheme in _colorThemeItems)
                colorTheme.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;
            RefreshThemeToFile();
            ThemeController.SetChosenThemeColors();
            RefreshTheme();
        }

        private void RefreshTheme()
        {
            ThemeController.SetFormControlsTheme(form: this);
            List<ToolStripMenuItem> toolStripMenuItems = new List<ToolStripMenuItem>();
            foreach (ToolStripMenuItem toolStripMenuItem in menuStrip.Items)
            {
                toolStripMenuItems.Add(toolStripMenuItem);
                foreach (ToolStripMenuItem toolStripItem in toolStripMenuItem.DropDownItems)
                    toolStripMenuItems.Add(toolStripItem);
            }
            ThemeController.SetToolStripMenuItemsFullColor(toolStripMenuItems);
            toolStripMenuItems.Clear();
        }

        private void ButtonRandomAvailableGame_Click(object sender, EventArgs e)
        {
            int defaultTimerInterval = 10;
            if (listBoxAvailableGames.Items.Count > 0)
            {
                if (rouletteInsteadProgressbarToolStripMenuItem.Checked)
                    progressBar.Visible = true;
                progressBar.Value = 0;
                ThemeController.SetTextBoxForeColor(textBox, win: false);
                pictureBoxSmile.Hide();
                pictureBoxFireworks.Hide();
                buttonRandomAvailableGame.Enabled = false;
                SetToolStripItemsEnables(enable: false);
                timer.Interval = defaultTimerInterval;
                timer.Enabled = true;
                foreach (CheckBox checkBox in _checkBoxes)
                    checkBox.Enabled = false;
            }
            else if (ShowMessages())
            {
                _messageController.ShowNoGamesToPlayMessage();
            }
        }

        private void SetToolStripItemsEnables(bool enable)
        {
            editToolStripMenuItem.Enabled = enable;
            optionsToolStripMenuItem.Enabled = enable;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int progressBarSmallIndent = 2, progressBarBigIndent = 3, timerInterval = 5,
                timerMaximumAccelerationInterval = 45, maximumTimerInterval = 200;
            bool changeProgressbarValue = progressBar.Value < progressBar.Maximum - progressBarBigIndent
                && rouletteInsteadProgressbarToolStripMenuItem.Checked;
            if (changeProgressbarValue)
            {
                progressBar.Value += timer.Interval < timerMaximumAccelerationInterval ?
                    progressBarSmallIndent : progressBarBigIndent;
            }
            listBoxAvailableGames.Focus();
            timer.Interval += timerInterval;
            Random random = new Random();
            int randomAvailableGame = random.Next(listBoxAvailableGames.Items.Count);
            if (listBoxAvailableGames.Items.Count > 0)
                textBox.Text = Convert.ToString(listBoxAvailableGames.Items[randomAvailableGame]);
            if (timer.Interval == maximumTimerInterval)
            {
                SetToolStripItemsEnables(enable: true);
                buttonRandomAvailableGame.Enabled = true;
                timer.Enabled = false;
                if (CelebrateRandomGameToolStripMenuItem.Checked)
                {
                    pictureBoxSmile.Show();
                    pictureBoxFireworks.Show(); pictureBoxFireworks.SendToBack();
                }
                ThemeController.SetTextBoxForeColor(textBox, win: true);
                foreach (CheckBox checkBox in _checkBoxes) checkBox.Enabled = true;
                progressBar.Visible = false;
                _messageController.ShowGameToPlayMessage(gameToPlay: textBox.Text);
            }
        }

        public void ClearInformation()
        {
            HidePictureBoxes();
            RefreshPlayersList();
            listBoxAvailableGames.Items.Clear();
            textBox.Clear();
        }

        private void HidePictureBoxes()
        {
            pictureBoxFireworks.Visible = false;
            pictureBoxSmile.Visible = false;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
