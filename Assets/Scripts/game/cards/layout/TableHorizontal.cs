using UnityEngine;

namespace game.cards.layout
{
    public class TableHorizontal : MonoBehaviour
    {

        /// <summary>
        /// x - columns count, y - lines count
        /// </summary>
        public Table GetLayout(int cardsTotal)
        {
            Table table =  new Table();

            switch (cardsTotal)
            {
                case 2: table = new Table(2, 1); break;
                case 4: table = new Table(4, 1); break;
                case 6: table = new Table(3, 2); break;
                case 8: table = new Table(4, 2); break;
                case 10: table = new Table(5, 2); break;
                case 12: table = new Table(6, 2); break;
                case 14:table = new Table(7, 2); break;
                case 16: table = new Table(4, 4); break;
                case 18: table = new Table(6, 3); break;
                case 20: table = new Table(5, 4); break;
                case 22:  break;
                case 24: table = new Table(6, 4); break;
                case 26: break;
                case 28: table = new Table(7, 4); break;
                case 30: table = new Table(10, 3); break;
                case 32: table = new Table(8, 4); break;
                case 34:  break;
                case 36: table = new Table(9, 4); break;
                case 38:  break;
                case 40: table = new Table(8, 5); break;
                default: break;
            }

            return table;
        }

    }
}
