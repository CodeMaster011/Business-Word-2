using BW.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BW.Imp
{
    public class DefaultPlayer : Player
    {
        private List<Card> ownedCards = new List<Card>();

        public override IReadOnlyCollection<Card> OwnedCards => ownedCards;
        public override string Name { get; }

        public DefaultPlayer(string name, double balance, IEnumerable<Card> ownedCards = null, string color = "Red")
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("message", nameof(name));

            Name = name;

            if(ownedCards != null)
                ownedCards = new List<Card>(ownedCards);
            else
                ownedCards = new List<Card>();
            Balance = balance;
            Color = color;
        }

        public override Task InitilizeInBoard(GameManager manager)
        {
            manager.OnChangeCharectorBalance += Manager_OnChangeCharectorBalance;
            manager.OnChangeOwner += Manager_OnChangeOwner;

            return Task.CompletedTask;
        }

        private void Manager_OnChangeOwner(GameManager manager, ChangeOwnerArgs args)
        {
            if(args.OldOwner == this)
            {
                ownedCards.Remove(args.Card);
            }

            if(args.NewOwner == this)
            {
                ownedCards.Add(args.Card);
            }
        }

        private void Manager_OnChangeCharectorBalance(GameManager manager, Character character, double newBalance)
        {
            if(character == this)
            {
                this.Balance = newBalance;
            }
        }
    }
}
