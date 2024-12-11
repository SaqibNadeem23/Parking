using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;

using SQLite;
using Parking.Model;
using System.Collections.ObjectModel;
using System.IO;

namespace Parking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        string SId, SName, SEmail, SPhone, SPass;
        string GooglePlacesApiKey = "enter your api key";
        Location loc;




        public SearchPage(string v1, string v2, string v3, string v4, string v5, Location location)
        {
            InitializeComponent();
            SId = v1;
            SName = v2;
            SPhone = v3;
            SEmail = v4;
            SPass = v5;
            loc = location;
            // sssrc.ApiKey = "AIzaSyDd6E_Bd7fJBxDfMkCOtgH4wj7mb1VQdPY";
            //search_bar.ApiKey = "AIzaSyDd6E_Bd7fJBxDfMkCOtgH4wj7mb1VQdPY";
            //string ApiKey = Device.OS == TargetPlatform.Android ? "AIzaSyDd6E_Bd7fJBxDfMkCOtgH4wj7mb1VQdPY" : "";
            //search_bar.ApiKey = GooglePlacesApiKey;
            //search_bar.Type = PlaceType.Address;
            //search_bar.Components = new Components("country:au|country:nz"); // Restrict results to Australia and New Zealand
            //search_bar.PlacesRetrieved += Search_Bar_PlacesRetrieved;
            //search_bar.TextChanged += Search_Bar_TextChanged;
            //search_bar.MinimumSearchText = 2;
            //results_list.ItemSelected += Results_List_ItemSelected;



            // AutoCompleteTextView completeTextView = new AutoCompleteTextView();

            SQLiteConnection con = new SQLiteConnection(App.Databaselocation);
            con.CreateTable<Parkings>();
            var nms = con.Query<Parkings>("Select * from Parkings ORDER BY RANDOM() LIMIT 5");
            foreach (var x in nms)
            {
                string plat = x.ParkingLatitude;
                string plong = x.ParkingLongitude;

                Frame frame = new Frame()
                {
                    HeightRequest = 70,
                };
                   

                stk.Children.Add(frame);

                var mainstack = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    Margin = 10,
                };
                frame.Content = mainstack;

                var fs1 = new StackLayout()
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 10, 0),
                   
                };
                mainstack.Children.Add(fs1);

                byte[] b = x.imgbyte;
                Stream ms = new MemoryStream(b);
                Image image = new Image()
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    Source = ImageSource.FromStream(() => ms),
            };
                fs1.Children.Add(image);

                var fs2 = new StackLayout()
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 20, 0),
                };
                Label flb1 = new Label
                {
                    Text = x.ParkingName,
                    FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label)),
                    TextColor = Color.FromHex("#0275d8"),
                };
                Label flb2 = new Label
                {
                    Text = "Distance = " + Math.Round(Location.CalculateDistance(loc.Latitude, loc.Longitude, Convert.ToDouble(plat), Convert.ToDouble(plong), DistanceUnits.Kilometers), 2) + " Km",

                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    TextColor = Color.FromHex("#0275d8"),
                };
                fs2.Children.Add(flb1);
                fs2.Children.Add(flb2);
                mainstack.Children.Add(fs2);

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
                frame.GestureRecognizers.Add(tapGestureRecognizer);

                void TapGestureRecognizer_Tapped(object sender, EventArgs e)
                {
                    string flag = "parking";
                    Navigation.PushAsync(new UserMain(SId, SName, SEmail, SPhone, SPass, x.ParkingId.ToString(), flag));
                }
            }


        }


        protected override void OnAppearing()
        {
            srch.Focus();
        }


        private void srch_SearchButtonPressed(object sender, EventArgs e)
        {
            string address = srch.Text + ", Quetta, Balochistan";
            string flag = "search";

            Navigation.PushAsync(new UserMain(SId, SName, SEmail, SPhone, SPass, address, flag));

            // meramap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));

        }

        
    }
}
