using System;
using System.Collections.Generic;

namespace WhatGameToPlay
{
    public class MainFormModel
    {
        private readonly MainForm _mainForm;
        private readonly MessageDisplayer _messageDisplayer;
        private List<Player> _players = new List<Player>();

        public MainFormModel(MainForm mainForm)
        {
            _mainForm = mainForm;
            _messageDisplayer = new MessageDisplayer(mainForm);
        }

        public List<Player> Players { get => _players; set => _players = value; }

        public MessageDisplayer MessageDisplayer  => _messageDisplayer; 

        public AdvancedMessageBox AdvancedMessageBox { get; } = new AdvancedMessageBox();

        public int CheckBoxTopMeasure => FormHasExtraCheckBoxes ? 5 : 70;

        public int CheckBoxLeftMeasure => FormHasExtraCheckBoxes ? 20 : 25;

        public bool FormHasExtraCheckBoxes
        {
            get
            {
                const int maximumCheckBoxesOnForm = 11;
                return _players.Count > maximumCheckBoxesOnForm;
            }
        }

        public void MainFormLoad()
        {
            if (!FilesReader.StandartFilesExist)
            {
                if (_messageDisplayer.ShowFirstMeetingDialog())
                {
                    FilesController.CreateStartingFiles();
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

        private void SetSavedOptionsFromFile()
        {
            string[] currentOptions = FilesReader.OptionsFromFile;
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
            FilesWriter.WriteOptionsToFile(options);
        }

        public void PlayerCheckBoxCheckedChange()
        {
            List<string> gamesAvailable = FilesReader.GamesListFromFile;
            _mainForm.ListBoxAvailableGames.Items.Clear();
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
                foreach (string limitedGame in FilesReader.GetLimitedGamesFromDirectory(_mainForm.CheckedPlayersCount))
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

        public void ListBoxAvailableGamesDoubleClick()
        {
            if (_mainForm.ListBoxAvailableGames.SelectedItem == null) return;
            if (!_mainForm.ShowConfirmationMessages ||
                _messageDisplayer.ShowDeleteAvailableGameDialog(_mainForm.ListBoxAvailableGames.SelectedItem.ToString()))
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
            const int defaultTimerInterval = 10;
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
                _mainForm.Timer.Interval = defaultTimerInterval;
                _mainForm.Timer.Enabled = true;
            }
            else if (_mainForm.ShowMessages)
            {
                _messageDisplayer.ShowNoGamesToPlayMessage();
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
            _messageDisplayer.ShowGameToPlayMessage(gameToPlay: _mainForm.TextBox.Text);
        }
    }
}
