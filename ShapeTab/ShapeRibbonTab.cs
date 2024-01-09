
using Contract;
using Fluent;
using LineShape;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ShapeTab
{
    public class ShapeRibbonTab : ITab
    {

        public override string? name => "Shape";

        public override RibbonTabItem createTab()  
        {
            RibbonTabItem tab = new RibbonTabItem()
            {
                Header = this.name,
                Name = this.name,
            };

            RibbonGroupBox groupBox = new RibbonGroupBox()
            {
                Header = this.name,
            };
            Fluent.Button button = new Fluent.Button() { };

            IShape line = new Line2D();
            button.Header = line.Name;
            button.Click += ShapeButtonClick;

            groupBox.Items.Add(button);
            tab.Groups.Add(groupBox);
            return tab;
        }

        private void ShapeButtonClick(object sender, RoutedEventArgs e)
        {
            this.TargetElement = (new Line2D()).Draw() ;
        }
    }

}
