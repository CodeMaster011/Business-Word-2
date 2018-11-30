using System.Threading.Tasks;

namespace BW.Core
{
    public abstract class Card
    {
        public abstract CardType Type { get; }
        public abstract string Color { get; }
        public abstract string Name { get; }
        public abstract double? Price { get; }
        public abstract double? Rent { get; }
        public virtual string Description { get; }

        public abstract Task InitilizeInBoard(GameManager manager, Board board, int cardLocation);
        // public abstract Task OnCharactorStepIn(GameManager manager, Character character);
    }
}
