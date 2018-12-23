using System;
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

        public event ChangePriceHandler OnChangePrice;
        public event ChangeRentHandler OnChangeRent;
        public event Action<Card, Character, double?> OnRentCharged;

        public abstract Task InitilizeInBoard(GameManager manager, Board board, int cardLocation);
        // public abstract Task OnCharactorStepIn(GameManager manager, Character character);

        protected void RaiseOnChangePrice(double? newPrice) => OnChangePrice?.Invoke(this, newPrice);
        protected void RaiseOnChangeRent(double? newRent) => OnChangeRent?.Invoke(this, newRent);
        protected void RaiseOnRentCharged(Character character, double? rent) => OnRentCharged?.Invoke(this, character, rent);
    }

    public delegate void ChangePriceHandler(Card card, double? newPrice);
    public delegate void ChangeRentHandler(Card card, double? newRent);
}
