using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class MainForm : Form
    {
        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();
        private readonly List<CheckBox> _checkBoxesCopy = new List<CheckBox>();
        private readonly List<ToolStripMenuItem> _colorThemeItems = new List<ToolStripMenuItem>();
        private readonly List<ToolStripMenuItem> _optionToolStrips = new List<ToolStripMenuItem>();
        private readonly MessageDisplayer _messageDisplayer;
        private List<Player> _players = new List<Player>();

        public MainForm()
        {
            InitializeComponent();
            _messageDisplayer = new MessageDisplayer(this);
            foreach (ToolStripMenuItem toolStripMenuItem in menuStrip.Items)
            {
                if (toolStripMenuItem.DropDownItems.Count == 0) continue;
                toolStripMenuItem.DropDownOpening += new EventHandler(MenuToolStripItem_DropDownOpening);
                toolStripMenuItem.DropDownClosed += new EventHandler(MenuToolStripItem_DropDownClosed);
                SetToolStripTheme(toolStripMenuItem);
            }
        }

        private bool FormHasExtraCheckBoxes
        {
            get
            {
                const int maximumCheckBoxesOnForm = 11;
                return _players.Count > maximumCheckBoxesOnForm;
            }
        }

        private int CheckedPlayersCount { get => _checkBoxesCopy.Count(checkBox => checkBox.Checked); }

        public AdvancedMessageBox AdvancedMessageBox { get; } = new AdvancedMessageBox();

        public MessageDisplayer MessageDisplayer { get => _messageDisplayer; }

        public bool ShowMessages { get => showMessagesToolStripMenuItem.Checked; }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!FilesReader.StandartFilesExist)
            {
                if (_messageDisplayer.ShowFirstMeetingDialog())
                {
                    FilesController.CreateStartingFiles();
                }
                else
                {
                    Close();
                }
            }
            RefreshPlayersList();
            SetSavedOptionsFromFile();
            SetSavedColors();
            ThemeController.SetChosenThemeColors();
            RefreshTheme();
        }

        private void SetToolStripTheme(ToolStripMenuItem toolStripMenuItem)
        {
            bool isToolStripThemeMenuItem = toolStripMenuItem == themeToolStripMenuItem,
                 isToolStripOptionsMenuItem = toolStripMenuItem == optionsToolStripMenuItem;
            if (!isToolStripThemeMenuItem && !isToolStripOptionsMenuItem) return;
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

        private void MenuToolStripItem_DropDownOpening(object sender, EventArgs e)
        {
            ThemeController.SetBackgroundForeColor(sender as ToolStripMenuItem);
        }

        private void MenuToolStripItem_DropDownClosed(object sender, EventArgs e)
        {
            ThemeController.SetTextForeColor(sender as ToolStripMenuItem);
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
            _colorThemeItems.OfType<ToolStripMenuItem>()
                .Where(colorTheme => colorTheme.Text == FilesReader.CurrentThemeFromFile)
                .ToList()
                .ForEach(colorTheme => colorTheme.Checked = true);
        }

        private void SetSavedOptionsFromFile()
        {
            string[] currentOptions = FilesReader.OptionsFromFile;
            for (int i = 0; i < currentOptions.Length; i++)
            {
                _optionToolStrips[i].Checked = Convert.ToBoolean(currentOptions[i]);
            }
        }

        private void RefreshOptionsToFiles()
        {
            var options = new string[_optionToolStrips.Count];
            for (int i = 0; i < _optionToolStrips.Count; i++)
            {
                options[i] = Convert.ToString(_optionToolStrips[i].Checked);
            }
            FilesWriter.WriteOptionsToFile(options);
        }

        private void RefreshThemeToFile()
        {
            _colorThemeItems.OfType<ToolStripMenuItem>()
                .Where(colorTheme => colorTheme.Checked == true)
                .ToList()
                .ForEach(colorTheme => FilesWriter.WriteThemeToFile(colorTheme.Text));
        }

        private void RefreshPlayersList()
        {
            _players.Clear();
            foreach (CheckBox checkbox in _checkBoxesCopy) checkbox.Dispose();
            _players = FilesReader.PlayersFromDirectory;
            _checkBoxesCopy.Clear();
            playersPanel.Visible = FormHasExtraCheckBoxes;
            playersGroupBox.Visible = FormHasExtraCheckBoxes;
            foreach (Player player in _players)
                AddPlayerCheckBox(player);
            _checkBoxes.AddRange(_checkBoxesCopy);
            pictureBoxFireworks.SendToBack();
            RefreshTheme();
        }

        public void AddPlayerCheckBox(Player player)
        {
            int topMeasure = FormHasExtraCheckBoxes ? 5 : 70,
                leftMeasure = FormHasExtraCheckBoxes ? 20 : 25;
            var checkBox = new CheckBox
            {
                Top = topMeasure + (_checkBoxesCopy.Count * leftMeasure),
                Left = leftMeasure,
                Name = "checkBox " + player.Name,
                Text = player.Name,
                AutoSize = true
            };
            checkBox.CheckedChanged += new EventHandler(CheckBox_CheckedChanged);
            player.CheckBox = checkBox;
            checkBox.BringToFront();
            AddCheckBoxToControls(checkBox);
        }

        private void AddCheckBoxToControls(CheckBox checkBox)
        {
            if (FormHasExtraCheckBoxes)
                playersPanel.Controls.Add(checkBox);
            else
                Controls.Add(checkBox);
            _checkBoxesCopy.Add(checkBox);
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PlayerCheckBoxCheckedChange();
        }

        private void PlayerCheckBoxCheckedChange()
        {
            List<string> gamesAvailable = FilesReader.GamesListFromFile;
            listBoxAvailableGames.Items.Clear();
            RemoveGamesPlayerDoesNotPlayFromGamesList(gamesAvailable);
            RemoveLimitedGamesFromGamesList(gamesAvailable);
            foreach (string game in gamesAvailable)
            {
                listBoxAvailableGames.Items.Add(game);
            }
            if (CheckedPlayersCount == 0) listBoxAvailableGames.Items.Clear();
        }

        private void RemoveLimitedGamesFromGamesList(List<string> gamesAvailable)
        {
            if (ConsiderGamePlayersLimitsToolStripMenuItem.Checked)
            {
                foreach (string limitedGame in FilesReader.GetLimitedGamesFromDirectory(CheckedPlayersCount))
                {
                    gamesAvailable.Remove(limitedGame);
                }
            }
        }

        private void RemoveGamesPlayerDoesNotPlayFromGamesList(List<string> gamesAvailable)
        {
            foreach (Player player in _players)
            {
                if (!player.CheckBox.Checked) continue;
                for (int i = 0; i < gamesAvailable.Count; i++)
                {
                    foreach (string gameNotPlaying in player.GamesNotPlaying)
                    {
                        gamesAvailable.Remove(gameNotPlaying);
                    }
                }
            }
        }

        private void ListBoxAvailableGames_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAvailableGames.SelectedItem == null) return;
            if (!showConfirmationMessagesToolStripMenuItem.Checked ||
                _messageDisplayer.ShowDeleteAvailableGameDialog(listBoxAvailableGames.SelectedItem.ToString()))
            {
                DeleteGameFromListBox();
            }
        }

        private void DeleteGameFromListBox()
        {
            while (listBoxAvailableGames.SelectedItems.Count > 0)
            {
                listBoxAvailableGames.Items.Remove(listBoxAvailableGames.SelectedItems[0]);
            }
        }

        private void ShowConfirmationMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showMessagesToolStripMenuItem.Checked)
            {
                _messageDisplayer.ShowTurningConfirmationMessagesError();
            }
            else
            {
                showConfirmationMessagesToolStripMenuItem.Checked =
                    !showConfirmationMessagesToolStripMenuItem.Checked;
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
            var gamesListForm = new GamesListForm(this);
            gamesListForm.ShowDialog();
        }

        private void RouletteInsteadProgressbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rouletteInsteadProgressbarToolStripMenuItem.Checked =
                !rouletteInsteadProgressbarToolStripMenuItem.Checked;
            RefreshOptionsToFiles();
        }

        private void PlayersListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var playerListForm = new PlayersListForm(this);
            playerListForm.ShowDialog();
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
            ThemeController.SetToolStripMenuItemsFullColor(GetAllToolStripMenuItems());
        }

        private List<ToolStripMenuItem> GetAllToolStripMenuItems()
        {
            var toolStripMenuItems = new List<ToolStripMenuItem>();
            foreach (ToolStripMenuItem toolStripMenuItem in menuStrip.Items)
            {
                toolStripMenuItems.Add(toolStripMenuItem);
                foreach (ToolStripMenuItem toolStripItem in toolStripMenuItem.DropDownItems)
                {
                    toolStripMenuItems.Add(toolStripItem);
                }
            }
            return toolStripMenuItems;
        }

        private void ButtonRandomAvailableGame_Click(object sender, EventArgs e)
        {
            StartRandomGameRoulette();
        }

        private void SetActiveFormControlsEnables(bool enable)
        {
            buttonRandomAvailableGame.Enabled = enable;
            SetToolStripItemsEnables(enable: enable);
            foreach (CheckBox checkBox in _checkBoxes)
            {
                checkBox.Enabled = enable;
            }
        }

        private void SetToolStripItemsEnables(bool enable)
        {
            editToolStripMenuItem.Enabled = enable;
            optionsToolStripMenuItem.Enabled = enable;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            const int progressBarSmallIndent = 2, progressBarBigIndent = 3, timerInterval = 5,
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
            if (listBoxAvailableGames.Items.Count > 0)
            {
                int randomAvailableGame = new Random().Next(0, listBoxAvailableGames.Items.Count);
                textBox.Text = Convert.ToString(listBoxAvailableGames.Items[randomAvailableGame]);
            }
            if (timer.Interval == maximumTimerInterval)
            {
                StopRandomGameRoulette();
            }
        }
        private void StartRandomGameRoulette()
        {
            const int defaultTimerInterval = 10;
            if (listBoxAvailableGames.Items.Count > 0)
            {
                if (rouletteInsteadProgressbarToolStripMenuItem.Checked)
                {
                    progressBar.Visible = true;
                    progressBar.Value = 0;
                }
                ThemeController.SetTextBoxForeColor(textBox, win: false);
                SetPictureBoxesVisibility(visible: false);
                SetActiveFormControlsEnables(enable: false);
                timer.Interval = defaultTimerInterval;
                timer.Enabled = true;
            }
            else if (ShowMessages)
            {
                _messageDisplayer.ShowNoGamesToPlayMessage();
            }
        }

        private void StopRandomGameRoulette()
        {
            SetActiveFormControlsEnables(enable: true);
            timer.Enabled = false;
            progressBar.Visible = false;
            if (CelebrateRandomGameToolStripMenuItem.Checked)
            {
                SetPictureBoxesVisibility(visible: true);
            }
            ThemeController.SetTextBoxForeColor(textBox, win: true);
            _messageDisplayer.ShowGameToPlayMessage(gameToPlay: textBox.Text);
        }

        private void SetPictureBoxesVisibility(bool visible)
        {
            if (visible)
            {
                pictureBoxSmile.Show();
                pictureBoxFireworks.Show();
                pictureBoxFireworks.SendToBack();
            }
            else
            {
                pictureBoxSmile.Hide();
                pictureBoxFireworks.Hide();
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
