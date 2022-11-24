using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace WhatGameToPlay
{
    public partial class MainForm : Form
    {
        private int _checkedPeopleCount = 0;
        private string[] _fileRead;
        private ToolStripMenuItem[] _optionToolStrips;
        private readonly List<CheckBox> _checkBoxesList = new List<CheckBox>();
        private readonly List<CheckBox> _checkBoxesListCopy = new List<CheckBox>();
        private readonly List<ToolStripMenuItem> _colorThemeItems = new List<ToolStripMenuItem>();
        public MyMessageBox MyMessageBox { get; set; }
        public List<string> ListOfGames { get; set; } = new List<string>();
        public List<string> GamesListCopy { get; set; } = new List<string>();
        public List<Player> People { get; set; } = new List<Player>();
        public Color _colorBackgrounds = Color.Black;
        public Color _colorTexts = Color.White;
        public Color _colorTextsLighter = Color.FromArgb(240, 240, 240);
        public Color _buttonColor = Color.FromArgb(225, 225, 225);
        public bool ShowConfirmingMessages { get; set; }
        public bool ShowMessages { get; set; } = true;
        public bool SaveDeletedGamesData { get; set; } = false;

        public MainForm()
        {
            InitializeComponent();
            string[] fileRead = File.ReadAllLines("GamesList.txt");
            foreach (string game in fileRead)
            {
                ListOfGames.Add(game);
                GamesListCopy.Add(game);
            }
            _colorThemeItems.AddRange(new List<ToolStripMenuItem> {
                whiteToolStripMenuItem,
                darkToolStripMenuItem,
                telegramToolStripMenuItem,
                discordToolStripMenuItem,
                youtubeToolStripMenuItem}
            );
            RefreshListOfPeople();
            SetSavedOptions();
            SetSavedColors();
            SetChosenThemeColors();
        }

        private void SetSavedColors()
        {
            _fileRead = File.ReadAllLines("Theme.txt");
            foreach (ToolStripMenuItem item in _colorThemeItems)
                if (_fileRead[0] == item.Text) item.Checked = true;
        }

        private void SetSavedOptions()
        {
            _fileRead = File.ReadAllLines("Options.txt");
            _optionToolStrips = new ToolStripMenuItem[] 
            {
                showMessagesToolStripMenuItem,
                showConfirmationMessagesToolStripMenuItem,
                takeIntoAccountPeopleNumberToolStripMenuItem,
                rouletteInsteadProgressbarToolStripMenuItem,
                TurnOffGameCelebrationToolStripMenuItem,
                SaveDeletedGamesDataToolStripMenuItem
            };
            for (int i = 0; i < _fileRead.Length; i++)
                _optionToolStrips[i].Checked = Convert.ToBoolean(_fileRead[i]);
            ShowMessages = showMessagesToolStripMenuItem.Checked;
            ShowConfirmingMessages = showConfirmationMessagesToolStripMenuItem.Checked;
            SaveDeletedGamesData = SaveDeletedGamesDataToolStripMenuItem.Checked;
        }

        private void RefreshOptionsToFiles()
        {
            string[] stringOptionsList = new string[_optionToolStrips.Length];
            for (int i = 0; i < _optionToolStrips.Length; i++)
                stringOptionsList[i] = Convert.ToString(_optionToolStrips[i].Checked);
            File.WriteAllLines("Options.txt", stringOptionsList);
        }

        private void RefreshThemeToFile()
        {
            foreach (ToolStripMenuItem item in _colorThemeItems)
                if(item.Checked) File.WriteAllText("Theme.txt", item.Text);
        }

        public void RefreshGamesList()
        {
            GamesListCopy.Clear();
            _fileRead = File.ReadAllLines("GamesList.txt");
            foreach (string line in _fileRead) GamesListCopy.Add(line);
        }

        public void RefreshListOfPeople()
        {
            People.Clear();
            DirectoryInfo directory = new DirectoryInfo("Players");
            FileInfo[] files = directory.GetFiles("*.txt");
            foreach (CheckBox checkbox in _checkBoxesListCopy) checkbox.Dispose();
            foreach (FileInfo fileInfo in files)
            {
                List<string> gamesNotPlaying = new List<string>();
                _fileRead = File.ReadAllLines(fileInfo.FullName);
                foreach (string GameDontPlay in _fileRead)
                    gamesNotPlaying.Add(GameDontPlay);
                People.Add(new Player(Path.GetFileNameWithoutExtension(fileInfo.Name), gamesNotPlaying));
            }
            _checkBoxesListCopy.Clear();
            foreach (Player person in People)
                AddCheckBox(person);
            _checkBoxesList.AddRange(_checkBoxesListCopy);
            pictureBoxFireworks.SendToBack();
            RefreshColors();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MyMessageBox = new MyMessageBox(this);
            pictureBoxSmile.Hide();
            pictureBoxFireworks.Hide();
            textBox.ReadOnly = true;
            textBox.BackColor = SystemColors.Window;
            RefreshColors();
        }

        public void AddCheckBox(Player person)
        {
            int topMeasure = 70, leftMeasure = 25;
            CheckBox checkBox = new CheckBox
            {
                Top = topMeasure + (_checkBoxesListCopy.Count * leftMeasure),
                Left = leftMeasure,
                Name = ("checkBox " + person.Name),
                Text = person.Name
            };
            checkBox.CheckedChanged += new EventHandler(CheckBox_CheckedChanged);
            person.CheckBox = checkBox;
            checkBox.BringToFront();
            Controls.Add(checkBox);
            _checkBoxesListCopy.Add(checkBox);
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PlayerCheckBoxCheckedChange();
        }

        public void PlayerCheckBoxCheckedChange()
        {
            _checkedPeopleCount = 0;
            foreach (CheckBox checkbox in _checkBoxesListCopy)
                if (checkbox.Checked) _checkedPeopleCount++;
            listBoxAvailableGames.Items.Clear();
            ListOfGames = new List<string>() { };
            foreach (var element in GamesListCopy) ListOfGames.Add(element);
            foreach (Player person in People)
            {
                if (person.CheckBox.Checked)
                {
                    for (int i = 0; i < ListOfGames.Count; i++)
                    {
                        foreach (string gameNotPlaying in person.GamesNotPlaying)
                            if (ListOfGames[i] == gameNotPlaying) ListOfGames[i] = null;
                    }
                }
            }
            if (takeIntoAccountPeopleNumberToolStripMenuItem.Checked)
            {
                DirectoryInfo directory = new DirectoryInfo("Restrictions");
                FileInfo[] files = directory.GetFiles("*.txt");
                foreach (FileInfo fileInfo in files)
                {
                    string[] lines = File.ReadAllLines(fileInfo.FullName);
                    if (_checkedPeopleCount < Convert.ToInt32(lines[0]))
                        MakeGameFromListNull(Path.GetFileNameWithoutExtension(fileInfo.Name));
                    if (_checkedPeopleCount > Convert.ToInt32(lines[1]))
                        MakeGameFromListNull(Path.GetFileNameWithoutExtension(fileInfo.Name));
                }
            }
            foreach (string game in ListOfGames)
                if (game != null) listBoxAvailableGames.Items.Add(game);
            int uncheckedPeopleCount = 0;
            foreach (Player person in People)
                if (!person.CheckBox.Checked) uncheckedPeopleCount++;
            if (uncheckedPeopleCount == People.Count) listBoxAvailableGames.Items.Clear();
        }

        private void MakeGameFromListNull(string Game)
        {
            for (int i = 0; i < ListOfGames.Count; i++)
                if (ListOfGames[i] == Game) ListOfGames[i] = null;
        }

        private void ButtonRandomAvailableGame_Click(object sender, EventArgs e)
        {
            Color defaultButtonForeColor = Color.Black;
              int defaultTimerInterval = 10;

            if (listBoxAvailableGames.Items.Count > 0)
            {
                if (rouletteInsteadProgressbarToolStripMenuItem.Checked) 
                    progressBar.Visible = true;
                progressBar.Value = 0;
                textBox.ForeColor = defaultButtonForeColor;
                pictureBoxSmile.Hide();
                pictureBoxFireworks.Hide();
                timer.Interval = defaultTimerInterval;
                buttonRandomAvailableGame.Enabled = false;
                timer.Enabled = true;
                foreach (CheckBox checkBox in _checkBoxesList) 
                    checkBox.Enabled = false;
            }
            else if (ShowMessages)
            {
               MyMessageBox.Show("You don't have games to play (Bad ending)");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int progressBarSmallIndent = 2, progressBarBigIndent = 3, timerInterval = 5, 
                timerMaximumAccelerationInterval = 45, maximumTimerInterval = 200;
            bool changeProgressbarValue = progressBar.Value < progressBar.Maximum - progressBarBigIndent
                && rouletteInsteadProgressbarToolStripMenuItem.Checked;

            if (changeProgressbarValue)
            {
                progressBar.Value = timer.Interval < timerMaximumAccelerationInterval ? 
                    progressBarSmallIndent : progressBarBigIndent;
            }
            textBox.ForeColor = _colorBackgrounds;
            listBoxAvailableGames.Focus();
            timer.Interval += timerInterval;
            Random random = new Random();
            int randomAvailableGame = random.Next(listBoxAvailableGames.Items.Count);
            if (listBoxAvailableGames.Items.Count > 0)
                textBox.Text = Convert.ToString(listBoxAvailableGames.Items[randomAvailableGame]);
            if (timer.Interval == maximumTimerInterval)
            {
                buttonRandomAvailableGame.Enabled = true;
                timer.Enabled = false;
                if (!TurnOffGameCelebrationToolStripMenuItem.Checked)
                {
                    pictureBoxSmile.Show();
                    pictureBoxFireworks.Show(); pictureBoxFireworks.SendToBack();
                }
                textBox.ForeColor = Color.Green;
                foreach (CheckBox checkBox in _checkBoxesList) checkBox.Enabled = true;
                progressBar.Visible = false;
                string messageBoxToShow = "Let's go play " + textBox.Text + "!";
                MyMessageBox.Show(messageBoxToShow);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void ClearAvailableGamesListBox()
        {
            listBoxAvailableGames.Items.Clear();
        }

        private void ListBoxAvailableGames_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxAvailableGames.SelectedItem != null)
            {
                if (showConfirmationMessagesToolStripMenuItem.Checked)
                {
                    DialogResult dialogResult = MyMessageBox.Show("Are you sure you want to delete "
                        + listBoxAvailableGames.SelectedItem.ToString()
                        + " from available games list?", "Confirmation", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes) DeleteGameFromListBox();
                }
                else DeleteGameFromListBox();
            }
        }

        private void DeleteGameFromListBox()
        {
            while (listBoxAvailableGames.SelectedItems.Count > 0)
                listBoxAvailableGames.Items.Remove(listBoxAvailableGames.SelectedItems[0]);
        }

        private void отображатьСообщенияПодтвержденияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showMessagesToolStripMenuItem.Checked)
            {
                MyMessageBox.Show("You can't turn confirmation messages off while (all) messages are on");
            }
            else
            {
                showConfirmationMessagesToolStripMenuItem.Checked =
                    !showConfirmationMessagesToolStripMenuItem.Checked;
                ShowConfirmingMessages = showConfirmationMessagesToolStripMenuItem.Checked;
            }
            RefreshOptionsToFiles();
        }

        private void TakeIntoAccountPeopleNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            takeIntoAccountPeopleNumberToolStripMenuItem.Checked =
                !takeIntoAccountPeopleNumberToolStripMenuItem.Checked;
            PlayerCheckBoxCheckedChange();
            RefreshOptionsToFiles();
        }

        private void игруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GamesListForm formGamesList = new GamesListForm(this);
            formGamesList.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rouletteInsteadProgressbarToolStripMenuItem.Checked = 
                !rouletteInsteadProgressbarToolStripMenuItem.Checked;
            RefreshOptionsToFiles();
        }

        private void человекаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayerListForm formPlayerList = new PlayerListForm(this);
            formPlayerList.ShowDialog();
        }

        private void выключитьПразднованиеИгрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TurnOffGameCelebrationToolStripMenuItem.Checked =
                !TurnOffGameCelebrationToolStripMenuItem.Checked;
            pictureBoxSmile.Visible = false;
            pictureBoxFireworks.Visible = false;
            RefreshOptionsToFiles();
        }

        private void сохранятьДанныеОбУдалённыхИграхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDeletedGamesDataToolStripMenuItem.Checked
                = !SaveDeletedGamesDataToolStripMenuItem.Checked;
            SaveDeletedGamesData = SaveDeletedGamesDataToolStripMenuItem.Checked;
            RefreshOptionsToFiles();
        }

        private void отображатьСообщенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showMessagesToolStripMenuItem.Checked
                = !showMessagesToolStripMenuItem.Checked;
            if (showMessagesToolStripMenuItem.Checked)
                showConfirmationMessagesToolStripMenuItem.Checked = true;
            ShowMessages = showMessagesToolStripMenuItem.Checked;
            RefreshOptionsToFiles();
        }

        private void SetThemeColors(Color ColorWhite, Color ColorBlack, Color ColorBlackLighter,
            Color ButtonColor)
        {
            _colorBackgrounds = ColorWhite;
            _colorTexts = ColorBlack;
            _colorTextsLighter = ColorBlackLighter;
            _buttonColor = ButtonColor;
        }

        private void SetChosenThemeColors()
        {
            // change for switch
            if(whiteToolStripMenuItem.Checked)
            {
                SetThemeColors(Color.Black, Color.White, Color.FromArgb(240, 240, 240),
                    Color.FromArgb(225, 225, 225));
            }
            if (darkToolStripMenuItem.Checked)
            {
                SetThemeColors(Color.White, Color.FromArgb(50, 46, 52), Color.FromArgb(70, 65, 72),
                    Color.FromArgb(56, 52, 57));
            }
            if (telegramToolStripMenuItem.Checked)
            {
                SetThemeColors(Color.FromArgb(245, 245, 245), Color.FromArgb(14, 22, 33),
                    Color.FromArgb(23, 33, 43), Color.FromArgb(32, 43, 54));
            }
            if (discordToolStripMenuItem.Checked)
            {
                SetThemeColors(Color.FromArgb(241, 236, 235), Color.FromArgb(47, 49, 54),
                    Color.FromArgb(54, 57, 63), Color.FromArgb(66, 70, 77));
            }
            if (youtubeToolStripMenuItem.Checked)
            {
                SetThemeColors(Color.FromArgb(254, 254, 254), Color.FromArgb(24, 24, 24),
                    Color.FromArgb(33, 33, 33), Color.FromArgb(56, 56, 56));
            }
        }

        private void SetTheme(object sender)
        {
            foreach (ToolStripMenuItem item in _colorThemeItems)
                item.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;

            SetChosenThemeColors();
            RefreshColors();
            RefreshThemeToFile();
        }
        // can do all theme toolStrips to List and give Click event += SetTheme();
        private void WhiteThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTheme(sender);
        }

        private void тёмнаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTheme(sender);
        }

        private void telegramцветаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTheme(sender);
        }

        private void discordцветаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTheme(sender);
        }

        private void youtubeцветаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTheme(sender);
        }

        public void RefreshColors()
        {
            BackColor = _colorTextsLighter;
            textBox.ForeColor = _colorBackgrounds;
            foreach (CheckBox checkBox in _checkBoxesList)
            {
                checkBox.ForeColor = _colorBackgrounds;
                progressBar.ForeColor = _colorTexts;
            }
            listBoxAvailableGames.BackColor = _colorTexts;
            listBoxAvailableGames.ForeColor = _colorBackgrounds;
            buttonRandomAvailableGame.BackColor = _buttonColor;
            buttonRandomAvailableGame.ForeColor = _colorBackgrounds;
            textBox.ForeColor = _colorBackgrounds;
            labelPresentPeople.ForeColor = _colorBackgrounds;
            labelAvailableGames.ForeColor = _colorBackgrounds;
            menuStrip.ForeColor = _colorBackgrounds;
            optionsToolStripMenuItem.BackColor = _colorTextsLighter;
            foreach (ToolStripMenuItem element in menuStrip.Items)
            {
                element.ForeColor = _colorBackgrounds;
                element.BackColor = _colorTexts;
            }
            List<Control> listOfBackColor = new List<Control>()
            {
                menuStrip,
                listBoxAvailableGames,
                textBox
            };
            foreach (Control item in listOfBackColor) item.BackColor = _colorTexts;
            List<ToolStripMenuItem> toolStripItems = new List<ToolStripMenuItem>()
            {
                peopleListToolStripMenuItem,
                gameListToolStripMenuItem,
                showMessagesToolStripMenuItem,
                showConfirmationMessagesToolStripMenuItem,
                takeIntoAccountPeopleNumberToolStripMenuItem,
                rouletteInsteadProgressbarToolStripMenuItem,
                TurnOffGameCelebrationToolStripMenuItem,
                SaveDeletedGamesDataToolStripMenuItem,
                whiteToolStripMenuItem,
                darkToolStripMenuItem,
                telegramToolStripMenuItem,
                discordToolStripMenuItem,
                youtubeToolStripMenuItem
            };
            foreach (ToolStripMenuItem toolStripItem in toolStripItems)
            {
                toolStripItem.BackColor = _colorTexts;
                toolStripItem.ForeColor = _colorBackgrounds;
            };
        }

        // can do all theme toolStrips to List and give events

        private void добавитьToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if(!whiteToolStripMenuItem.Checked) editToolStripMenuItem.ForeColor = _colorTexts;
        }

        private void добавитьToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            if(!whiteToolStripMenuItem.Checked) editToolStripMenuItem.ForeColor = _colorBackgrounds;
        }

        private void опцииToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            if (!whiteToolStripMenuItem.Checked) optionsToolStripMenuItem.ForeColor = _colorBackgrounds;
        }

        private void опцииToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (!whiteToolStripMenuItem.Checked) optionsToolStripMenuItem.ForeColor = _colorTexts;
        }

        private void темаToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (!whiteToolStripMenuItem.Checked) themeToolStripMenuItem.ForeColor = _colorTexts;
        }

        private void темаToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            if (!whiteToolStripMenuItem.Checked) themeToolStripMenuItem.ForeColor = _colorBackgrounds;
        }
    }
}