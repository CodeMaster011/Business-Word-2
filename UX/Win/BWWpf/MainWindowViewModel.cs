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
        public WpfGridBoard Board { get; set; }

        public async Task Start()
        {
            var gridData = await GridBoard.ResolveFileData("Board1.txt");
            List<Character> characters = new List<Character>()
            {
                new DefaultPlayer("Karan", 100d),
                new DefaultPlayer("Karan2", 100d)
            };
            Board = new WpfGridBoard(gridData,
            cards: new List<Card>()
            {
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
                new DarjeelingRedCard(),
            },
            characters: characters);

            var gameManager = new DefaultGameManager(Board, characters[0]);

            await Board.InitilizeBoard(gameManager);
        }
    }
}
