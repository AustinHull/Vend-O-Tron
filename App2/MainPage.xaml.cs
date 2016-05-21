using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Input;
using Windows.Devices.Input;
using Windows.UI.Core;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        Windows.Foundation.Point mousePos;
        PointerPoint mousePoint;
        CoreCursor cursor;
        CoreCursorType cType = CoreCursorType.Arrow;
        
        
        public MainPage()
        {
            this.InitializeComponent();

            this.VendFinderApp.PointerPressedOverride += VendFinderApp_PointerPressedOverride;

            //programLoop();
        }

        void VendFinderApp_PointerPressedOverride(object sender, PointerRoutedEventArgs e)
        {
            Bing.Maps.Location loc = new Bing.Maps.Location();
            this.VendFinderApp.TryPixelToLocation(e.GetCurrentPoint(this.VendFinderApp).Position, out loc);
            Bing.Maps.Pushpin pushPin = new Bing.Maps.Pushpin();
            pushPin.SetValue(Bing.Maps.MapLayer.PositionProperty, loc);
            this.VendFinderApp.Children.Add(pushPin);

            // Add code to prompt user to enter VendMachine metadata...
            TextBox inputPrompt = new TextBox();
            inputPrompt.Height = (1/4) * Height;
            inputPrompt.Width = Width;
            inputPrompt.Text = "Enter location of Vending Machine...";
            inputPrompt.UpdateLayout();
            inputPrompt.Opacity = 100;
            inputPrompt.Visibility = Visibility.Visible;
            VendFinderApp.Children.Add(inputPrompt);
        }

        public void programLoop()
        {
            while (true)
            {
                

                Debug.WriteLine("Hello");
                //UInt32 pointerLoc = 0;
                //PointerPoint.GetCurrentPoint(pointerLoc);
            }
        }
    }
}
