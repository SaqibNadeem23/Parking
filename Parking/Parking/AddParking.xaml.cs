using Parking.Model;
using Plugin.Media;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Parking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddParking : ContentPage
    {
        string mLat = "", mLong = "";
        string SId, SName, SEmail, SPhone, SPass;
        public AddParking(string Id, string Name, string Email, string Phone, string Pass)
        {
            try
            {
                InitializeComponent();
                SId = Id;
                SName = Name;
                SEmail = Email;
                SPhone = Phone;
                SPass = Pass;
            }
            catch (Exception e)
            {

            }
        }

        string psid = "";
        public AddParking(string parkingId, string Id, string Name, string Email, string Phone, string Pass)
        {
            try
            {

            InitializeComponent();
            slotsstack.IsVisible = true;
            btn1.IsVisible = false;
            btn2.IsVisible = true;
            psid = parkingId;
            SId = Id;
            SName = Name;
            SEmail = Email;
            SPhone = Phone;
            SPass = Pass;


                SQLiteConnection con = new SQLiteConnection(App.Databaselocation);
            con.CreateTable<Parkings>();
            var nms = con.Query<Parkings>("Select * from Parkings where ParkingId = '"+ psid + "'");
            foreach (var x in nms)
            {
                Pin pin = new Pin()
                {
                    Label = "Parking Location",
                    Position = new Position(Convert.ToDouble(x.ParkingLatitude), Convert.ToDouble(x.ParkingLongitude)),
                };

                map.Pins.Add(pin);

                mLat = x.ParkingLatitude;
                mLong = x.ParkingLongitude;

                NameEntry.Text = x.ParkingName;
                CarSlotsEntry.Text = x.ParkingCarSlots;
                CarFeesEntry.Text = x.ParkingCarFees;
                BikeSlotsEntry.Text = x.ParkingBikeSlots;
                BikeFeesEntry.Text = x.ParkingBikeFees;
                BikeSlotsFilled.Text = x.ParkingBikeFilled;
                CarSlotsFilled.Text = x.ParkingCarFilled;
            }
            }
            catch (Exception e)
            {

            }
        }

        private void map_MapClicked(object sender, MapClickedEventArgs e)
        {
            map.Pins.Clear();
            Pin pin = new Pin()
            {
                Label = "Parking Location",
                Position = new Position(e.Position.Latitude, e.Position.Longitude),
            };

            map.Pins.Add(pin);

            mLat = e.Position.Latitude.ToString();
            mLong = e.Position.Longitude.ToString();
        }
      

        protected override async void OnAppearing()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(1)));
            }
            catch (Exception e)
            {

            }
        }

        private void btns1_Clicked(object sender, EventArgs e)
        {
            bool a1, a2, a3, a4, a5, a6, a7 = true, a8 = true;
            string error = "Following errors occured:\n";

            if (NameEntry.Text != null && NameEntry.Text != "")
            {
                a1 = true;
            }
            else
            {
                a1 = false;
                error += "Name is Empty or Incorrect\n";
            }

            if (mLat != null && mLat != "")
            {
                a2 = true;
            }
            else
            {
                a2 = false;
                error += "Parking Location is not Selected\n";
            }

            if (CarSlotsEntry.Text != null && CarSlotsEntry.Text != "")
            {
                a3 = true;

                if (CarSlotsFilled.Text != null && CarSlotsFilled.Text != "")
                {
                    if (Convert.ToInt32(CarSlotsFilled.Text) <= Convert.ToInt32(CarSlotsEntry.Text))
                        a8 = true;

                    else
                    {
                        a8 = false;
                        error += "Car Filled slots is greater than available slots\n";
                    }
                }
                else
                {
                    a8 = false;
                    error += "Car Filled slots is Empty or Incorrect\n";
                }
            }
            else
            {
                a3 = false;
                error += "Car Slots is Empty or Incorrect\n";
            }

            if (CarFeesEntry.Text != null && CarFeesEntry.Text != "")
            {
                a4 = true;
            }
            else
            {
                a4 = false;
                error += "Car Fees is Empty or Incorrect\n";
            }

            if (BikeSlotsEntry.Text != null && BikeSlotsEntry.Text != "")
            {
                a5 = true;

                if (BikeSlotsFilled.Text != null && BikeSlotsFilled.Text != "")
                {
                    if( Convert.ToInt32(BikeSlotsFilled.Text)  <= Convert.ToInt32(BikeSlotsEntry.Text) )
                    a7 = true;

                    else
                    {
                        a7 = false;
                        error += "Bike Filled slots is greater than available slots\n";
                    }
                }
                else
                {
                    a7 = false;
                    error += "Bike Filled slots is Empty or Incorrect\n";
                }
            }
            else
            {
                a5 = false;
                error += "Bike Slots is Empty or Incorrect\n";
            }

            if (BikeFeesEntry.Text != null && BikeFeesEntry.Text != "")
            {
                a6 = true;

                
            }
            else
            {
                a6 = false;
                error += "Bike Fees is Empty or Incorrect\n";
            }

            if (a1 == true && a2 == true && a3 == true && a4 == true && a5 == true && a6 == true && a7 == true && a8 == true)
            {
                SQLiteConnection con = new SQLiteConnection(App.Databaselocation);
                con.CreateTable<Parkings>();
                con.Query<Parkings>("Update Parkings set ParkingName = ?,ParkingLatitude = ?,ParkingLongitude = ?,ParkingCarSlots = ?,ParkingCarFees = ?,ParkingBikeSlots = ?,ParkingBikeFilled = ?,ParkingCarFilled = ?,ParkingBikeFees = ? where ParkingId= '"+ psid + "'", NameEntry.Text, mLat, mLong, CarSlotsEntry.Text, CarFeesEntry.Text, BikeSlotsEntry.Text, BikeSlotsFilled.Text, CarSlotsFilled.Text, BikeFeesEntry.Text);
                

                DisplayAlert("Successfull", "Parking Edited Successfully", "Ok");

                Navigation.PushAsync(new Main(SId, SName, SEmail, SPhone, SPass));
            }

            else
            {
                DisplayAlert("Error", error, "Ok");
            }

        }

        byte[] imagearray1;
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "Camera is not supported", "ok");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();


            if (file == null)
                return;



            img2.Source = ImageSource.FromStream(() =>
            {
                var Str = file.GetStream();

                return Str;
            });

            using (MemoryStream memory = new MemoryStream())
            {

                Stream stream = file.GetStream();
                stream.CopyTo(memory);
                imagearray1 = memory.ToArray();
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            bool a1, a2, a3, a4,a5,a6,a7;
            string error = "Following errors occured:\n";

            if (NameEntry.Text != null && NameEntry.Text != "")
            {
                a1 = true;
            }
            else
            {
                a1 = false;
                error += "Name is Empty or Incorrect\n";
            }

            if (mLat != null && mLat != "")
            {
                a2 = true;
            }
            else
            {
                a2 = false;
                error += "Parking Location is not Selected\n";
            }

            if (CarSlotsEntry.Text != null && CarSlotsEntry.Text != "")
            {
                a3 = true;
            }
            else
            {
                a3 = false;
                error += "Car Slots is Empty or Incorrect\n";
            }

            if (CarFeesEntry.Text != null && CarFeesEntry.Text != "")
            {
                a4 = true;
            }
            else
            {
                a4 = false;
                error += "Car Fees is Empty or Incorrect\n";
            }

            if (BikeSlotsEntry.Text != null && BikeSlotsEntry.Text != "")
            {
                a5 = true;
            }
            else
            {
                a5 = false;
                error += "Bike Slots is Empty or Incorrect\n";
            }

            if (BikeFeesEntry.Text != null && BikeFeesEntry.Text != "")
            {
                a6 = true;
            }
            else
            {
                a6 = false;
                error += "Bike Fees is Empty or Incorrect\n";
            }

            if (imagearray1 != null)
            {
                a7 = true;
            }

            else
            {
                a7 = false;
                error += "Image is not Selected\n";
            }


            if (a1 == true && a2 == true && a3 == true && a4 == true && a5 == true && a6 == true && a7 == true)
            {
                Parkings parking = new Parkings()
                {
                    ParkingName = NameEntry.Text.ToString(),
                    ParkingLatitude = mLat,
                    ParkingLongitude = mLong,
                    ParkingBikeSlots = BikeSlotsEntry.Text.ToString(),
                    ParkingBikeFees = BikeFeesEntry.Text.ToString(),
                    ParkingCarFees = BikeFeesEntry.Text.ToString(),
                    ParkingCarSlots = BikeSlotsEntry.Text.ToString(),
                    ParkingBikeFilled = "0",
                    ParkingCarFilled = "0",
                    imgbyte = imagearray1,
                    UserId = SId,
                };
                SQLiteConnection conn = new SQLiteConnection(App.Databaselocation);
                conn.CreateTable<Parkings>();
                conn.Insert(parking);
                conn.Close();

                DisplayAlert("Successfull", "Parking created Successfully", "Ok");

                Navigation.PushAsync(new Main(SId, SName, SEmail, SPhone, SPass));
            }

            else
            {
                DisplayAlert("Error", error, "Ok");
            }
        }
    }
}