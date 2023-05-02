using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Label = System.Windows.Controls.Label;

namespace TicTacToe
{
    partial class MainWindow
    {
        //Fields 
        private DispatcherTimer timer;
        private int timeElapsed = 11;
        private int numberOfPlayers = 2;
        Random random = new Random();
        public MainWindow()
        {
            // Button Clicks
            InitializeComponent();
            button1.Click += button_Click;
            button2.Click += button_Click;
            button3.Click += button_Click;
            button4.Click += button_Click;
            button5.Click += button_Click;
            button6.Click += button_Click;
            button7.Click += button_Click;
            button8.Click += button_Click;
            button9.Click += button_Click;
            // Timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;

        }

        // Timer Input
        private void timer_Tick(object sender, EventArgs e)
        {
            timeElapsed--;
            label1.Content = "Timer: " + timeElapsed + " Seconds";
            if (timeElapsed == 0)
            {
                timer.Stop();
                MessageBox.Show("Time Ran Out!");
                if (player1Turn)
                {
                    Scoreboard.IncrementPlayer2Score();
                }
                else
                {
                    Scoreboard.IncrementPlayer1Score();
                }
                Scoreboard.UpdateScoreboard(this);
                ResetGame();
            }
        }

        // Board
        private char[,] board = new char[3, 3];
        private bool player1Turn = true;

        // Player Input
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (numberOfPlayers == 2)
            {
                twoPlayerClick(sender, e);

            }
            else
            {
                onePlayerClick(sender, e);
            }
        }

        //Human Player Game Play
        private void twoPlayerClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int row = Grid.GetRow(button);
            int column = Grid.GetColumn(button);

            if (board[row, column] == '\0')
            {
                timeElapsed = 11;
                if (player1Turn)
                {
                    board[row, column] = 'X';
                    button.Content = "X";
                }
                else
                {
                    board[row, column] = 'O';
                    button.Content = "O";
                }

                // Player Turn Signal
                player1Turn = !player1Turn;
                if (player1Turn)
                {
                    turnLabel.Content = "Player 1's Turn";
                }
                else
                {
                    turnLabel.Content = "Human Player's Turn";
                }
                // Check For A Win Or A Draw
                checkForWin();
            }
        }

        //Random Player Game Play
        private void onePlayerClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int row = Grid.GetRow(button);
            int column = Grid.GetColumn(button);

            if (board[row, column] == '\0')
            {
                timeElapsed = 11;
                if (player1Turn)
                {
                    board[row, column] = 'X';
                    button.Content = "X";
                }

                checkForWin();

                int x = random.Next(0, 3);
                int y = random.Next(0, 3);

                while (board[y, x] != '\0')
                {
                    y = random.Next(0, 3);
                    x = random.Next(0, 3);

                }

                board[y, x] = 'O';
                GetButton(y, x).Content = "O";
                checkForWin();

            }
        }

        // Check For Win
        private void checkForWin()
        {
            char winner = '\0';

            // Check Rows
            for (int row = 0; row < 3; row++)
            {
                if (board[row, 0] != '\0' && board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
                {
                    winner = board[row, 0];
                }
            }

            // Check Columns
            for (int column = 0; column < 3; column++)
            {
                if (board[0, column] != '\0' && board[0, column] == board[1, column] && board[1, column] == board[2, column])
                {
                    winner = board[0, column];
                }
            }

            // Check Diagonals
            if (board[0, 0] != '\0' && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            {
                winner = board[0, 0];
            }
            if (board[0, 2] != '\0' && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            {
                winner = board[0, 2];
            }

            if (winner != '\0')
            {
                MessageBox.Show("Player " + (winner == 'O' ? "2" : "1") + " Wins!");

                if (winner == 'X')
                {
                    Scoreboard.IncrementPlayer1Score();
                }
                else
                {
                    Scoreboard.IncrementPlayer2Score();
                }
                Scoreboard.UpdateScoreboard(this);
                ResetGame();
            }
            else if (CheckForDraw())
            {
                MessageBox.Show("Draw!");
                ResetGame();
            }
        }

        // Check For Draw
        private bool CheckForDraw()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (board[row, column] == '\0')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Scoreboard
        public static class Scoreboard
        {
            private static int player1Score;
            private static int player2Score;

            public static void IncrementPlayer1Score()
            {
                player1Score++;
            }

            public static void IncrementPlayer2Score()
            {
                player2Score++;
            }

            public static void Reset()
            {
                player1Score = 0;
                player2Score = 0;
            }

            public static int GetPlayer1Score()
            {
                return player1Score;
            }
            public static int GetPlayer2Score()
            {
                return player2Score;
            }

            public static void UpdateScoreboard(MainWindow window)
            {
                window.Player1ScoreLabel.Content = $"Player 1: {player1Score}";
                window.Player2ScoreLabel.Content = $"Player 2: {player2Score}";
            }
        }

        // Start & New Game Button
        private void StartGame_Click(object sender, EventArgs e)
        {
            StartButton.Content = "New Game";
            Scoreboard.Reset();
            Scoreboard.UpdateScoreboard(this);
            ResetGame();
        }

        // Reset Game
        private void ResetGame()
        {
            player1Turn = true;
            turnLabel.Content = "Player 1's Turn";
            board = new char[3, 3];
            timeElapsed = 11;

            foreach (Button button in grid1.Children)
            {
                button.Content = "";
            }
            timer.Start();
        }

        // Opponent Selection 
        private void OpponentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (playerLabel == null) return;
            var selectedText = (string)((ComboBoxItem)OpponentComboBox.SelectedItem).Content;
            playerLabel.Content = "You selected: " + selectedText;
            Player2label.Content = selectedText;

            if (selectedText == "Random Player")
            {
                numberOfPlayers = 1;
            }
            else
            {
                numberOfPlayers = 2;
            }
        }

        // Get Button Result
        private Button GetButton(int row, int col)
        {
            UIElement ui = grid1.Children.Cast<UIElement>()
                .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);

            return ui as Button;
        }


    }
}