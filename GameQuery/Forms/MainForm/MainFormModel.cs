using System;
using System.Collections.Generic;

namespace WhatGameToPlay
{
    public class MainFormModel
    {
        private readonly MainForm _mainForm;

        public MainFormModel(MainForm mainForm)
        {
            _mainForm = mainForm; // mb all of those initializations to separate class
            Files = new Files();
            Directories = new Directories();
            FilesReader = new FilesReader(this);
            AdvancedMessageBox = new AdvancedMessageBox(FilesReader);
            DialogDisplayer = new DialogDisplayer(mainForm);
            MessageDisplayer = new MessageDisplayer(mainForm);
            FormsTheme = new FormsTheme(this);
        }

        public Files Files { get; }

        public Directories Directories { get; }

        private FilesReader FilesReader { get; } // change later to separate class w initializations

        public FormsTheme FormsTheme { get; }

        public HashSet<Player> Players { get; set; } = new HashSet<Player>();

        public MessageDisplayer MessageDisplayer { get; }

        public DialogDisplayer DialogDisplayer { get; }

        public AdvancedMessageBox AdvancedMessageBox { get; }

        public int CheckBoxTopMeasure => FormHasExtraCheckBoxes ? 5 : 70;

        public int CheckBoxLeftMeasure => FormHasExtraCheckBoxes ? 20 : 25;

        public bool FormHasExtraCheckBoxes
        {
            get
            {
                const int maximumCheckBoxesOnForm = 11;
                return Players.Count > maximumCheckBoxesOnForm;
            }
        }

        public void MainFormLoad()
        {
            if (!FilesReader.StandardFilesExist)
            {
                if (DialogDisplayer.ShowFirstMeetingDialog())
                {
                    CreateStartingFiles();
                }
                else
                {
                    _mainForm.Close();
                }
            }

            _mainForm.RefreshPlayersList();
            SetSavedOptionsFromFile();

            _mainForm.SetSavedColors();
            FormsTheme.SetChosenThemeColors();
            RefreshTheme();
        }

        private void CreateStartingFiles()
        {
            var files = (HashSet<StorageItem>)Files;
            var directories = (HashSet<StorageItem>)Directories;
            files.UnionWith(directories);

            FilesCreator.CreateStartingFiles(files);

            files.Clear();
            directories.Clear();
        }

        private void SetSavedOptionsFromFile()
        {
            string[] currentOptions = Files.Options.CurrentOptions;
            for (int i = 0; i < currentOptions.Length; i++)
            {
                _mainForm.OptionToolStrips[i].Checked = Convert.ToBoolean(currentOptions[i]);
            }
        }

        public void RefreshTheme()
        {
            FormsTheme.ColorControls(form: _mainForm);
            FormsTheme.ColorToolStripMenuItems(_mainForm.AllToolStripMenuItems);
        }

        public void RefreshOptionsToFiles()
        {
            var options = new string[_mainForm.OptionToolStrips.Count];
            for (int i = 0; i < _mainForm.OptionToolStrips.Count; i++)
            {
                options[i] = Convert.ToString(_mainForm.OptionToolStrips[i].Checked);
            }
            Files.Options.WriteToFile(options);
        }

        public void PlayerCheckBoxCheckedChange()
        {
            _mainForm.ListBoxAvailableGames.Items.Clear();

            List<string> gamesAvailable = Files.GamesList.CurrentGamesList;
            RemoveGamesPlayerDoesNotPlayFromGamesList(gamesAvailable);
            RemoveLimitedGamesFromGamesList(gamesAvailable);
            foreach (string game in gamesAvailable)
            {
                _mainForm.ListBoxAvailableGames.Items.Add(game);
            }

            if (_mainForm.CheckedPlayersCount == 0) _mainForm.ListBoxAvailableGames.Items.Clear();
        }

        private void RemoveLimitedGamesFromGamesList(List<string> gamesAvailable)
        {
            if (_mainForm.ConsiderGamePlayersLimits)
            {
                foreach (string limitedGame in Directories.GamesLimits.GetLimitedGamesList(_mainForm.CheckedPlayersCount))
                {
                    gamesAvailable.Remove(limitedGame);
                }
            }
        }

        private void RemoveGamesPlayerDoesNotPlayFromGamesList(List<string> gamesAvailable)
        {
            foreach (Player player in Players)
            {
                if (!player.CheckBox.Checked) continue;
                for (int i = 0; i < gamesAvailable.Count; i++)
                {
                    foreach (string gameNotPlaying in player.UnplayedGames)
                    {
                        gamesAvailable.Remove(gameNotPlaying);
                    }
                }
            }
        }

        public void ListBoxAvailableGamesDoubleClick()
        {
            if (_mainForm.ListBoxAvailableGames.SelectedItem == null) return;

            if (!_mainForm.ShowConfirmationMessages ||
                DialogDisplayer.ShowDeleteAvailableGameDialog(_mainForm.ListBoxAvailableGames.SelectedItem.ToString()))
            {
                DeleteGameFromListBox();
            }
        }

        private void DeleteGameFromListBox()
        {
            while (_mainForm.ListBoxAvailableGames.SelectedItems.Count > 0)
            {
                _mainForm.ListBoxAvailableGames.Items.Remove(_mainForm.ListBoxAvailableGames.SelectedItems[0]);
            }
        }

        public void CreateNewGamesListForm()
        {
            var gamesListForm = new GamesListForm(_mainForm);
            gamesListForm.ShowDialog();
        }

        public void CreateNewPlayersListForm()
        {
            var playerListForm = new PlayersListForm(_mainForm);
            playerListForm.ShowDialog();
        }

        public void TimerTick(bool rouletteInsteadProgressbar)
        {
            const int progressBarSmallIndent = 2, progressBarBigIndent = 3, timerInterval = 5,
                timerMaximumAccelerationInterval = 45, maximumTimerInterval = 200;

            bool changeProgressbarValue = _mainForm.ProgressBar.Value < _mainForm.ProgressBar.Maximum - progressBarBigIndent
                && rouletteInsteadProgressbar;

            if (changeProgressbarValue)
            {
                _mainForm.ProgressBar.Value += _mainForm.Timer.Interval < timerMaximumAccelerationInterval ?
                    progressBarSmallIndent : progressBarBigIndent;
            }

            _mainForm.ListBoxAvailableGames.Focus();
            _mainForm.Timer.Interval += timerInterval;

            if (_mainForm.ListBoxAvailableGames.Items.Count > 0)
            {
                int randomAvailableGame = new Random().Next(0, _mainForm.ListBoxAvailableGames.Items.Count);
                _mainForm.TextBox.Text = Convert.ToString(_mainForm.ListBoxAvailableGames.Items[randomAvailableGame]);
            }

            if (_mainForm.Timer.Interval == maximumTimerInterval)
            {
                StopRandomGameRoulette();
            }
        }

        public void StartRandomGameRoulette(bool rouletteInsteadProgressbar)
        {
            if (_mainForm.ListBoxAvailableGames.Items.Count > 0)
            {
                if (rouletteInsteadProgressbar)
                {
                    _mainForm.ProgressBar.Visible = true;
                    _mainForm.ProgressBar.Value = 0;
                }
                FormsTheme.ColorTextBox(_mainForm.TextBox, win: false);
                _mainForm.SetPictureBoxesVisibility(visible: false);
                _mainForm.SetActiveFormControlsEnables(enable: false);
                const int defaultTimerInterval = 10;
                _mainForm.Timer.Interval = defaultTimerInterval;
                _mainForm.Timer.Enabled = true;
            }
            else if (_mainForm.ShowMessages)
            {
                MessageDisplayer.ShowNoGamesToPlayMessage();
            }
        }

        private void StopRandomGameRoulette()
        {
            _mainForm.SetActiveFormControlsEnables(enable: true);
            _mainForm.Timer.Enabled = false;
            _mainForm.ProgressBar.Visible = false;
            if (_mainForm.CelebrateRandomGame)
            {
                _mainForm.SetPictureBoxesVisibility(visible: true);
            }

            FormsTheme.ColorTextBox(_mainForm.TextBox, win: true);
            MessageDisplayer.ShowGameToPlayMessage(gameToPlay: _mainForm.TextBox.Text);
        }
    }
}
