using Parking.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParkingInfo : ContentPage
    {
        string SName, SEmail, SPhone, SPass, SId;
        public ParkingInfo(string UId, string UName, string UEmail, string UPhone, string UPass)
        {
            InitializeComponent();
            SId = UId;
            SName = UName;
            SEmail = UEmail;
            SPhone = UPhone;
            SPass = UPass;

            // cp.Title = SName;

            e1.Text = SName;
            e2.Text = SEmail;
            e3.Text = SPhone;
            e4.Text = SPass;

            e1.IsEnabled = false;
            e2.IsEnabled = false;
            e3.IsEnabled = false;
            e4.IsEnabled = false;
            btn.IsEnabled = false;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            e1.IsEnabled = true;
            e2.IsEnabled = true;
            e3.IsEnabled = true;
            e4.IsEnabled = true;
            btn.IsEnabled = true;
        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            bool nmC, phC, psC, emC;

            if (e1.Text != null && e1.Text != "" && Regex.IsMatch(e1.Text, "^(([A-za-z]+[ ]{1}[A-za-z]+)|([A-Za-z]+|[A-za-z]+[ ]{1}[A-za-z]+[ ]{1}[A-za-z]+))$"))
            {
                nmC = true;
            }
            else
            {
                nmC = false;
                DisplayAlert("Error", "Name is Empty or Incorrect", "Ok");
            }


            if (e3.Text != null && e3.Text != "" && Regex.IsMatch(e3.Text, @"^-?\d+\.?\d*$"))
            {
                phC = true;
            }
            else
            {
                phC = false;
                DisplayAlert("Error", "Phone Number is Empty or Incorrect", "Ok");
            }

            if (e2.Text != null && e2.Text != "" && Regex.IsMatch(e2.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))

            {
                emC = true;
            }
            else
            {
                emC = false;
                DisplayAlert("Error", "Email is Empty or Incorrect", "Ok");
            }


            if (e4.Text != null && e4.Text != "")
            {
                psC = true;
            }
            else
            {
                psC = false;
                DisplayAlert("Error", "Password is Empty or Incorrect", "Ok");
            }

            if (nmC == true && phC == true && psC == true && emC == true)
            {
                SQLiteConnection con = new SQLiteConnection(App.Databaselocation);
                con.CreateTable<Users>();
                con.Query<Users>("Update Users SET FullName = ?, PhoneNumber = ?, Email = ?, Password = ? Where UserId = ?", e1.Text, e2.Text, e3.Text, e4.Text, SId);
                con.Close();

                DisplayAlert("Successfull", "Updated Successfully", "ok");
                Navigation.PushAsync(new Main(SId, e1.Text, e3.Text, e2.Text, e4.Text));
            }

            //Navigation.PushAsync(new Main("1", "Admin", "123", "admin@admin.com", "admin123"));
        }
    }
}