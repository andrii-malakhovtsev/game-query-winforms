using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace WhatGameToPlay
{
    public partial class PlayerListForm : Form
    {
        public MainForm _mainForm = new MainForm();
        private readonly Color _colorBackgrounds = Color.Black;
        private readonly Color _colorTexts = Color.White;
        private readonly Color _colorBlackLighter = Color.FromArgb(240, 240, 240);
        private readonly Color _buttonColor = Color.FromArgb(225, 225, 225);

        public PlayerListForm(MainForm MainForm)
        {
            _mainForm = MainForm;
            _colorTexts = _mainForm._colorTexts;
            _colorBlackLighter = MainForm._colorTextsLighter;
            _colorBackgrounds = _mainForm._colorBackgrounds;
            _buttonColor = _mainForm._buttonColor;
            InitializeComponent();
        }

        public void RefreshColors()
        {
            BackColor = _colorBlackLighter;
            labelPlayerList.ForeColor = _colorBackgrounds;
            labelGamesPlaying.ForeColor = _colorBackgrounds;
            listBoxPlayers.BackColor = _colorTexts;
            listBoxPlayers.ForeColor = _colorBackgrounds;
            checkedListBoxGamesPlaying.BackColor = _colorTexts;
            checkedListBoxGamesPlaying.ForeColor = _colorBackgrounds;
            checkBoxSelectAll.ForeColor = _colorBackgrounds;
            textBoxSelectedPlayer.BackColor = _colorTexts;
            textBoxSelectedPlayer.ForeColor = _colorBackgrounds;
            List<Button> listOfButtons = new List<Button>()
            {
                buttonSet,
                buttonAddPerson,
                buttonDeletePerson
            };
            foreach(Button button in listOfButtons)
            {
                button.ForeColor = _colorBackgrounds;
                button.BackColor = _buttonColor;
            }
        }

        private void FormPlayerList_Load(object sender, EventArgs e)
        {
            foreach (var person in _mainForm.People)
                listBoxPlayers.Items.Add(person.Name);
            RefreshColors();
        }

        private void ListBoxPlayers_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPlayers.SelectedItem != null) 
                textBoxSelectedPlayer.Text = listBoxPlayers.SelectedItem.ToString();
        }

        private void SelectPerson()
        {
            checkedListBoxGamesPlaying.Items.Clear();
            foreach (string game in _mainForm.GamesListCopy)
                checkedListBoxGamesPlaying.Items.Add(game);
            for (int index = 0; index < checkedListBoxGamesPlaying.Items.Count; index++)
                checkedListBoxGamesPlaying.SetItemChecked(index, value: true);

            DirectoryInfo directory = new DirectoryInfo("Players");
            FileInfo[] smFiles = directory.GetFiles("*.txt");
            foreach (FileInfo file in smFiles)
            {
                if (textBoxSelectedPlayer.Text == Path.GetFileNameWithoutExtension(file.Name))
                {
                    string[] fileInfo = File.ReadAllLines(file.FullName);
                    foreach (string GameDontPlay in fileInfo)
                    {
                        for (int i = 0; i < checkedListBoxGamesPlaying.Items.Count; i++)
                        {
                            if (GameDontPlay == checkedListBoxGamesPlaying.Items[i].ToString())
                                checkedListBoxGamesPlaying.SetItemChecked(i, false);
                        }
                    }
                }
            }
        }

        private void ButtonGamesPlayingSet_Click(object sender, EventArgs e)
        {
            string path = "Players\\" + textBoxSelectedPlayer.Text + ".txt";
            List<string> ListOfCheckedCheckboxes = new List<string>();
            foreach (var item in checkedListBoxGamesPlaying.CheckedItems)
                ListOfCheckedCheckboxes.Add(item.ToString());

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                using (TextWriter tw = new StreamWriter(path))
                {
                    var ListOfGamesNotPlaying =
                        _mainForm.GamesListCopy.Distinct().Except(ListOfCheckedCheckboxes).ToList();

                    foreach (string GameNotPlyaing in ListOfGamesNotPlaying)
                        tw.WriteLine(GameNotPlyaing.ToString());
                }
            }
            //else if (File.Exists(path))
            else 
            {
                using (TextWriter textWriter = new StreamWriter(path))
                {
                    List<string> gamesNotPlayingList =
                        _mainForm.GamesListCopy.Distinct().Except(ListOfCheckedCheckboxes).ToList();

                    foreach (string gameNotPlaying in gamesNotPlayingList)
                        textWriter.WriteLine(gameNotPlaying.ToString());
                }
            }
            if (_mainForm.ShowMessages)
                _mainForm.MyMessageBox.Show("Список игр для соответствующего человека установлен!");
            _mainForm.RefreshListOfPeople();
            _mainForm.ClearAvailableGamesListBox();
        }

        private void CheckBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxGamesPlaying.Items.Count; i++)
                checkedListBoxGamesPlaying.SetItemChecked(i, checkBoxSelectAll.Checked);
        }

        public bool CheckPersonInPeopleList(string PersonToCheck)
        {
            DirectoryInfo directory = new DirectoryInfo("Players");
            FileInfo[] smFiles = directory.GetFiles("*.txt");
            foreach (FileInfo fi in smFiles)
                if (PersonToCheck == Path.GetFileNameWithoutExtension(fi.Name)) return true;
            return false;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            bool selectedPerson = CheckPersonInPeopleList(textBoxSelectedPlayer.Text);
            if (selectedPerson) SelectPerson();
            else checkedListBoxGamesPlaying.Items.Clear();
            SetPlayerButtonsEnables(enable: !selectedPerson);
        }

        private void ButtonAddPerson_Click(object sender, EventArgs e)
        {
            string path = "Players\\" + textBoxSelectedPlayer.Text + ".txt";
            File.Create(path).Dispose();
            _mainForm.RefreshListOfPeople();

            listBoxPlayers.Items.Clear();
            foreach (Player person in _mainForm.People)
                listBoxPlayers.Items.Add(person.Name);
            SelectPerson();
            if (_mainForm.ShowMessages) _mainForm.MyMessageBox.Show("New person added to the list!");
            SetPlayerButtonsEnables(enable: false);
        }

        private void ButtonDeletePerson_Click(object sender, EventArgs e)
        {
            if (_mainForm.ShowConfirmingMessages)
            {
                DialogResult dialogResult = _mainForm.MyMessageBox.Show("Are you sure you want to delete "
                    + textBoxSelectedPlayer.Text + " from player list?", "Confirmation", 
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                    DeletePersonFromPeopleList();
            }
            else DeletePersonFromPeopleList();
            _mainForm.ClearAvailableGamesListBox();
            textBoxSelectedPlayer.Clear();
        }

        private void DeletePersonFromPeopleList()
        {
            foreach (Player person in _mainForm.People)
            {
                if (person.Name == textBoxSelectedPlayer.Text)
                {
                    _mainForm.People.Remove(person);
                    break;
                }
            }
            File.Delete("Players\\" + textBoxSelectedPlayer.Text + ".txt");
            listBoxPlayers.Items.Clear();
            _mainForm.RefreshListOfPeople();
            foreach (Player person in _mainForm.People)
                listBoxPlayers.Items.Add(person.Name);
            checkedListBoxGamesPlaying.Items.Clear();
            SetPlayerButtonsEnables(enable: true);
        }

        private void SetPlayerButtonsEnables(bool enable)
        {
            buttonAddPerson.Enabled = enable;
            buttonDeletePerson.Enabled = !enable;
        }
    }
}
