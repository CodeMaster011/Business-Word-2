using BW.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BW.Imp.Boards
{
    // [Export(typeof(Board))]
    public abstract class GridBoard : Board
    {
        private List<Card> cards;
        private List<Character> characters;

        public override string Name { get; } = "Grid Board";
        public override string Description { get; } = "Grid Shaped";
        public override IReadOnlyList<Card> Cards => cards;
        public override IReadOnlyList<Character> Characters => characters;

        public GridBoard(List<Card> cards, List<Character> characters)
        {
            this.cards = cards;
            this.characters = characters;
        }

        protected virtual async Task InitilizeCardsToBoard(GameManager manager, IReadOnlyList<Card> cards)
        {
            var index = 0;
            foreach (var card in cards)
            {
                await card.InitilizeInBoard(manager, this, index);
            }
        }

        protected virtual IReadOnlyList<Card> AllocateCardsToBoard(string[,] gridMatric, IReadOnlyList<Card> cards)
        {
            // if (manager == null) throw new NullReferenceException("Game Manage can not be null.");
            
            var cardAllocationMetadata = new List<(CardType cardType, int index)>();
            for (int row = 0; row < gridMatric.GetLength(0); row++)
            {
                for (int col = 0; col < gridMatric.GetLength(1); col++)
                {
                    (CardType cardType, int index) gridData = InterpreteGridData(gridMatric[row, col]);
                    if(gridData.index >=0)
                        cardAllocationMetadata.Add(gridData);
                }
            }

            cardAllocationMetadata = cardAllocationMetadata.OrderBy(d => d.index).ToList();
            var leftoverCards = cards.ToList();
            var finalCardAllocations = new List<Card>();

            foreach (var item in cardAllocationMetadata)
            {
                var selectedCard = GetCardForIndex(item, leftoverCards);
                finalCardAllocations.Add(selectedCard);
                leftoverCards.Remove(selectedCard);
            }

            return finalCardAllocations;
            // cards[0].InitilizeInBoard(manager, this, )
        }

        protected virtual (CardType cardType, int index) InterpreteGridData(string gridData)
        {
            gridData = gridData.Replace("[", string.Empty).Replace("]", string.Empty).Trim();
            if(gridData.Length != 0 && int.TryParse(gridData.Substring(1), out var index))
            {
                switch (gridData[0])
                {
                    case 'R':
                        return (CardType.Regular, index);
                    case 'S':
                        return (CardType.Special, index);
                    case 'J':
                        return (CardType.Jail, index);
                    default:
                        break;
                }
            }
            return (CardType.Regular, -1);
            //throw new InvalidOperationException("Grid data is not appropriate type.");
        }

        protected virtual Card GetCardForIndex((CardType cardType, int index) gridData, IReadOnlyList<Card> leftoverCards)
        {
            return leftoverCards.FirstOrDefault(c => c.Type == gridData.cardType) 
                ?? leftoverCards.FirstOrDefault(c => c.Type == CardType.Regular);
        }

        public static async Task<string[,]> ResolveFileData(string filePath)
        {
            using (var file = File.OpenText(filePath))
            {
                var firstline = await file.ReadLineAsync();
                var dimentions = GetDimentions(firstline);

                var matrix = new string[dimentions.height, dimentions.width];
                for (int row = 0; row < dimentions.height; row++)
                {
                    var line = await file.ReadLineAsync();
                    var lineData = line.Split(new[] { '[' },StringSplitOptions.RemoveEmptyEntries);

                    if (lineData.Length != dimentions.width)
                        throw new InvalidDataException("The width is not same as data size.");

                    for (int col = 0; col < dimentions.width; col++)
                    {
                        matrix[row, col] = lineData[col].Replace("]", string.Empty);
                    }
                }

                return matrix;
            }

            (int width, int height) GetDimentions(string line)
            {
                var dimentionData = line.Split('x');// 4x4
                if(dimentionData.Length == 2)
                {
                    if (int.TryParse(dimentionData[0], out var width) && int.TryParse(dimentionData[1], out var height))
                    {
                        return (width, height);
                    }
                }
                return (0, 0);
            }
        }
    }
}
