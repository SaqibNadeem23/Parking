using Parking.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyParkings : ContentPage
    {
        string SId, SName, SEmail, SPhone, SPass;
        public MyParkings(string Id, string Name, string Email, string Phone, string Pass)
        {
            InitializeComponent();
            SId = Id;
            SName = Name;
            SEmail = Email;
            SPhone = Phone;
            SPass = Pass;


            SQLiteConnection con = new SQLiteConnection(App.Databaselocation);
            con.CreateTable<Parkings>();
            var nms = con.Query<Parkings>("Select * from Parkings where UserId = ?",SId);
            foreach (var x in nms)
            {
                Frame frame = new Frame();


                OrderStack.Children.Add(frame);

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

                //byte[] b = x.imgbyte;
                //Stream ms = new MemoryStream(b);
                Image image = new Image()
                {
                    HeightRequest = 50,
                    WidthRequest = 50,
                    Source = "parkingcar.png",
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
                fs2.Children.Add(flb1);
                mainstack.Children.Add(fs2);

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
                frame.GestureRecognizers.Add(tapGestureRecognizer);

                void TapGestureRecognizer_Tapped(object sender, EventArgs e)
                {
                    Navigation.PushAsync(new AddParking(x.ParkingId.ToString(), SId, SName, SEmail, SPhone, SPass));
                }
            }
        }


    }
}