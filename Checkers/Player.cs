using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Player
    {
        public int PlayerId { get; }
        public string Color { get; }

        public Player(int playerId, string color)
        {
            PlayerId = playerId;
            Color = color;
        }



    }

}
