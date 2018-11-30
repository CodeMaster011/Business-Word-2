using System.Linq;
using System.Threading.Tasks;

namespace BW.Core
{
    public static class GameManagerExtentions
    {
        public static Task IncreaseCharactorAmount(this GameManager manager, Character character, double amountToChange)
        {
            var newBalance = character.Balance + amountToChange;
            return manager.ChangeCharectorBalance(character, newBalance);
        }

        public static Character[] GetOwners(this GameManager manager, Card card)
        {
            return manager.Board.Characters.Where(c => c.OwnedCards.Contains(card)).ToArray();
        }
    }
}
