using BW.Core;
using BW.Imp.Boards;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWWpf.Core
{
    [Export(typeof(Board))]
    public class WpfGridBoard : GridBoard
    {
        private readonly string[,] gridMatrix;

        public override IReadOnlyList<Card> Cards => base.Cards;
        public ObservableCollection<ObservableCollection<Card>> CardsUx { get; set; } = new ObservableCollection<ObservableCollection<Card>>();

        /* 4x4 (W X H)
         * [R07][R08][S09][R10]
         * [R06][   ][   ][R11]
         * [S05][   ][   ][R12]
         * [S04][R03][R02][S01]
         * */
        public WpfGridBoard(string[,] gridMatrix, List<Card> cards, List<Character> characters) : base(cards, characters)
        {
            this.gridMatrix = gridMatrix;
        }

        public override async Task InitilizeBoard(GameManager manager)
        {
            var allocatedCards = AllocateCardsToBoard(gridMatrix, Cards);

            //add cards to Ux of CardsUx using grid markings
            AddCardsToUx(allocatedCards);

            await InitilizeCardsToBoard(manager, allocatedCards);
        }

        protected void AddCardsToUx(IReadOnlyList<Card> cards)
        {
            for (int row = 0; row < gridMatrix.GetLength(0); row++)
            {
                if (row >= CardsUx.Count)
                    CardsUx.Add(new ObservableCollection<Card>());

                var currentRow = CardsUx[row];

                for (int col = 0; col < gridMatrix.GetLength(1); col++)
                {
                    (CardType cardType, int index) gridData = InterpreteGridData(gridMatrix[row, col]);
                    if (gridData.index >= 0)
                    {
                        currentRow.Add(cards[gridData.index - 1]);
                    }
                    else
                    {
                        currentRow.Add(null);
                    }

                    Debug.WriteLine($"{row},{col}");
                }
            }
        }
    }
}
