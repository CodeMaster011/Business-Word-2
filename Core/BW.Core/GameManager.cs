using System.Threading.Tasks;

namespace BW.Core
{
    public abstract class GameManager
    {
        public abstract Board Board { get; }

        public event CharacterMoveHandler OnCharacterMove;
        public event ChangeTurnHandler OnChangeTurn;
        public event RollDieHandler OnRollDie;
        public event ChangeCharectorBalanceHandler OnChangeCharectorBalance;
        public event ChangeOwnerHandler OnChangeOwner;
        public event ChangeActiveCharacterHandler OnChangeActiveCharacter;



        public abstract Task MoveCharactor(Character character, int newLocation);
        public abstract Task ChangeTurnTo(Character character);
        public abstract Task NextTurn();
        public abstract Task<int> RollDie();
        public abstract int? GetCharactorLocation(Character character);


        public abstract Task ChangeCharectorBalance(Character character, double newBalance);
        public abstract Task SetOwner(Character newOwner, Card card);
        public abstract Task RemoveOwner(Character oldOwner, Card card);
        // public abstract Task MortgageCard(Character owner, Character mortgageTo, Card card);

        protected void RaiseOnCharacterMove(CharacterMoveArgs args) => OnCharacterMove?.Invoke(this, args);
        protected void RaiseOnChangeTurn(ChangeTurnArgs args) => OnChangeTurn?.Invoke(this, args);
        protected void RaiseOnRollDie(int number) => OnRollDie?.Invoke(this, number);
        protected void RaiseOnChangeCharectorBalance(Character character, double newBalance) => OnChangeCharectorBalance?.Invoke(this, character, newBalance);
        protected void RaiseOnChangeOwner(ChangeOwnerArgs args) => OnChangeOwner?.Invoke(this, args);
        protected void RaiseOnChangeActiveCharacter(Character active, Character old) => OnChangeActiveCharacter?.Invoke(this, active, old);

    }

    public delegate void CharacterMoveHandler(GameManager manager, CharacterMoveArgs args);
    public delegate void ChangeTurnHandler(GameManager manager, ChangeTurnArgs args);
    public delegate void RollDieHandler(GameManager manager, int number);
    public delegate void ChangeCharectorBalanceHandler(GameManager manager, Character character, double newBalance);
    public delegate void ChangeOwnerHandler(GameManager manager, ChangeOwnerArgs args);
    public delegate void ChangeActiveCharacterHandler(GameManager manager, Character active, Character old);

    public class ChangeOwnerArgs
    {
        public ChangeOwnerArgs(Character oldOwner, Character newOwner, Card card)
        {
            OldOwner = oldOwner;
            NewOwner = newOwner;
            Card = card;
        }

        public Character OldOwner { get; }
        public Character NewOwner { get; }
        public Card Card { get; }
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
