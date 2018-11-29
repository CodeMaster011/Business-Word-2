using System.Threading.Tasks;

namespace BW.Core
{
    public abstract class GameManager
    {
        public event CharacterMoveHandler OnCharacterMove;
        public event ChangeTurnHandler OnChangeTurn;
        public event RollDieHandler OnRollDie;
        public event ChangeCharectorAmountHandler OnChangeCharectorAmount;
        public event ChangeOwnerHandler OnChangeOwner;


        public abstract Task MoveCharactor(Character character, int newLocation);
        public abstract Task ChangeTurnTo(Character character);
        public abstract Task NextTurn();
        public abstract Task<int> RollDie();


        public abstract Task ChangeCharectorAmount(Character character, double newAmount);
        public abstract Task SetOwner(Character newOwner, Card card);
        public abstract Task RemoveOwner(Character oldOwner, Card card);
        // public abstract Task MortgageCard(Character owner, Character mortgageTo, Card card);

    }

    public delegate void CharacterMoveHandler(GameManager manager, CharacterMoveArgs args);
    public delegate void ChangeTurnHandler(GameManager manager, ChangeTurnArgs args);
    public delegate void RollDieHandler(GameManager manager, int number);
    public delegate void ChangeCharectorAmountHandler(GameManager manager, Character character, int amount);
    public delegate void ChangeOwnerHandler(GameManager manager, ChangeOwnerArgs args);

    public class ChangeOwnerArgs
    {
        public ChangeOwnerArgs(Character oldOwner, Character newOwner, Character card)
        {
            OldOwner = oldOwner;
            NewOwner = newOwner;
            Card = card;
        }

        public Character OldOwner { get; }
        public Character NewOwner { get; }
        public Character Card { get; }
    }

    public class ChangeTurnArgs
    {
        public ChangeTurnArgs(Character oldCharacter, Character newCharacter)
        {
            OldCharacter = oldCharacter;
            NewCharacter = newCharacter;
        }

        public Character OldCharacter { get; }
        public Character NewCharacter { get; }
    }

    public class CharacterMoveArgs
    {
        public CharacterMoveArgs(Character character, int oldLocation, int newLocation)
        {
            Character = character;
            OldLocation = oldLocation;
            NewLocation = newLocation;
        }

        public Character Character { get; }
        public int OldLocation { get; }
        public int NewLocation { get; }
    }
}
