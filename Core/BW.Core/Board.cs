using System.Collections.Generic;
using System.Threading.Tasks;

namespace BW.Core
{

    public abstract class Board
    {
        public abstract IReadOnlyCollection<Card> Cards { get; }
        public abstract IReadOnlyCollection<Character> Characters { get; }

        public abstract Task InitilizeBoard(GameManager manager);
    }
}
