using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class MainForm : Form
    {
        private readonly MessageController _messageController;
        private readonly ThemeController _theme;
        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();
        private readonly List<CheckBox> _checkBoxesCopy = new List<CheckBox>();
        private readonly List<ToolStripMenuItem> _colorThemeItems = new List<ToolStripMenuItem>();
        private ToolStripMenuItem[] _optionToolStrips;
        public List<Player> Players { get; set; } = new List<Player>();
        public MyMessageBox MyMessageBox { get; set; }
        public bool ShowConfirmingMessages { get; set; }
        public bool ShowMessages { get; set; }
        public bool SaveDeletedGamesData { get; set; }

        public MainForm()
        {
            InitializeComponent();
            _theme = new ThemeController();
            _messageController = new MessageController(this);
            _colorThemeItems.AddRange(new List<ToolStripMenuItem> {
                whiteToolStripMenuItem,
                darkToolStripMenuItem,
                telegramToolStripMenuItem,
                discordToolStripMenuItem,
                youtubeToolStripMenuItem
            });
            foreach (ToolStripMenuItem themeToolStripMenuItem in _colorThemeItems)
                themeToolStripMenuItem.Click += new EventHandler(SetTheme);
            ToolStripMenuItem[] menuToolStripItems = new ToolStripMenuItem[] {
                editToolStripMenuItem,
                optionsToolStripMenuItem,
                themeToolStripMenuItem
            };
            foreach (ToolStripMenuItem menuToolStripItem in menuToolStripItems)
            {
                menuToolStripItem.DropDownOpening += new EventHandler(MenuToolStripItem_DropDownOpening);
                menuToolStripItem.DropDownClosed += new EventHandler(MenuToolStripItem_DropDownClosed);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!FilesController.StandartFilesExist())
            {
                if (MessageController.ShowFirstMeetDialog())
                    FilesController.CreateStartingFiles();
                else Close();
            }
            RefreshPlayersList();
            SetSavedOptions();
            SetSavedColors();
            _theme.SetChosenThemeColors();
            MyMessageBox = new MyMessageBox(_theme);
            RefreshColors();
        }

        private void MenuToolStripItem_DropDownOpening(object sender, EventArgs e)
        {
            _theme.SetBackgroundForeColor((ToolStripMenuItem)sender);
        }

        private void MenuToolStripItem_DropDownClosed(object sender, EventArgs e)
        {
            _theme.SetTextForeColor((ToolStripMenuItem)sender);
        }

        private void SetSavedColors()
        {
            foreach (ToolStripMenuItem colorTheme in _colorThemeItems)
                if (FilesController.GetCurrentTheme() == colorTheme.Text)
                    colorTheme.Checked = true;
        }

        private void SetSavedOptions()
        {
            _optionToolStrips = new ToolStripMenuItem[] {
                showMessagesToolStripMenuItem,
                showConfirmationMessagesToolStripMenuItem,
                ConsiderGamePlayersLimitsToolStripMenuItem,
                rouletteInsteadProgressbarToolStripMenuItem,
                CelebrateRandomGameToolStripMenuItem,
                SaveDeletedGamesDataToolStripMenuItem
            };
            string[] currentOptions = FilesController.GetOptionsFromFile();
            for (int i = 0; i < currentOptions.Length; i++)
                _optionToolStrips[i].Checked = Convert.ToBoolean(currentOptions[i]);
            ShowMessages = showMessagesToolStripMenuItem.Checked;
            ShowConfirmingMessages = showConfirmationMessagesToolStripMenuItem.Checked;
            SaveDeletedGamesData = SaveDeletedGamesDataToolStripMenuItem.Checked;
        }

        private void RefreshOptionsToFiles()
        {
            string[] options = new string[_optionToolStrips.Length];
            for (int i = 0; i < _optionToolStrips.Length; i++)
                options[i] = Convert.ToString(_optionToolStrips[i].Checked);
            FilesController.AddOptionsToFile(options);
        }

        private void RefreshThemeToFile()
        {
            foreach (ToolStripMenuItem colorTheme in _colorThemeItems)
                if (colorTheme.Checked) FilesController.AddThemeToFile(colorTheme.Text);
        }

        private bool FormHasExtraCheckBoxes()
        {
            int maxCheckBoxesOnFormCount = 10;
            return Players.Count > maxCheckBoxesOnFormCount;
        }

        private void RefreshPlayersList()
        {
            Players.Clear();
            foreach (CheckBox checkbox in _checkBoxesCopy) checkbox.Dispose();
            Players = FilesController.GetPlayersListFromDirectory();
            _checkBoxesCopy.Clear();
            playersPanel.Visible = FormHasExtraCheckBoxes();
            foreach (Player player in Players)
                AddPlayerCheckBox(player);
            _checkBoxes.AddRange(_checkBoxesCopy);
            pictureBoxFireworks.SendToBack();
            RefreshColors();
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

        public void PlayerCheckBoxCheckedChange()
        {
            List<string> currentGames = FilesController.GetGamesListFromFile();
            listBoxAvailableGames.Items.Clear();
            foreach (Player player in Players)
                if (player.CheckBox.Checked)
                    for (int i = 0; i < currentGames.Count; i++)
                        foreach (string gameNotPlaying in player.GamesNotPlaying)
                            currentGames.Remove(gameNotPlaying);
            if (ConsiderGamePlayersLimitsToolStripMenuItem.Checked)
                foreach (string restrictedGame in
                    FilesController.GetRestrictedGamesFromDirectory(GetCheckedPlayersCount()))
                        currentGames.Remove(restrictedGame);
            foreach (string game in currentGames)
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

        private void ButtonRandomAvailableGame_Click(object sender, EventArgs e)
        {
            int defaultTimerInterval = 10;
            if (listBoxAvailableGames.Items.Count > 0)
            {
                if (rouletteInsteadProgressbarToolStripMenuItem.Checked)
                    progressBar.Visible = true;
                progressBar.Value = 0;
                _theme.SetTextBoxForeColor(textBox, win: false);
                pictureBoxSmile.Hide();
                pictureBoxFireworks.Hide();
                buttonRandomAvailableGame.Enabled = false;
                SetToolStripItemsEnables(enable: false);
                timer.Interval = defaultTimerInterval;
                timer.Enabled = true;
                foreach (CheckBox checkBox in _checkBoxes)
                    checkBox.Enabled = false;
            }
            else if (ShowMessages)
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
                _theme.SetTextBoxForeColor(textBox, win: true);
                foreach (CheckBox checkBox in _checkBoxes) checkBox.Enabled = true;
                progressBar.Visible = false;
                _messageController.ShowGameToPlayMessage(gameToPlay: textBox.Text);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void ClearInformation()
        {
            RefreshPlayersList();
            listBoxAvailableGames.Items.Clear();
            textBox.Clear();
        }

        private void ListBoxAvailableGames_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAvailableGames.SelectedItem != null)
            {
                if (showConfirmationMessagesToolStripMenuItem.Checked)
                {
                    if (_messageController.ShowDeleteAvailableGameDialog
                        (listBoxAvailableGames.SelectedItem.ToString()))
                        DeleteGameFromListBox();
                }
                else DeleteGameFromListBox();
            }
        }

        private void DeleteGameFromListBox()
        {
            while (listBoxAvailableGames.SelectedItems.Count > 0)
                listBoxAvailableGames.Items.Remove(listBoxAvailableGames.SelectedItems[0]);
        }

        private void ShowConfirmationMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showMessagesToolStripMenuItem.Checked)
            {
                _messageController.ShowTurnConfirmationMessagesError();
            }
            else
            {
                showConfirmationMessagesToolStripMenuItem.Checked =
                    !showConfirmationMessagesToolStripMenuItem.Checked;
                ShowConfirmingMessages = showConfirmationMessagesToolStripMenuItem.Checked;
            }
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
            GamesListForm formGamesList = new GamesListForm(this, _theme);
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
            PlayerListForm formPlayerList = new PlayerListForm(this, _theme);
            formPlayerList.ShowDialog();
        }

        private void CelebrateRandomGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CelebrateRandomGameToolStripMenuItem.Checked =
                !CelebrateRandomGameToolStripMenuItem.Checked;
            pictureBoxSmile.Visible = false;
            pictureBoxFireworks.Visible = false;
            RefreshOptionsToFiles();
        }

        private void SaveDeletedGamesDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDeletedGamesDataToolStripMenuItem.Checked
                = !SaveDeletedGamesDataToolStripMenuItem.Checked;
            SaveDeletedGamesData = SaveDeletedGamesDataToolStripMenuItem.Checked;
            RefreshOptionsToFiles();
        }

        private void ShowMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showMessagesToolStripMenuItem.Checked
                = !showMessagesToolStripMenuItem.Checked;
            if (showMessagesToolStripMenuItem.Checked)
                showConfirmationMessagesToolStripMenuItem.Checked = true;
            ShowMessages = showMessagesToolStripMenuItem.Checked;
            RefreshOptionsToFiles();
        }

        private void SetTheme(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem colorTheme in _colorThemeItems)
                colorTheme.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;
            RefreshThemeToFile();
            _theme.SetChosenThemeColors();
            RefreshColors();
        }

        private void RefreshColors()
        {
            _theme.SetFormBackgroundColor(this);
            List<Control> foreColorControls = new List<Control>();
            foreColorControls.AddRange(_checkBoxes);
            foreColorControls.Add(labelPresentPlayers);
            foreColorControls.Add(labelAvailableGames);
            _theme.SetControlsForeColor(foreColorControls);
            foreColorControls.Clear();
            _theme.SetButtonColor(buttonRandomAvailableGame);
            Label[] labels = {
                labelPresentPlayers,
                labelAvailableGames
            };
            _theme.SetTextForeColor(labels);
            _theme.SetToolStripBackColor(optionsToolStripMenuItem);
            List<ToolStripMenuItem> toolStripMenuItems = new List<ToolStripMenuItem>();
            foreach (ToolStripMenuItem toolStripMenuItem in menuStrip.Items)
                toolStripMenuItems.Add(toolStripMenuItem);
            toolStripMenuItems.AddRange(new List<ToolStripMenuItem>{
                playerListToolStripMenuItem,
                gameListToolStripMenuItem,
                showMessagesToolStripMenuItem,
                showConfirmationMessagesToolStripMenuItem,
                ConsiderGamePlayersLimitsToolStripMenuItem,
                rouletteInsteadProgressbarToolStripMenuItem,
                CelebrateRandomGameToolStripMenuItem,
                SaveDeletedGamesDataToolStripMenuItem,
                whiteToolStripMenuItem,
                darkToolStripMenuItem,
                telegramToolStripMenuItem,
                discordToolStripMenuItem,
                youtubeToolStripMenuItem});
            _theme.SetToolStripMenuItemsFullColor(toolStripMenuItems);
            toolStripMenuItems.Clear();
            Control[] fullColorControls = {
                menuStrip,
                listBoxAvailableGames,
                textBox,
                playersPanel
            };
            _theme.SetControlsFullColor(fullColorControls);
        }
    }
}
