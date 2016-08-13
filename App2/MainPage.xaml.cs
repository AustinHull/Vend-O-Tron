﻿using System;
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
using Windows.System;
using System.Diagnostics;
using System.Data.Common;
using System.Windows.Input;
using Windows.Storage;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        // Used to tell if a pin has been placed, but has not yet been submitted.
        bool pinBeingPlaced;
        string universalText;
        string mLoc; // This data will be set when the pin is first placed.

        TextBox inputPrompt; // While TextBox is active, have access to present data.

        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        public MainPage()
        {
            this.InitializeComponent();
            Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
            //storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            Task tempFile;

            tempFile = loadFile("dataBase.txt");

            // Checks for click/tap input - used to place pins on the map.
            this.VendFinderApp.PointerPressedOverride += VendFinderApp_PointerPressedOverride;

            // Checks for Enter key input after recent pin placement - used to save
            // pin's vending machine data to an SQLite database.
            this.VendFinderApp.KeyDown += Enter_KeyDown;

            //programLoop();
        }

        void Enter_KeyDown(object semder, KeyRoutedEventArgs e)
        {
            // Fix key recognition!
            if (e.Key == VirtualKey.Enter && pinBeingPlaced)
            {
                // Try putting some SQLite stuff here...
                using (var dataBase = new VendingInfoContext())
                {
                    var machine = new Machine
                    {
                        machineName = inputPrompt.Text,
                        machineLocation = mLoc
                    };

                    dataBase.Machine.Add(machine);

                    dataBase.SaveChanges();

                    Debug.WriteLine(machine.machineID + ":" + machine.machineName + ":" + machine.machineLocation);
                }

                pinBeingPlaced = false;
            }
        }

        async void VendFinderApp_PointerPressedOverride(object sender, PointerRoutedEventArgs e)
        {
            Bing.Maps.Location loc = new Bing.Maps.Location();
            this.VendFinderApp.TryPixelToLocation(e.GetCurrentPoint(this.VendFinderApp).Position, out loc);
            Bing.Maps.Pushpin pushPin = new Bing.Maps.Pushpin();
            pushPin.SetValue(Bing.Maps.MapLayer.PositionProperty, loc);
            this.VendFinderApp.Children.Add(pushPin);



            // Add code to prompt user to enter VendMachine metadata...
            inputPrompt = new TextBox();
            inputPrompt.Height = (1 / 4) * Height;
            inputPrompt.Width = Width;
            inputPrompt.SetValue(Bing.Maps.MapLayer.PositionProperty, loc);
            inputPrompt.PlaceholderText = "Enter location of Vending Machine...";
            inputPrompt.IsEnabled = true;
            inputPrompt.UpdateLayout();
            inputPrompt.Opacity = 100;
            inputPrompt.Visibility = Visibility.Visible;
            VendFinderApp.Children.Add(inputPrompt);

            // Generic file name for file which stores user-prescribed location info.
            string name = "database.txt";

            // Package the info file and save it to the user's system.
            Windows.Storage.StorageFile storeFile =
                await storageFolder.CreateFileAsync(name, CreationCollisionOption.GenerateUniqueName);

            mLoc = loc.Latitude.ToString() + "|" + loc.Longitude.ToString();

            storeFile = await storageFolder.GetFileAsync("dataBase.txt");
            await Windows.Storage.FileIO.WriteTextAsync(storeFile, loc.Latitude.ToString() + " " + loc.Longitude.ToString());

            pinBeingPlaced = true;
        }

        public void programLoop()
        {
            while (true)
            {
                Debug.WriteLine("Hello");
            }
        }

        // This is the task which runs to save a specified file to the user system.
        public async Task<StorageFile> loadFile(string fileName)
        {
            Windows.Storage.StorageFile loadFile = await storageFolder.GetFileAsync(fileName);

            universalText = await Windows.Storage.FileIO.ReadTextAsync(loadFile);

            double lat = double.Parse(universalText.Substring(0, 16));
            double longi = double.Parse(universalText.Substring(17));
            Bing.Maps.Location loc = new Bing.Maps.Location(lat, longi);

            Bing.Maps.Pushpin pushPin = new Bing.Maps.Pushpin();
            pushPin.SetValue(Bing.Maps.MapLayer.PositionProperty, loc);
            this.VendFinderApp.Children.Add(pushPin);

            return loadFile;
        }
    }
}
