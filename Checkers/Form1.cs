using System;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers
{
    public partial class Form1 : Form
    {
        private CheckersGame game;
        private Button[,] boardButtons;
        private int selectedRow = -1;
        private int selectedCol = -1;

        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
            game = new CheckersGame();
            game.StartGame();
            UpdateBoard();
        }

        private void InitializeBoard()
        {
            const int size = 8;
            const int cellSize = 60;
            boardButtons = new Button[size, size];

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    var button = new Button
                    {
                        Size = new Size(cellSize, cellSize),
                        Location = new Point(col * cellSize, row * cellSize),
                        Tag = new int[] { row, col },
                        Font = new Font(Font.FontFamily, 16),
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderSize = 0 }
                    };

                    button.Click += Cell_Click;
                    Controls.Add(button);
                    boardButtons[row, col] = button;
                }
            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var coords = (int[])button.Tag;
            int row = coords[0];
            int col = coords[1];

            //
            if (selectedRow == -1 && game.GetPiece(row, col) != null && game.GetPiece(row, col).Color == game.CurrentPlayer.Color)
            {
                ClearHighlightedMoves();
                selectedRow = row;
                selectedCol = col;
                HighlightPossibleMoves(row, col);
            }
            //
            else if (selectedRow != -1)
            {
                if (game.MakeMove(selectedRow, selectedCol, row, col))
                {
                    UpdateBoard();
                    selectedRow = -1;
                    selectedCol = -1;
                    ClearHighlightedMoves();
                }
                else if (game.GetPiece(row, col) != null && game.GetPiece(row, col).Color == game.CurrentPlayer.Color)
                {
                    ClearHighlightedMoves();
                    selectedRow = row;
                    selectedCol = col;
                    HighlightPossibleMoves(row, col);
                }
                else
                {
                    MessageBox.Show("Invalid move!");
                }
            }
            else
            {
                ClearHighlightedMoves();
                selectedRow = row;
                selectedCol = col;
                
            }
        }


        private void HighlightPossibleMoves(int row, int col)
        {
            List<int[]> possibleMoves = game.GetPossibleMoves(row, col);
            foreach (var move in possibleMoves)
            {
                boardButtons[move[0], move[1]].BackColor = Color.LightGreen;
            }
        }


        private void ClearHighlightedMoves()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    boardButtons[row, col].BackColor = (row + col) % 2 == 0 ? Color.White : Color.LightGray;
                }
            }
        }

        private void UpdateBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var piece = game.GetPiece(row, col);
                    if (piece != null)
                    {
                        var button = boardButtons[row, col];
                        button.Text = "";

                        if (piece.IsKing)
                        {
                            button.BackgroundImage = Image.FromFile(piece.Color == "Black" ? "E:\\c#\\Checkers\\Checkers\\black_king.png" : "E:\\c#\\Checkers\\Checkers\\white_king.png");
                        }
                        else
                        {
                            button.BackgroundImage = Image.FromFile(piece.Color == "Black" ? "E:\\c#\\Checkers\\Checkers\\black.png" : "E:\\c#\\Checkers\\Checkers\\white.png");
                        }
                        button.BackgroundImageLayout = ImageLayout.Zoom;
                    }
                    else
                    {
                        boardButtons[row, col].Text = "";
                        boardButtons[row, col].BackgroundImage = null;
                    }
                    boardButtons[row, col].BackColor = (row + col) % 2 == 0 ? Color.White : Color.LightGray;
                }
            }
        }
    }
}
