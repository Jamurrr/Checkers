using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Piece
    {
        public string Color { get; }
        public int[] Position { get; set; }
        public bool IsKing { get; set; }

        public Piece(string color, int[] position)
        {
            Color = color;
            Position = position;
            IsKing = false;
        }



    }

}
