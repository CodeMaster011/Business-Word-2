using System.Collections.Generic;
using System.Threading.Tasks;

namespace BW.Core
{
    public abstract class Character
    {
        public abstract IReadOnlyCollection<Card> OwnedCards { get; }


        public virtual double Balance { get; protected set; } = 0d;
        public abstract string Name { get; }
        public virtual bool IsInGame { get; } = true;

        public abstract Task InitilizeInBoard(GameManager manager);
    }
}
