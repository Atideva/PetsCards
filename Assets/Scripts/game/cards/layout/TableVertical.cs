using UnityEngine;

namespace game.cards.layout
{
    public class TableVertical : MonoBehaviour
    {
        public Table GetLayout(int cardsTotal)
        {
            var table = new Table();

            switch (cardsTotal)
            {
                case 2: table = new Table(1, 2); break;
                case 4: table = new Table(2, 2); break;
                case 6: table = new Table(2, 3); break;
                case 8: table = new Table(2, 4); break;
                case 10: table = new Table(2, 5); break;
                case 12: table = new Table(3, 4); break;
                case 14:  break;
                case 16: table = new Table(4, 4); break;
                case 18: table = new Table(3, 6); break;
                case 20: table = new Table(4, 5); break;
            }

            return table;
        }

    }
}
