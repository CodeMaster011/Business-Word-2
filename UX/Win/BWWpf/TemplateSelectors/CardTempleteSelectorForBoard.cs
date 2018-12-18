using BWWpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BWWpf.TemplateSelectors
{
    public class CardTempleteSelectorForBoard : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is WpfCard)
            {
                return element.FindResource("generalCardTemplete") as DataTemplate;
            }

            return element?.FindResource("emptyCardTemplete") as DataTemplate;
        }
    }
}
