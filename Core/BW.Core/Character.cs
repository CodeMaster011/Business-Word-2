using System.Collections.Generic;
using System.Threading.Tasks;

namespace BW.Core
{

    public abstract class Character
    {
        public abstract IReadOnlyCollection<Card> OwnedCards { get; set; }


        public virtual double Balance { get; private set; } = 0d;
        public abstract string Name { get; }
        public virtual bool IsInGame { get; } = true;
    }
}
