using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using LogisticsMapWindow;
using logisticsMagnificationWindow;
using logisticsTools;

namespace Logistics_Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private mapWindow mapWin;
        private magnificationWindow magWin;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
            mapPalette.newPaletteItemSelected += mapPalette_newPaletteItemSelected;
            mapWin = new mapWindow();
            mapWin.Show();
            magWin = new magnificationWindow();
            magWin.Show();
        }

        /// <summary>
        /// Used to get palette information to the bing map window
        /// </summary>
        /// <param name="Item"></param>
        void mapPalette_newPaletteItemSelected(logisticsConstants.paletteItems Item)
        {
            mapWin.selectedItem = Item;
        }

        private void frontRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                (sender as Rectangle).ContextMenu.IsEnabled = true;
                (sender as Rectangle).ContextMenu.PlacementTarget = (sender as Rectangle);
                (sender as Rectangle).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
                (sender as Rectangle).ContextMenu.IsOpen = true;
            }
            catch
            {
                //do nothing
                return;
            }
        } 

        public string getCurrentRockSelection()
        {
            return mapPalette.currentItem.ToString();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            switch ((string)item.Uid)
            {
                case "newFront":
                    MessageBox.Show((string)item.Uid);
                    break;
                case "magFront":
                    MessageBox.Show((string)item.Uid);
                    break;
                case "newRight":
                    MessageBox.Show((string)item.Uid);
                    break;
                case "magRight":
                    MessageBox.Show((string)item.Uid);
                    break;
                case "newRear":
                    MessageBox.Show((string)item.Uid);
                    break;
                case "magRear":
                    MessageBox.Show((string)item.Uid);
                    break;
                case "newLeft":
                    MessageBox.Show((string)item.Uid);
                    break;
                case "magLeft":
                    MessageBox.Show((string)item.Uid);
                    break;
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void centerMapButton_Click(object sender, RoutedEventArgs e)
        {
            mapWin.centerOnNasa();
        }




    }
}
