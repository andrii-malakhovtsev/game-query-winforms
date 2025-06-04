using System;

namespace WhatGameToPlay
{
    public class MessageDisplayer
    {
        private readonly MainForm _mainForm;

        public MessageDisplayer(MainForm mainform) => _mainForm = mainform;

        private void ShowMainFormMessage(string message) => _mainForm.AdvancedMessageBox.Show(message);

        private void ShowOptionalMainFormMessage(string message)
        {
            if (_mainForm.ShowMessages) 
                ShowMainFormMessage(message);
        }

        public void ShowTurningConfirmationMessagesError()
        {
            ShowMainFormMessage("You can't turn showing confirmation messages off while showing " +
                "(all) messages are on");
        }

        public void ShowNoGamesToPlayMessage()
        {
            ShowMainFormMessage("You don't have games to play (Bad ending)");
        }

        private string GetRandomGameToPlayPhrase(string gameToPlay)
        {
            string[] gameToPlayPhrases =
            {
                $"Let's go play {gameToPlay}!",
                $"Yoooo, is it {gameToPlay} that we gonna play?",
                $"The best choice for today is...  {gameToPlay}!",
                $"No way we are going to play {gameToPlay} rn!",
                $"Hey chads, we are going to {gameToPlay}!"
            };
            int randomIndex = new Random().Next(0, gameToPlayPhrases.Length);
            return gameToPlayPhrases[randomIndex];
        }

        public void ShowGameToPlayMessage(string gameToPlay)
        {
            ShowMainFormMessage(GetRandomGameToPlayPhrase(gameToPlay));
        }

        public void ShowPlayersLimitsErrorMessage()
        {
            ShowMainFormMessage("The Min value must not exceed the Max value");
        }

        public void ShowGameAddedToListMessage(string gameName)
        {
            ShowOptionalMainFormMessage($"Game {gameName} is successfully added!");
        }

        public void ShowPlayerAddedToListMessage(string playerName)
        {
            ShowOptionalMainFormMessage(playerName + " is added to the list!");
        }
    }
}
