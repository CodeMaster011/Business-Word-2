using BW.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BW.Imp.Boards
{
    public class GridBoard : Board
    {
        private List<Card> cards;
        private List<Character> characters;
        private GameManager manager;

        public override IReadOnlyCollection<Card> Cards => cards;
        public override IReadOnlyCollection<Character> Characters => characters;

        public GridBoard(List<Card> cards, List<Character> characters)
        {
            this.cards = cards;
            this.characters = characters;
        }

        public override Task InitilizeBoard(GameManager manager)
        {
            this.manager = manager;

            return Task.CompletedTask;
        }
    }
}
