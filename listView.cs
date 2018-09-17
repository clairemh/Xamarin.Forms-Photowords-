using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Wordfinder
{
    public class listView : ContentPage
    {


        public static ObservableCollection<string> Items { get; set; }

        public listView()
        {
            Items = new ObservableCollection<string>() { };

            ListView lstView = new ListView();


            

            lstView.ItemTapped += OnSelection;

            lstView.ItemsSource = Items;

            var temp = new DataTemplate(typeof(textViewCell));
            lstView.ItemTemplate = temp;

            Content = new StackLayout
            {
                Margin = new Thickness(20),
                Children =
                {
                    new Label { FontAttributes = FontAttributes.Bold, TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.Center },
                    lstView
                }
            };
        }




        void OnSelection(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }


            string selected = e.Item.ToString();

        }


        public class textViewCell : ViewCell
        {
            public textViewCell()
            {
                StackLayout layout = new StackLayout();
                layout.Padding = new Thickness(15, 0);
                Label label = new Label();

                label.SetBinding(Label.TextProperty, ".");
                layout.Children.Add(label);

                View = layout;
            }


        }


    }
}



        
