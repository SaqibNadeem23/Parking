using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Pages;
using SQLite;
using Parking.Model;
using System.IO;
using Xamarin.Essentials;

namespace Parking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopPage1 : PopupPage
    {
        string Pd = "",Sd, Psd = "", plong = "", plat="",status = "",vehicle = "";
        public PopPage1(string PId, string SId)
        {
            InitializeComponent();
            Pd = PId;
            Sd = SId;

            

            SQLiteConnection con = new SQLiteConnection(App.Databaselocation);
            con.CreateTable<Parkings>();
            var nms = con.Query<Parkings>("Select * from Parkings where ParkingId = '" + Pd + "'");
            foreach (var s in nms)
            {
                TitleLabel.Text = s.ParkingName;
                BikeSpotsFilled.Text = s.ParkingBikeFilled;
                BikeTotalSpots.Text = s.ParkingBikeSlots;
                BikeParkingCost.Text = s.ParkingBikeFees;
                CarSpotsFilled.Text = s.ParkingCarFilled;
                CarTotalSpots.Text = s.ParkingCarSlots;
                CarParkingCost.Text = s.ParkingCarFees;
                byte[] b = s.imgbyte;
                Stream ms = new MemoryStream(b);
                img.Source = ImageSource.FromStream(() => ms);
                plat = s.ParkingLatitude;
                plong = s.ParkingLongitude;
                Psd = s.UserId;
            }
            con.Close();

            if (Psd == Sd)
            {
                BookingButton.IsVisible = false;
            }


            SQLiteConnection con1 = new SQLiteConnection(App.Databaselocation);
            con1.CreateTable<Users>();
            var nms1 = con1.Query<Users>("Select StatusId from Users where UserId = '" + Sd + "'");
            foreach (var s in nms1)
            {
                status = s.StatusId;
            }

            if(status == Pd)
            {
                BookingButton.Text = "Finish";
            }
            else if(status == "None")
            {

            }
            else
            {
                BookingButton.IsEnabled = false;
            }
            con1.Close();
        }
        protected override async void OnAppearing()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            DistanceLabel.Text ="Distance = " + Math.Round(Location.CalculateDistance(location.Latitude, location.Longitude, Convert.ToDouble(plat), Convert.ToDouble(plong), DistanceUnits.Kilometers),2) + " Km";
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (status == Pd)
            {
                SQLiteConnection con = new SQLiteConnection(App.Databaselocation);
                con.CreateTable<Users>();
                var nms = con.Query<Users>("Select * from Users where UserId = '" + Sd + "'");
                foreach (var s in nms)
                {
                    status = s.StatusId;
                    vehicle = s.VehicleSelected;
                }
                con.Close();
                if(vehicle == "Car")
                {
                    SQLiteConnection con1 = new SQLiteConnection(App.Databaselocation);
                    con1.CreateTable<Parkings>();
                    var nms1 = con1.Query<Parkings>("Select ParkingCarFilled from Parkings where ParkingId = '" + Pd + "'");
                    foreach (var s in nms1)
                    {
                        int value = Convert.ToInt32(s.ParkingCarFilled);
                        value--;
                        SQLiteConnection con2 = new SQLiteConnection(App.Databaselocation);
                        con2.CreateTable<Parkings>();
                        con2.Query<Parkings>("Update Parkings set ParkingCarFilled = '" + value.ToString() + "' where ParkingId = '" + Pd + "'");                       
                        con2.Close();

                        SQLiteConnection con3 = new SQLiteConnection(App.Databaselocation);
                        con3.CreateTable<Users>();
                        con3.Query<Users>("Update Users set StatusId = 'None' where UserId = '" + Sd + "'");
                        con3.Close();

                        PopupNavigation.PopAllAsync();
                        DisplayAlert("Success", "Your Parking has been completed successfully", "Ok");
                    }
                    con1.Close();
                }
                else if (vehicle == "Bike")
                {
                    SQLiteConnection con1 = new SQLiteConnection(App.Databaselocation);
                    con1.CreateTable<Parkings>();
                    var nms1 = con1.Query<Parkings>("Select ParkingBikeFilled from Parkings where ParkingId = '" + Pd + "'");
                    foreach (var s in nms1)
                    {
                        int value = Convert.ToInt32(s.ParkingBikeFilled);
                        value--;
                        SQLiteConnection con2 = new SQLiteConnection(App.Databaselocation);
                        con2.CreateTable<Parkings>();
                        con2.Query<Parkings>("Update Parkings set ParkingBikeFilled = '" + value.ToString() + "' where ParkingId = '" + Pd + "'");
                        con2.Close();

                        SQLiteConnection con3 = new SQLiteConnection(App.Databaselocation);
                        con3.CreateTable<Users>();
                        con3.Query<Users>("Update Users set StatusId = 'None' where UserId = '" + Sd + "'");
                        con3.Close();

                        PopupNavigation.PopAllAsync();
                        DisplayAlert("Success", "Your Parking has been completed successfully", "Ok");
                    }
                    con1.Close();
                }
                BookingButton.Text = "Book";
            }
            else if (status == "None")
            {
                s1.IsVisible = false;
                s2.IsVisible = true;
            }
            else
            {
                BookingButton.IsEnabled = false;
            }
            
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            f1.BorderColor = Color.DarkBlue;
            f2.BorderColor = Color.Default;
            vehicle = "Bike";
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            f1.BorderColor = Color.Default;
            f2.BorderColor = Color.DarkBlue;
            vehicle = "Car";
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            SQLiteConnection con = new SQLiteConnection(App.Databaselocation);
            con.CreateTable<Parkings>();
            if(vehicle == "Car")
            {
                int parkingfilled = 0, parkingtotal = 0;
                var nms = con.Query<Parkings>("Select * from Parkings where ParkingId = '" + Pd + "'");
                foreach (var s in nms)
                {
                    parkingfilled = Convert.ToInt32(s.ParkingCarFilled);
                    parkingtotal = Convert.ToInt32(s.ParkingCarSlots);
                }

                if(parkingfilled == parkingtotal)
                {
                    DisplayAlert("Attention", "Parking Slots of Car is already filled", "Ok");

                }
                else
                {
                    parkingfilled++;
                    SQLiteConnection con2 = new SQLiteConnection(App.Databaselocation);
                    con2.CreateTable<Parkings>();
                    con2.Query<Parkings>("Update Parkings set ParkingCarFilled = '" + parkingfilled.ToString() + "' where ParkingId = '" + Pd + "'");
                    con2.Close();

                    SQLiteConnection con3 = new SQLiteConnection(App.Databaselocation);
                    con3.CreateTable<Users>();
                    con3.Query<Users>("Update Users set StatusId = '" + Pd + "',VehicleSelected = 'Car' where UserId = '" + Sd + "'");
                    con3.Close();

                    PopupNavigation.PopAllAsync();
                    DisplayAlert("Success", "Your vehicle is Booked Successfully!", "Ok");
                }
            }
            else if (vehicle == "Bike")
            {
                int parkingfilled = 0, parkingtotal = 0;
                var nms = con.Query<Parkings>("Select * from Parkings where ParkingId = '" + Pd + "'");
                foreach (var s in nms)
                {
                    parkingfilled = Convert.ToInt32(s.ParkingBikeFilled);
                    parkingtotal = Convert.ToInt32(s.ParkingBikeSlots);
                }

                if (parkingfilled == parkingtotal)
                {
                    DisplayAlert("Attention", "Parking Slots of Bike is already filled", "Ok");

                }
                else
                {
                    parkingfilled++;
                    SQLiteConnection con2 = new SQLiteConnection(App.Databaselocation);
                    con2.CreateTable<Parkings>();
                    con2.Query<Parkings>("Update Parkings set ParkingBikeFilled = '" + parkingfilled.ToString() + "' where ParkingId = '" + Pd + "'");
                    con2.Close();

                    SQLiteConnection con3 = new SQLiteConnection(App.Databaselocation);
                    con3.CreateTable<Users>();
                    con3.Query<Users>("Update Users set StatusId = '" + Pd + "',VehicleSelected = 'Bike' where UserId = '" + Sd + "'");
                    con3.Close();

                    PopupNavigation.PopAllAsync();
                    DisplayAlert("Success", "Your vehicle is Booked Successfully!", "Ok");
                }
            }
            con.Close();

            
        }
    }
}