
namespace Checkers
{
    public class CheckersGame
    {
        private Board board;
        private Player[] players;
        private int currentPlayerIndex;

        public Player CurrentPlayer => players[currentPlayerIndex];

        public CheckersGame()
        {
            board = new Board();
            players = new Player[] { new Player(1, "Black"), new Player(2, "White") };
            currentPlayerIndex = 1;
        }

        public void StartGame()
        {
            board.Initialize();
        }

        public bool MakeMove(int startRow, int startCol, int endRow, int endCol)
        {
            Piece piece = board.GetPiece(startRow, startCol);
            if (piece == null || piece.Color != CurrentPlayer.Color)
                return false;

            List<int[]> possibleMoves = GetPossibleMoves(startRow, startCol);
            bool canContinueCapture = false;

            foreach (var move in possibleMoves)
            {
                if (move[0] == endRow && move[1] == endCol)
                {
                    board.MovePiece(startRow, startCol, endRow, endCol);
                    int directionRow = Math.Sign(endRow - startRow);
                    int directionCol = Math.Sign(endCol - startCol);
                    bool captureOccurred = false;

                    for (int i = 1; i < Math.Abs(endRow - startRow); i++)
                    {
                        int capturedRow = startRow + directionRow * i;
                        int capturedCol = startCol + directionCol * i;
                        if (board.GetPiece(capturedRow, capturedCol) != null)
                        {
                            board.RemovePiece(capturedRow, capturedCol);
                            captureOccurred = true;
                        }
                    }

                    // Проверка на превращение в дамку
                    if ((piece.Color == "Black" && endRow == 7) || (piece.Color == "White" && endRow == 0))
                    {
                        piece.IsKing = true;
                    }

                    // Проверка на продолжение хода после поедания шашки
                    if (captureOccurred)
                    {
                        List<int[]> captureMoves = GetCaptureMoves(endRow, endCol);
                        if (captureMoves.Any())
                        {
                            canContinueCapture = true;
                        }
                    }

                    // Если не  может больше кушать
                    if (!canContinueCapture)
                    {
                        SwitchPlayer();
                    }

                    return true;
                }
            }
            return false;
        }

        public List<int[]> GetPossibleMoves(int row, int col)
        {
            List<int[]> moves = new List<int[]>();
            Piece piece = board.GetPiece(row, col);

            if (piece == null)
                return moves;

            int forwardDirection = piece.Color == "Black" ? 1 : -1;
            int backwardDirection = piece.Color == "Black" ? -1 : 1;

            // Обычные ходы
            TryAddMove(row + forwardDirection, col - 1, moves, forwardDirection, -1, piece.IsKing);
            TryAddMove(row + forwardDirection, col + 1, moves, forwardDirection, 1, piece.IsKing);

            // Ходы назад для дамки
            if (piece.IsKing)
            {
                TryAddMove(row + backwardDirection, col - 1, moves, backwardDirection, -1, piece.IsKing);
                TryAddMove(row + backwardDirection, col + 1, moves, backwardDirection, 1, piece.IsKing);
            }

            // Ходы с поеданием шашек
            TryAddCaptureMove(row, col, row + forwardDirection, col - 1, row + forwardDirection * 2, col - 2, moves, piece.IsKing, forwardDirection, -1);
            TryAddCaptureMove(row, col, row + forwardDirection, col + 1, row + forwardDirection * 2, col + 2, moves, piece.IsKing, forwardDirection, 1);
            TryAddCaptureMove(row, col, row + backwardDirection, col - 1, row + backwardDirection * 2, col - 2, moves, piece.IsKing, backwardDirection, -1);
            TryAddCaptureMove(row, col, row + backwardDirection, col + 1, row + backwardDirection * 2, col + 2, moves, piece.IsKing, backwardDirection, 1);

            return moves;
        }

        private void TryAddMove(int row, int col, List<int[]> moves, int directionRow, int directionCol, bool isKing = false)
        {
            if (isKing)
            {
                while (row >= 0 && row < 8 && col >= 0 && col < 8)
                {
                    Piece piece = board.GetPiece(row, col);
                    if (piece == null)
                    {
                        moves.Add(new int[] { row, col });
                        row += directionRow;
                        col += directionCol;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                if (row >= 0 && row < 8 && col >= 0 && col < 8)
                {
                    Piece piece = board.GetPiece(row, col);
                    if (piece == null)
                    {
                        moves.Add(new int[] { row, col });
                    }
                }
            }
        }

        private void TryAddCaptureMove(int startRow, int startCol, int middleRow, int middleCol, int endRow, int endCol, List<int[]> moves, bool isKing, int directionRow, int directionCol)
        {
            if (!isKing && endRow >= 0 && endRow < 8 && endCol >= 0 && endCol < 8)
            {
                Piece startPiece = board.GetPiece(startRow, startCol);
                Piece middlePiece = board.GetPiece(middleRow, middleCol);
                Piece endPiece = board.GetPiece(endRow, endCol);

                if (startPiece != null && middlePiece != null && middlePiece.Color != startPiece.Color && endPiece == null)
                {
                    moves.Add(new int[] { endRow, endCol });
                }
            }
            else if (isKing && endRow >= 0 && endRow < 8 && startRow >= 0 && startRow < 8)
            {
                middleRow = startRow + directionRow;
                middleCol = startCol + directionCol;
                endRow = startRow + 2 * directionRow;
                endCol = startCol + 2 * directionCol;
                int countPieces = 0;

                while (isKing && endRow >= 0 && endRow < 8 && endCol >= 0 && endCol < 8 && countPieces < 1)
                {
                    Piece startPiece = board.GetPiece(startRow, startCol);
                    Piece middlePiece = board.GetPiece(middleRow, middleCol);
                    Piece endPiece = board.GetPiece(endRow, endCol);

                    if (startPiece != null && middlePiece != null && middlePiece.Color != startPiece.Color && endPiece == null)
                    {
                        moves.Add(new int[] { endRow, endCol });
                        break;
                    }
                    else
                    {
                        middleRow += directionRow;
                        middleCol += directionCol;
                        endRow += directionRow;
                        endCol += directionCol;
                    }

                    if (middlePiece != null && endPiece != null)
                    {
                        countPieces++;
                    }
                }
            }
        }

        public List<int[]> GetCaptureMoves(int row, int col)
        {
            List<int[]> moves = new List<int[]>();
            Piece piece = board.GetPiece(row, col);

            if (piece == null)
                return moves;

            int forwardDirection = piece.Color == "Black" ? 1 : -1;
            int backwardDirection = piece.Color == "Black" ? -1 : 1;

            // Кушаем?
            TryAddCaptureMove(row, col, row + forwardDirection, col - 1, row + forwardDirection * 2, col - 2, moves, piece.IsKing, forwardDirection, -1);
            TryAddCaptureMove(row, col, row + forwardDirection, col + 1, row + forwardDirection * 2, col + 2, moves, piece.IsKing, forwardDirection, 1);
            TryAddCaptureMove(row, col, row + backwardDirection, col - 1, row + backwardDirection * 2, col - 2, moves, piece.IsKing, backwardDirection, -1);
            TryAddCaptureMove(row, col, row + backwardDirection, col + 1, row + backwardDirection * 2, col + 2, moves, piece.IsKing, backwardDirection, 1);

            return moves;
        }

        public bool IsGameOver()
        {
            return false;
        }

        public void SwitchPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % 2;
        }

        public Piece GetPiece(int row, int col)
        {
            return board.GetPiece(row, col);
        }
    }
}
