using BW.Core;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Imp.RedCards
{
    [Export(typeof(Card))]
    public class DarjeelingRedCard : Card
    {
        private double current_Rent = 300d;

        private int cardLocationInBoard;

        public override CardType Type { get; } = CardType.Regular;
        public override string Color { get; } = "Red";
        public override string Name { get; } = "Darjeeling";
        public override double? Price { get; } = 1200d;
        public override double? Rent => current_Rent;
        public override string Description { get; } = "Come to feel the winter in summer.";

        public override Task InitilizeInBoard(GameManager manager, Board board, int cardLocation)
        {
            cardLocationInBoard = cardLocation;

            manager.OnCharacterMove += Manager_OnCharacterMove;
            return Task.CompletedTask;
        }

        private async void Manager_OnCharacterMove(GameManager manager, CharacterMoveArgs args)
        {
            var charactorOnCard = args.Character;

            if (args.NewLocation == cardLocationInBoard)
            {
                // One player is on card
                
                // Is any player owns this card
                var owners = manager.GetOwners(this);
                if(owners != null && owners.Length > 0 && !owners.Contains(charactorOnCard))
                {
                    //Owner(s) found
                    await manager.IncreaseCharactorAmount(charactorOnCard, -current_Rent); // charge current player

                    foreach (var owner in owners)
                        await manager.IncreaseCharactorAmount(owner, current_Rent / owners.Length); // increase owns amounts
                }
            }
        }
    }
}
