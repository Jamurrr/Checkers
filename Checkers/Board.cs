using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Board
    {
        private Piece[,] pieces;

        public Board()
        {
            pieces = new Piece[8, 8];
        }

        public void Initialize()
        {
            
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if ((row + col) % 2 != 0 && row < 3)
                    {
                        pieces[row, col] = new Piece("Black", new int[] { row, col });
                    }
                    else if ((row + col) % 2 != 0 && row > 4)
                    {
                        pieces[row, col] = new Piece("White", new int[] { row, col });
                    }
                }
            }
        }

        public Piece GetPiece(int row, int col)
        {
            return pieces[row, col];
        }

        public void MovePiece(int startRow, int startCol, int endRow, int endCol)
        {
            pieces[endRow, endCol] = pieces[startRow, startCol];
            pieces[startRow, startCol] = null;
        }

        public void RemovePiece(int row, int col)
        {
            pieces[row, col] = null;
        }



    }

}
