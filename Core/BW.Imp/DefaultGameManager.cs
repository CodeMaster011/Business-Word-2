using BW.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Imp
{
    public class DefaultGameManager : GameManager
    {
        private Random random = new Random();

        private Dictionary<Character, AdditionalCharInfo> charDetails;
        public override Board Board { get; }
        public Character ActiveCharacter { get; private set; }

        public DefaultGameManager(Board board, Character activePlayer)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            ActiveCharacter = activePlayer ?? throw new ArgumentNullException(nameof(activePlayer));

            var index = 0;
            foreach (var charator in board.Characters)
                charDetails.Add(charator, new AdditionalCharInfo { TrunNumber = index++ });
        }

        public override Task ChangeCharectorBalance(Character character, double newBalance)
        {
            RaiseOnChangeCharectorBalance(character, newBalance);
            return Task.CompletedTask;
        }

        public override Task ChangeTurnTo(Character character)
        {
            var _activeCharactor = ActiveCharacter;
            ActiveCharacter = character;
            RaiseOnChangeTurn(new ChangeTurnArgs(_activeCharactor, character));

            return Task.CompletedTask;
        }

        public override Task MoveCharactor(Character character, int newLocation)
        {
            var oldLocation = charDetails[character].Location;
            charDetails[character].Location = newLocation;
            RaiseOnCharacterMove(new CharacterMoveArgs(character, oldLocation, newLocation));

            return Task.CompletedTask;
        }

        public override Task NextTurn()
        {
            var _activePlayer = ActiveCharacter;
            var nextTurnNumber = charDetails[_activePlayer].TrunNumber + 1 % charDetails.Count;
            var nextActivePlayer = charDetails.First(p => p.Value.TrunNumber == nextTurnNumber).Key;
            ActiveCharacter = nextActivePlayer;

            RaiseOnChangeTurn(new ChangeTurnArgs(_activePlayer, nextActivePlayer));

            return Task.CompletedTask;
        }

        public override Task RemoveOwner(Character oldOwner, Card card)
        {
            var owners = this.GetOwners(card);
            if (!owners.Contains(oldOwner))
                throw new InvalidOperationException($"This Character ({oldOwner.Name}) is not an owner of the card ({card.Name})");

            RaiseOnChangeOwner(new ChangeOwnerArgs(oldOwner, null, card));

            return Task.CompletedTask;
        }

        public override Task<int> RollDie()
        {
            var dieValue = random.Next(1, 12);
            RaiseOnRollDie(dieValue);

            return Task.FromResult(dieValue);
        }

        public override Task SetOwner(Character newOwner, Card card)
        {
            RaiseOnChangeOwner(new ChangeOwnerArgs(null, newOwner, card));

            return Task.CompletedTask;
        }


        private class AdditionalCharInfo
        {
            public int Location { get; set; } = 0;
            public int TrunNumber { get; set; } = 0;
        }
    }
}
