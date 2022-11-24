using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WhatGameToPlay
{
    public partial class GamesListForm : Form
    {
        public MainForm _mainForm = new MainForm();
        private readonly Color _colorWhite;
        private readonly Color _colorBlack;
        private readonly Color _colorBlackLighter;
        private readonly Color _buttonColor;

        public GamesListForm(MainForm MainForm)
        {
            _mainForm = MainForm;
            _colorBlack = _mainForm._colorTexts;
            _colorBlackLighter = MainForm._colorTextsLighter;
            _colorWhite = _mainForm._colorBackgrounds;
            _buttonColor = _mainForm._buttonColor;
            InitializeComponent();
            foreach (string game in MainForm.GamesListCopy)
                listBoxGames.Items.Add(game);
        }

        public void RefreshColors()
        {
            BackColor = _colorBlackLighter;
            List<Label> labels = new List<Label>()
            {
                labelGamesList,
                labelEnterGameName,
                labelMin,
                labelMax
            };
            foreach (Label label in labels)
                label.ForeColor = _colorWhite;
            checkBoxPeopleNumberLimit.ForeColor = _colorWhite;
            List<Button> buttons = new List<Button>()
            {
                buttonAddGame,
                buttonDeleteGame,
                buttonSet
            };
            foreach (Button button in buttons)
            {
                button.ForeColor = _colorWhite;
                button.BackColor = _buttonColor;
            }
            listBoxGames.BackColor = _colorBlack;
            listBoxGames.ForeColor = _colorWhite;
            textBoxGameName.BackColor = _colorBlack;
            textBoxGameName.ForeColor = _colorWhite;
            numericUpDownMax.BackColor = _colorBlack;
            numericUpDownMax.ForeColor = _colorWhite;
            numericUpDownMin.BackColor = _colorBlack;
            numericUpDownMin.ForeColor = _colorWhite;
        }

        private void ButtonAddGame_Click(object sender, EventArgs e)
        {
            File.AppendAllText("GamesList.txt", textBoxGameName.Text + Environment.NewLine);
            _mainForm.RefreshGamesList();
            listBoxGames.Items.Clear();
            foreach (string game in _mainForm.GamesListCopy)
                listBoxGames.Items.Add(game);
            DirectoryInfo directory = new DirectoryInfo("Players");
            FileInfo[] files = directory.GetFiles("*.txt");
            foreach (FileInfo file in files)
                File.AppendAllText(file.FullName, textBoxGameName.Text + "\n");
            _mainForm.RefreshListOfPeople();
            if (_mainForm.ShowMessages)
                _mainForm.MyMessageBox.Show("Game " + textBoxGameName.Text + " is successfully added!");
            //SwitchButtonsEnables();
            SetGameButtonsEnables(enable: false);
            checkBoxPeopleNumberLimit.Enabled = true;
        }

        private void ButtonDeleteGame_Click(object sender, EventArgs e)
        {
            if (_mainForm.ShowConfirmingMessages)
            {
                DialogResult dialogResult = _mainForm.MyMessageBox.Show("Are you sure you want to delete "
                    + textBoxGameName.Text + " from games list?", "Confirmation", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes) DeleteGameFromGameList();
            }
            else DeleteGameFromGameList();
        }

        private void DeleteGameFromGameList()
        {
            foreach (string game in _mainForm.GamesListCopy)
            {
                if (game == textBoxGameName.Text)
                {
                    _mainForm.GamesListCopy.Remove(textBoxGameName.Text);
                    break;
                }
            }
            _mainForm.RefreshListOfPeople();
            File.WriteAllLines("GamesList.txt", _mainForm.GamesListCopy);
            listBoxGames.Items.Clear();
            foreach (string game in _mainForm.GamesListCopy)
                listBoxGames.Items.Add(game);

            SwitchButtonsEnables();
            if (!_mainForm.SaveDeletedGamesData)
            {
                DirectoryInfo directory = new DirectoryInfo("Players");
                FileInfo[] files = directory.GetFiles("*.txt");
                foreach (FileInfo fileInfo in files)
                {
                    File.WriteAllLines(fileInfo.FullName,
                        File.ReadLines(fileInfo.FullName).Where(l => l != textBoxGameName.Text).ToList());
                }
                string path = "Restrictions\\" + textBoxGameName.Text + ".txt";
                if (File.Exists(path)) File.Delete(path);
            }
            textBoxGameName.Clear();
            numericUpDownMin.Value = 1;
            numericUpDownMax.Value = 100;
        }

        private void ListBoxGames_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxGames.SelectedItem != null)
                textBoxGameName.Text = listBoxGames.SelectedItem.ToString();
        }

        public bool CheckGameInGamesList(string gameToCheck)
        {
            foreach (string game in _mainForm.GamesListCopy) 
                if (gameToCheck == game) return true;
            return false;
        }

        private void TextBoxGameName_TextChanged(object sender, EventArgs e)
        {
            if (CheckGameInGamesList(textBoxGameName.Text))
            {
                checkBoxPeopleNumberLimit.Enabled = true;
                checkBoxPeopleNumberLimit.Checked = false;
                numericUpDownMin.Value = 1;
                numericUpDownMax.Value = 100;
                SetGameButtonsEnables(enable: false);
                DirectoryInfo directory = new DirectoryInfo("Restrictions");
                FileInfo[] files = directory.GetFiles("*.txt");
                foreach (FileInfo file in files)
                {
                    if (textBoxGameName.Text == Path.GetFileNameWithoutExtension(file.Name))
                    {
                        string[] lines = File.ReadAllLines(file.FullName);
                        checkBoxPeopleNumberLimit.Checked = true;
                        numericUpDownMin.Value = Convert.ToDecimal(lines[0]);
                        numericUpDownMax.Value = Convert.ToDecimal(lines[1]);
                    }
                }
            }
            else
            {
                buttonSet.Enabled = false;
                numericUpDownMax.Enabled = false;
                numericUpDownMin.Enabled = false;
                checkBoxPeopleNumberLimit.Enabled = false;
                checkBoxPeopleNumberLimit.Checked = false;
                SetGameButtonsEnables(enable: true);
            }
        }

        private void SetGameButtonsEnables(bool enable)
        {
            buttonAddGame.Enabled = enable;
            buttonDeleteGame.Enabled = !enable;
        }

        private void SwitchButtonsEnables()
        {
            buttonAddGame.Enabled = !buttonAddGame.Enabled;
            buttonDeleteGame.Enabled = !buttonDeleteGame.Enabled;
        }

        private void CheckBoxPeopleNumberLimit_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownMin.Enabled = checkBoxPeopleNumberLimit.Checked;
            numericUpDownMax.Enabled = checkBoxPeopleNumberLimit.Checked;
            buttonSet.Enabled = checkBoxPeopleNumberLimit.Checked;
        }

        private void CheckBoxPeopleNumberLimit_Click(object sender, EventArgs e)
        {
            DirectoryInfo directory = new DirectoryInfo("Restrictions");
            FileInfo[] files = directory.GetFiles("*.txt");
            foreach (FileInfo fileInfo in files)
            {
                if (textBoxGameName.Text == Path.GetFileNameWithoutExtension(fileInfo.Name) &&
                    !checkBoxPeopleNumberLimit.Checked && _mainForm.ShowConfirmingMessages)
                {
                    DialogResult dialogResult =
                        _mainForm.MyMessageBox.Show("Are you sure you want to delete "
                        + textBoxGameName.Text + "?", "Confirmation", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes) File.Delete(fileInfo.FullName);
                    else checkBoxPeopleNumberLimit.Checked = true;
                }
            }
        }

        private void ButtonSet_Click(object sender, EventArgs e)
        {
            if (numericUpDownMax.Value > numericUpDownMin.Value)
            {
                string path = "Restrictions\\" + textBoxGameName.Text + ".txt";

                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                    using (TextWriter tw = new StreamWriter(path))
                    {
                        tw.WriteLine(Convert.ToString(numericUpDownMin.Value));
                        tw.WriteLine(Convert.ToString(numericUpDownMax.Value));
                    }
                }
                //else if (File.Exists(path))
                else
                {
                    using (TextWriter tw = new StreamWriter(path))
                    {
                        tw.WriteLine(Convert.ToString(numericUpDownMin.Value));
                        tw.WriteLine(Convert.ToString(numericUpDownMax.Value));
                    }
                }
                if (_mainForm.ShowMessages) _mainForm.MyMessageBox.Show("Restriction Set!");
                _mainForm.ClearAvailableGamesListBox();
            }
            else if (_mainForm.ShowMessages)
            {
                _mainForm.MyMessageBox.Show("The Min value must not exceed the Max value");
            }
        }

        private void FormGamesList_Load(object sender, EventArgs e)
        {
            RefreshColors();
        }
    }
}
