using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using Xamarin.Forms.Maps;
using System.Diagnostics;
using Parking.Model;
using Rg.Plugins.Popup.Services;
using SQLite;
using Xamarin.Essentials;

namespace Parking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserMain : FlyoutPage
    {
        string SId, SName, SEmail, SPhone, SPass;
        string searchstring = "";
        string flg = "";
       

        public UserMain(string v1, string v2, string v3, string v4, string v5)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                SId = v1;
                SName = v2;
                SPhone = v3;
                SEmail = v4;
                SPass = v5;
                flg = "";
                searchstring = "";
                lb2.Text = SName;

                srch.Placeholder = "Search Area";
            }
            catch (Exception e)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                DisplayAlert("Try Catch error:", e.ToString() + "\n Line Number: " + line.ToString(), "Ok");

            }

        }

        public UserMain(string v1, string v2, string v3, string v4, string v5, string text, string flag)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);

                SId = v1;
                SName = v2;
                SPhone = v3;
                SEmail = v4;
                SPass = v5;
                lb2.Text = SName;

                flg = flag;

                if (flag == "search")
                {
                    searchstring = text;
                    srch.Text = text;
                }
                else
                {
                    searchstring = text;
                }
            }
            catch (Exception e)
            {
                // Get stack trace for the exception with source file information
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                DisplayAlert("Try Catch error:", e.ToString() + "\n Line Number: " + line.ToString(), "Ok");

            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        private async void srch_SearchButtonPressed(object sender, EventArgs e)
        {
            //var address = srch.Text + "Quetta, Balochistan";
            Location location = await Geolocation.GetLastKnownLocationAsync();


            //var location = locations?.FirstOrDefault();
            //if (location != null)
            //{

            //    meramap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));
            //}
            srch.Unfocus();
            Navigation.PushAsync(new SearchPage(SId, SName, SEmail, SPhone, SPass, location));



        }

        protected override async void OnAppearing()
        {
            try
            {
                if (searchstring == "")
                {
                    var location = await Geolocation.GetLastKnownLocationAsync();

                    meramap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));
                    var placemarks = await Geocoding.GetPlacemarksAsync(location);
                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        var geocodeAddress = placemark.AdminArea + ", " + placemark.Locality;

                    }
                }
                else
                {
                    if (flg == "search")
                    {
                        var address = searchstring + " Quetta, Balochistan";
                        var locations = await Geocoding.GetLocationsAsync(address);


                        var location = locations?.FirstOrDefault();
                        if (location != null)
                        {

                            meramap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));

                        }
                    }
                    else
                    {
                        SQLiteConnection con1 = new SQLiteConnection(App.Databaselocation);
                        con1.CreateTable<Parkings>();
                        var nms1 = con1.Query<Parkings>("Select * from Parkings where ParkingId = '" + searchstring + "'");
                        foreach (var x in nms1)
                        {
                            meramap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Convert.ToDouble(x.ParkingLatitude), Convert.ToDouble(x.ParkingLongitude)), Distance.FromMiles(1)));
                        }
                        con1.Close();
                    }
                }

                SQLiteConnection con = new SQLiteConnection(App.Databaselocation);
                con.CreateTable<Parkings>();
                var nms = con.Query<Parkings>("Select * from Parkings");

                if (nms.Count > 0)
                {
                    foreach (var s in nms)
                    {
                        Location loc = new Location
                        {
                            Latitude = Convert.ToDouble(s.ParkingLatitude),
                            Longitude = Convert.ToDouble(s.ParkingLongitude),
                        };


                        CPin pin = new CPin
                        {

                            Position = new Position(loc.Latitude, loc.Longitude),
                            Label = s.ParkingName,

                        };
                        meramap.CPins = new List<CPin> { pin };
                        meramap.Pins.Add(pin);






                        pin.MarkerClicked += Pin_Clicked;

                        meramap.Pins.Add(pin);




                        void Pin_Clicked(object sender, EventArgs e)
                        {
                            PopupNavigation.Instance.PushAsync(new PopPage1(s.ParkingId.ToString(),SId));
                        }
                    }
                }
                con.Close();
            }
            catch (Exception e)
            {

            }

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserInfo(SId, SName, SEmail, SPhone, SPass));
        }
        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Login());

        }

        private async void meramap_MapClicked(object sender, MapClickedEventArgs e)
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(e.Position.Latitude, e.Position.Longitude);
            var placemark = placemarks?.FirstOrDefault();
            if (placemark != null)
            {
                var geocodeAddress = placemark.AdminArea + ", " + placemark.Locality;
                srch1.Text = geocodeAddress;
            }
        }

    }
}