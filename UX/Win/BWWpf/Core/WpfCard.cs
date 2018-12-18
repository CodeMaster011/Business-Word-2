using BW.Core;
using Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWWpf.Core
{
    public class WpfCard : BindableBase
    {
        public double? Price { get; set; }
        public double? Rent { get; set; }
        public ObservableCollection<Character> Characters { get; set; } = new ObservableCollection<Character>();

        public WpfCard(int index, Card card)
        {
            Card = card;
            Price = Card.Price;
            Rent = Card.Rent;

            card.OnChangePrice += (c, p) => Price = p;
            card.OnChangeRent += (c, r) => Rent = r;
            Index = index;
        }

        public Card Card { get; }
        public int Index { get; }
    }
}
