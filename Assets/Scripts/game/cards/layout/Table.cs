using System;

namespace game.cards.layout
{
    [Serializable]
    public class Table
    {
        public int columns;
        public int rows;

        public Table(int c = 0, int r = 0)
        {
            rows = r;
            columns = c;
        }
    }
}