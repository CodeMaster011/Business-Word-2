using BW.Core;
using BW.Imp;
using BW.Imp.Boards;
using BW.Imp.RedCards;
using BWWpf.Core;
using Mvvm;
using Mvvm.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWWpf
{
    public class MainWindowViewModel : BindableBase
    {

        public DelegateCommand RollDieCommand { get; set; }
        public DelegateCommand BuyLandCommand { get; set; }
        public DelegateCommand EndTrunCommand { get; set; }

        public Character ActivePlayer { get; set; }
        public int Location { get; set; }
        public int Die { get; set; }
        public bool IsDiceRolled { get; set; }

        public WpfGridBoard Board { get; set; }
        public DefaultGameManager GameManager { get; private set; }

        public MainWindowViewModel()
        {
            RollDieCommand = new DelegateCommand(() => RollDie().ConfigureAwait(false));
            BuyLandCommand = new DelegateCommand(() => BuyLand().ConfigureAwait(false));
            EndTrunCommand = new DelegateCommand(() => GameManager.NextTurn().ConfigureAwait(false));
        }

        public async Task Start()
        {
            var gridData = await GridBoard.ResolveFileData("Board1.txt");

            List<Card> cards = new List<Card>()
            {
                new DarjeelingRedCard("Kolkata", "Purple", null, 9870, 6000),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
                new DarjeelingRedCard(),
            };

            List<Character> characters = new List<Character>()
            {
                new DefaultPlayer("Karan", 100d, color: "Blue", ownedCards: new[] { cards[1], cards[2] }),
                new DefaultPlayer("Suman", 100d)
            };

            Board = new WpfGridBoard(gridData,
            cards: cards,
            characters: characters);

            GameManager = new DefaultGameManager(Board, characters[0]);
            GameManager.OnChangeActiveCharacter += GameManager_OnChangeActiveCharacter;
            GameManager.OnRollDie += GameManager_OnRollDie;

            await Board.InitilizeBoard(GameManager);
            await GameManager.MoveCharactor(characters[0], 0);
            await GameManager.MoveCharactor(characters[1], 0);
        }

        private void GameManager_OnRollDie(GameManager manager, int number)
        {
            Die = number;
        }

        private void GameManager_OnChangeActiveCharacter(GameManager manager, Character active, Character old)
        {
            ActivePlayer = active;
            Location = manager.GetCharactorLocation(active).GetValueOrDefault(-1);

            IsDiceRolled = false;
        }

        public async Task RollDie()
        {
            if (IsDiceRolled) return;

            var die = await GameManager.RollDie();

            await GameManager.MoveCharactorBy(GameManager.ActiveCharacter, die);

            IsDiceRolled = true;
            // await GameManager.NextTurn();
        }

        public async Task BuyLand()
        {
            if (!IsDiceRolled) return;

            var location = GameManager.GetCharactorLocation(ActivePlayer).GetValueOrDefault(-1);
            var card = Board.Cards[location];
            var owners = GameManager.GetOwners(card);
            if (owners == null || owners.Length == 0)
            {
                await GameManager.SetOwner(ActivePlayer, card).ConfigureAwait(false);
            }
        }
    }
}
