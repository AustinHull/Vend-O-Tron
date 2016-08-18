using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
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

        Windows.Storage.StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

        public MainPage()
        {
            this.InitializeComponent();
          
            Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);
            //storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            loadData();

            // Checks for click/tap input - used to place pins on the map.
            this.VendFinderApp.PointerPressedOverride += VendFinderApp_PointerPressedOverride;

            // Checks for Enter key input after recent pin placement - used to save
            // pin's vending machine data to an SQLite database.
            this.VendFinderApp.KeyDown += Enter_KeyDown;
        }

        void Download_File()
        {
            var downloader = new BackgroundDownloader();


        }

        void Enter_KeyDown(object semder, KeyRoutedEventArgs e)
        {
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

                    dataBase.Machines.Add(machine);

                    dataBase.SaveChanges();

                    Debug.WriteLine(machine.Id + ":" + machine.machineName + ":" + machine.machineLocation);
                }

                // Now that the user has confirmed their data entry, disable the edit
                // mode of the textbox, and reset the pinBeingPlaced variable to false.
                inputPrompt.IsReadOnly = true;
                pinBeingPlaced = false;
            }
        }

        void VendFinderApp_PointerPressedOverride(object sender, PointerRoutedEventArgs e)
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
            inputPrompt.Opacity = 1;
            inputPrompt.Visibility = Visibility.Visible;
            VendFinderApp.Children.Add(inputPrompt);

            // Generic file name for file which stores user-prescribed location info.
            //string name = "database.txt";

            // Package the info file and save it to the user's system.
            //StorageFile storeFile =
              //  await storageFolder.CreateFileAsync(name, CreationCollisionOption.GenerateUniqueName);

            mLoc = loc.Latitude.ToString() + "|" + loc.Longitude.ToString();

            //storeFile = await storageFolder.GetFileAsync("dataBase.txt");
            //await FileIO.WriteTextAsync(storeFile, loc.Latitude.ToString() + " " + loc.Longitude.ToString());

            pinBeingPlaced = true;
        }

        // This is the task which runs to load database information to the app.
        public void loadData()
        {
            List<string> machineNames = new List<string>();
            List<double> machineCoordLat = new List<double>();
            List<double> machineCoordLong = new List<double>();

            using (var queryContext = new VendingInfoContext())
            {
                try
                {
                    // Finish implementing database query system!
                    var machineEntities = queryContext.Machines.ToList();

                    // Organizes pin data from SQLite database into compartmentalized variables
                    // for use within the application.
                    for (int i = 0; i < machineEntities.Count(); i++)
                    {
                        machineNames.Add(machineEntities.ElementAt(i).machineName);

                        double tempLat = double.Parse(machineEntities.ElementAt(i).machineLocation.Substring(0, 16));
                        double tempLong = double.Parse(machineEntities.ElementAt(i).machineLocation.Substring(17));
                        machineCoordLat.Add(tempLat);
                        machineCoordLong.Add(tempLong);
                    }

                    // Not sure if this actually helps with anything...
                    Debug.WriteLine(machineEntities[0].Id);
                }
                catch(Exception e)
                {
                    // Thrown if an exception occurs.
                    Debug.WriteLine("No elements in database at this time...: " + e);
                }
            }

            // Places all stored location pins onto the application's map.
            for (int i = 0; i < machineNames.Count(); i++)
            {
                Bing.Maps.Location loc = new Bing.Maps.Location(machineCoordLat.ElementAt(i), machineCoordLong.ElementAt(i));

                Bing.Maps.Pushpin pushPin = new Bing.Maps.Pushpin();
                pushPin.SetValue(Bing.Maps.MapLayer.PositionProperty, loc);
                this.VendFinderApp.Children.Add(pushPin);

                // Add textbox containing location name to placed pin location...
                TextBox locText = new TextBox();
                locText.Height = (1 / 4) * Height;
                locText.Width = Width;
                locText.SetValue(Bing.Maps.MapLayer.PositionProperty, loc);
                locText.Text = machineNames.ElementAt(i);
                locText.IsEnabled = true;
                locText.UpdateLayout();
                locText.Opacity = 1;
                locText.Visibility = Visibility.Visible;
                locText.IsReadOnly = true;
                VendFinderApp.Children.Add(locText);
            }

            //double lat = double.Parse(universalText.Substring(0, 16));
            //double longi = double.Parse(universalText.Substring(17));
            //Bing.Maps.Location loc = new Bing.Maps.Location(lat, longi);

            //Bing.Maps.Pushpin pushPin = new Bing.Maps.Pushpin();
            //pushPin.SetValue(Bing.Maps.MapLayer.PositionProperty, loc);
            //this.VendFinderApp.Children.Add(pushPin);
        }
    }
}
