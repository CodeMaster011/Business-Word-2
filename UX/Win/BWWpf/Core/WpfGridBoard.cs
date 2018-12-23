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
        private int effectiveCardCount;

        public override int EffectiveCardCount => effectiveCardCount;
        public ObservableCollection<ObservableCollection<WpfCard>> CardsUx { get; set; } = new ObservableCollection<ObservableCollection<WpfCard>>();

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
            manager.OnCharacterMove += Manager_OnCharacterMove;

            await InitilizePlayers(manager);
        }

        protected virtual async Task InitilizePlayers(GameManager manager)
        {
            foreach (var c in Characters)
            {
                await c.InitilizeInBoard(manager).ConfigureAwait(false);
            }
        }

        private void Manager_OnCharacterMove(GameManager manager, CharacterMoveArgs args)
        {
            var oldCard = GetWpfCardFromIndex(args.OldLocation);
            if (oldCard != null)
                oldCard.Characters.Remove(args.Character);
            var newCard = GetWpfCardFromIndex(args.NewLocation);
            if (newCard != null)
                newCard.Characters.Add(args.Character);
        }

        protected void AddCardsToUx(IReadOnlyList<Card> cards)
        {
            effectiveCardCount = 0;

            for (int row = 0; row < gridMatrix.GetLength(0); row++)
            {
                if (row >= CardsUx.Count)
                    CardsUx.Add(new ObservableCollection<WpfCard>());

                var currentRow = CardsUx[row];

                for (int col = 0; col < gridMatrix.GetLength(1); col++)
                {
                    (CardType cardType, int index) gridData = InterpreteGridData(gridMatrix[row, col]);
                    if (gridData.index >= 0)
                    {
                        currentRow.Add(new WpfCard(gridData.index - 1, cards[gridData.index - 1]));
                        effectiveCardCount++;
                    }
                    else
                    {
                        currentRow.Add(null);
                    }

                    Debug.WriteLine($"{row},{col}");
                }
            }
        }

        protected WpfCard GetWpfCardFromIndex(int index)
        {
            for (int i = 0; i < CardsUx.Count; i++)
            {
                if (CardsUx[i] == null) continue;

                for (int j = 0; j < CardsUx[i].Count; j++)
                {
                    if (CardsUx[i][j]?.Index == index)
                        return CardsUx[i][j];
                }
            }
            return null;
        }
    }
}
