using BW.Core;
using BW.Imp;
using BW.Imp.Boards;
using BW.Imp.RedCards;
using BWWpf.Core;
using Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWWpf
{
    public class MainWindowViewModel : BindableBase
    {
        public Character ActivePlayer { get; set; }
        public int Location { get; set; }
        public int Die { get; set; }

        public WpfGridBoard Board { get; set; }
        public DefaultGameManager GameManager { get; private set; }

        public async Task Start()
        {
            var gridData = await GridBoard.ResolveFileData("Board1.txt");
            List<Character> characters = new List<Character>()
            {
                new DefaultPlayer("Karan", 100d, color: "Blue"),
                new DefaultPlayer("Karan2", 100d)
            };
            Board = new WpfGridBoard(gridData,
            cards: new List<Card>()
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
            },
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
        }

        public async Task RollDie()
        {
            var die = await GameManager.RollDie();

            await GameManager.MoveCharactorBy(GameManager.ActiveCharacter, die);
            await GameManager.NextTurn();
        }
    }
}
