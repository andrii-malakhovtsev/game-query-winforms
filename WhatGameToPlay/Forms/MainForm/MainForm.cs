using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class MainForm : Form
    {
        private readonly MainFormModel _mainFormModel;
        private readonly List<CheckBox> _checkBoxes = new List<CheckBox>();
        private readonly List<CheckBox> _checkBoxesCopy = new List<CheckBox>();
        private readonly List<ToolStripMenuItem> _colorThemeItems = new List<ToolStripMenuItem>();
        private readonly List<ToolStripMenuItem> _optionToolStrips = new List<ToolStripMenuItem>();

        public MainForm()
        {
            InitializeComponent();
            _mainFormModel = new MainFormModel(this);
            Model = _mainFormModel;
            InitializeToolStripMenuItems();
        }

        private void InitializeToolStripMenuItems()
        {
            foreach (ToolStripMenuItem toolStripMenuItem in menuStrip.Items)
            {
                if (toolStripMenuItem.DropDownItems.Count == 0) continue;
                toolStripMenuItem.DropDownOpening += new EventHandler(MenuToolStripItem_DropDownOpening);
                toolStripMenuItem.DropDownClosed += new EventHandler(MenuToolStripItem_DropDownClosed);
                SetToolStripTheme(toolStripMenuItem);
            }
        }

        public MainFormModel Model { get; }

        public List<ToolStripMenuItem> AllToolStripMenuItems
        {
            get
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
        }

        public List<ToolStripMenuItem> OptionToolStrips  => _optionToolStrips;

        public ListBox ListBoxAvailableGames => listBoxAvailableGames;

        public AdvancedMessageBox AdvancedMessageBox => _mainFormModel.AdvancedMessageBox;

        public DialogDisplayer DialogDisplayer => _mainFormModel.DialogDisplayer;

        public MessageDisplayer MessageDisplayer => _mainFormModel.MessageDisplayer;

        public Timer Timer => timer;

        public ProgressBar ProgressBar => progressBar;

        public TextBox TextBox => textBox;

        public int CheckedPlayersCount => _checkBoxesCopy.Count(checkBox => checkBox.Checked);

        public bool ShowMessages => showMessagesToolStripMenuItem.Checked;

        public bool SaveDeletedGamesData => saveDeletedGamesDataToolStripMenuItem.Checked;

        public bool ShowConfirmationMessages => showConfirmationMessagesToolStripMenuItem.Checked;

        public bool ConsiderGamePlayersLimits => ConsiderGamePlayersLimitsToolStripMenuItem.Checked;

        public bool CelebrateRandomGame => CelebrateRandomGameToolStripMenuItem.Checked;

        private void MainForm_Load(object sender, EventArgs e) => _mainFormModel.MainFormLoad();

        public void RefreshPlayersList()
        {
            _mainFormModel.Players.Clear();
            foreach (CheckBox checkbox in _checkBoxesCopy) checkbox.Dispose();

            _mainFormModel.Players = _mainFormModel.Directories.Players.PlayersList;
            _checkBoxesCopy.Clear();

            playersPanel.Visible = _mainFormModel.FormHasExtraCheckBoxes;
            playersGroupBox.Visible = _mainFormModel.FormHasExtraCheckBoxes;

            foreach (Player player in _mainFormModel.Players)
                AddPlayerCheckBox(player);

            _checkBoxes.AddRange(_checkBoxesCopy);
            pictureBoxFireworks.SendToBack();
            _mainFormModel.RefreshTheme();
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
            _mainFormModel.FormsTheme.ColorToolStripMenuItem(sender as ToolStripMenuItem);
        }

        private void MenuToolStripItem_DropDownClosed(object sender, EventArgs e)
        {
            FormsTheme.ColorToolStripMenuItemDropDowns(sender as ToolStripMenuItem);
        }

        public void SetSavedColors()
        {
            _colorThemeItems.OfType<ToolStripMenuItem>()
                .Where(colorTheme => colorTheme.Text == _mainFormModel.Files.Theme.CurrentThemeName)
                .ToList()
                .ForEach(colorTheme => colorTheme.Checked = true);
        }

        private void RefreshThemeToFile()
        {
            _colorThemeItems.OfType<ToolStripMenuItem>()
                .Where(colorTheme => colorTheme.Checked == true)
                .ToList()
                .ForEach(colorTheme => _mainFormModel.Files.Theme.WriteToFile(colorTheme.Text));
        }

        private void AddPlayerCheckBox(Player player)
        {
            int topMeasure = _mainFormModel.CheckBoxTopMeasure,
                leftMeasure = _mainFormModel.CheckBoxLeftMeasure;

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
            if (_mainFormModel.FormHasExtraCheckBoxes)
                playersPanel.Controls.Add(checkBox);
            else
                Controls.Add(checkBox);
            _checkBoxesCopy.Add(checkBox);
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _mainFormModel.PlayerCheckBoxCheckedChange();
        }

        private void ListBoxAvailableGames_DoubleClick(object sender, EventArgs e)
        {
            _mainFormModel.ListBoxAvailableGamesDoubleClick();
        }

        private void ShowConfirmationMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showMessagesToolStripMenuItem.Checked)
            {
                MessageDisplayer.ShowTurningConfirmationMessagesError();
            }
            else
            {
                showConfirmationMessagesToolStripMenuItem.Checked =
                    !showConfirmationMessagesToolStripMenuItem.Checked;
            }
            _mainFormModel.RefreshOptionsToFiles();
        }

        private void ConsiderPlayersLimitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsiderGamePlayersLimitsToolStripMenuItem.Checked =
                !ConsiderGamePlayersLimitsToolStripMenuItem.Checked;
            _mainFormModel.PlayerCheckBoxCheckedChange();
            _mainFormModel.RefreshOptionsToFiles();
        }

        private void GamesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainFormModel.CreateNewGamesListForm();
        }

        private void RouletteInsteadProgressbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rouletteInsteadProgressbarToolStripMenuItem.Checked =
                !rouletteInsteadProgressbarToolStripMenuItem.Checked;
            _mainFormModel.RefreshOptionsToFiles();
        }

        private void PlayersListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainFormModel.CreateNewPlayersListForm();
        }

        private void CelebrateRandomGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CelebrateRandomGameToolStripMenuItem.Checked =
                !CelebrateRandomGameToolStripMenuItem.Checked;
            HidePictureBoxes();
            _mainFormModel.RefreshOptionsToFiles();
        }

        private void SaveDeletedGamesDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDeletedGamesDataToolStripMenuItem.Checked
                = !saveDeletedGamesDataToolStripMenuItem.Checked;
            _mainFormModel.RefreshOptionsToFiles();
        }

        private void ShowMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showMessagesToolStripMenuItem.Checked
                = !showMessagesToolStripMenuItem.Checked;
            showConfirmationMessagesToolStripMenuItem.Checked = showMessagesToolStripMenuItem.Checked;
            _mainFormModel.RefreshOptionsToFiles();
        }

        private void SetThemeFromToolStrip(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem colorTheme in _colorThemeItems)
                colorTheme.Checked = false;

            ((ToolStripMenuItem)sender).Checked = true;

            RefreshThemeToFile();
            _mainFormModel.FormsTheme.SetChosenThemeColors();
            _mainFormModel.RefreshTheme();
        }

        private void ButtonRandomAvailableGame_Click(object sender, EventArgs e)
        {
            _mainFormModel.StartRandomGameRoulette(rouletteInsteadProgressbarToolStripMenuItem.Checked);
        }

        public void SetActiveFormControlsEnables(bool enable)
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
            _mainFormModel.TimerTick(rouletteInsteadProgressbar: rouletteInsteadProgressbarToolStripMenuItem.Checked);
        }

        public void SetPictureBoxesVisibility(bool visible)
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

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Close();
    }
}
