using Parking.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUp());
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
             Navigation.PushAsync(new ParkingSignup());
        }

        private void Loginb_Clicked(object sender, EventArgs e)
        {
            SQLiteConnection con = new SQLiteConnection(App.Databaselocation);
            con.CreateTable<Users>();
            var nms = con.Query<Users>("Select UserId from Users where UserName = ? and Password = ?", name.Text, ps1.Text);



            string[] arr = new string[1];
            foreach (var s in nms)
            {
                arr[0] = s.UserId.ToString();
            }

            string idd = "";
            idd = arr[0];
            var data = con.Query<Users>("Select * from Users where UserId = ?", idd);

            string[] dat = new string[6];
            foreach (var s in data)
            {
                dat[0] = s.UserId.ToString();
                dat[1] = s.FullName;
                dat[2] = s.PhoneNumber;
                dat[3] = s.Email;
                dat[4] = s.Password;
                dat[5] = s.UserType;
            }

            con.Close();

            int x = nms.Count;


            if (name.Text == "admin" && ps1.Text == "admin")
            {
                //Navigation.PushAsync(new AdminView());
            }

            else if (x > 0 && dat[5] == "Customer")
            {
                Navigation.PushAsync(new UserMain(dat[0], dat[1], dat[2], dat[3], dat[4]));
            }
            else if (x > 0 && dat[5] == "Parking")
            {
                Navigation.PushAsync(new Main(dat[0], dat[1], dat[2], dat[3], dat[4]));
            }
            else
            {
                DisplayAlert("Error", "Username or Password is incorrect", "ok");

            }


        }
    }
}