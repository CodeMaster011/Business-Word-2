using System.Collections.Generic;
using System.Threading.Tasks;

namespace BW.Core
{

    public abstract class Board
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract int EffectiveCardCount { get; }
        public abstract IReadOnlyList<Card> Cards { get; }
        public abstract IReadOnlyList<Character> Characters { get; }

        public abstract Task InitilizeBoard(GameManager manager);
    }
}
