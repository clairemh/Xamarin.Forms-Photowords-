
using Microsoft.ProjectOxford.Vision.Contract;
using Microsoft.ProjectOxford.Vision;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using Plugin.Media;
using Plugin.Connectivity;
using Xamarin.Essentials;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using Color = Xamarin.Forms.Color;


namespace Wordfinder
{
    // [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class HomePage : ContentPage
    {

        public static ObservableCollection<string> Items { get; set; }
        

        public string[] tagcount = new string[10];
        public string selected;

        public Stack<string> liststack = new Stack<string>();

        public Stack<string> selectedstack = new Stack<string>();


        public Stack<string> categorystack = new Stack<string>();

        public Stack<int> countstack = new Stack<int>();

        public string Caption;

        public string FirstTag;
        
     

        public HomePage()
        {

            Items = new ObservableCollection<string>() { };



            InitializeComponent();



            //App/ NavigationBar title
            Title = "Photos To Words";

            // button click declarations


            // for orientation change
            SizeChanged += OnSizeChanged; 

            HomeButton.Clicked += HomeButton_Clicked;

            BottomBanner.Clicked += BottomBanner_Clicked;

            CameraButton.Clicked += CameraButton_Clicked;

            AlbumButton.Clicked += AlbumButton_Clicked;

            Greater.Clicked += Greater_Clicked;

            Less.Clicked += Less_Clicked;

            More.Clicked += More_Clicked;

            Undo.Clicked += Undo_Clicked;


            // adding tap/click recognition to labels

            var DescribeGestureRecognizer = new TapGestureRecognizer();

            Describe.GestureRecognizers.Add(DescribeGestureRecognizer);

            DescribeGestureRecognizer.Tapped += DescribeGestureRecognizer_Tapped;



            var ThingsGestureRecognizer = new TapGestureRecognizer();

            Things.GestureRecognizers.Add(ThingsGestureRecognizer);

            ThingsGestureRecognizer.Tapped += ThingsGestureRecognizer_Tapped;



            var ActionsGestureRecognizer = new TapGestureRecognizer();

            Actions.GestureRecognizers.Add(ActionsGestureRecognizer);

            ActionsGestureRecognizer.Tapped += ActionsGestureRecognizer_Tapped;
        }

        void OnSizeChanged(object sender, EventArgs e)
        {

            // if device is in landscape mode
            if (Width > Height)
            {

                MainStack.Orientation = StackOrientation.Horizontal;
         OuterStack1.Orientation = StackOrientation.Horizontal;
                ButtonLabelStack.Orientation = StackOrientation.Vertical;

            }


            if (Height >  Width)
            {

                MainStack.Orientation = StackOrientation.Vertical;
              OuterStack1.Orientation = StackOrientation.Vertical;
                ButtonLabelStack.Orientation = StackOrientation.Horizontal;

            }

        }

        // functions

        void HomeScreenReturn()
        {

            // making buttons and images null/invisible to return "home"

            Result.Text = null;
            HomeButton.IsVisible = false;
            ImgPhoto.IsVisible = false;
            listView.IsVisible = false;
            
            More.IsVisible = false;

            Greater.IsVisible = false;
            Less.IsVisible = false;

            Actions.IsVisible = false;
            Things.IsVisible = false;
            Describe.IsVisible = false;
           Undo.IsVisible = false;



        }


        private void HomeButton_Clicked(object sender, EventArgs e)
        {
            HomeScreenReturn();

           

          //  Result.WidthRequest = 200;
        }


        private async void BottomBanner_Clicked(object sender, EventArgs e)
        {
            // navigate to settings page
            await Navigation.PushAsync(new SettingsPage());
        }




        public async Task<AnalysisResult> GetImageDescription(Stream imageStream)
        {
            //Microsoft Azure Cognitive Services API key and features
            VisionServiceClient visionClient = new VisionServiceClient("d83d8958fad6455dbff7672618289fc3", "https://westeurope.api.cognitive.microsoft.com/vision/v1.0");
            VisualFeature[] features = {
                          VisualFeature.Tags,
                          VisualFeature.Categories,
                          VisualFeature.Description,
                          VisualFeature.Color,
                          VisualFeature.Faces,

                      };
            return await visionClient.AnalyzeImageAsync(imageStream, features.ToList(), null);
        }



        private async void CameraButton_Clicked(object sender, EventArgs e)
        {

            HomeScreenReturn();


            Result.Text = "Wait...";



            //initalizing CrossMedia plugin method to take photo
            await CrossMedia.Current.Initialize();
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Directory = "Sample",
                    Name = "xamarin.jpg"
                });
                if (file == null) return;
                ImgPhoto.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });

                //displays and speaks alert if no internet connection
                if (!CrossConnectivity.Current.IsConnected)
                {

                    await TextToSpeech.SpeakAsync("Connect Internet.");
                    await DisplayAlert("Error",
                      "Please check your network connection and retry.", "OK");

                }


                var result = await GetImageDescription(file.GetStream());
                


                ImgPhoto.IsVisible = true;

                int i = 0;

                // adding tags (related words) generated from image to array
                foreach (string tag in result.Description.Tags)

                {
                    tagcount[i] = tag;


                    i++;


                    if (i == 9)
                        break;

                }



                // top tag assigned to label
                Result.Text = "    " + tagcount[0];

                // home and more buttons visible after image and description generated

                HomeButton.IsVisible = true;
                More.IsVisible = true;

                //greater button visible so this can be clicked after image descriptive word generated
                Greater.IsVisible = true;

                 Caption = result.Description.Captions.First().Text;

                FirstTag = "    " + tagcount[0];


                // showing caption if greater button is clicked and top tag if less button is clicked
                Greater.Clicked += delegate
                {
                    Result.Text = Caption;
                };

                Less.Clicked += delegate
                {
                    Result.Text = FirstTag;
                };


                file.Dispose();

            }

            //in case of camera error
            catch (Exception)
            {

                await TextToSpeech.SpeakAsync("Camera Error.");
                await DisplayAlert("Error", "Camera Not Available", "OK");

            }

        }





        private async void AlbumButton_Clicked(object sender, EventArgs e)
        {
            HomeScreenReturn();


            Result.Text = "Wait...";



            //MediaPlugin method to pick photo

            await CrossMedia.Current.Initialize();
            try
            {
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null) return;

                ImgPhoto.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });

                //connectivity plugin checks internet connection
                //if not connected
                if (!CrossConnectivity.Current.IsConnected)

                {

                    await TextToSpeech.SpeakAsync("Connect Internet");
                    await DisplayAlert("Error",
                      "Please check your Internet connection.", "OK");


                }

                var result = await GetImageDescription(file.GetStream());

                ImgPhoto.IsVisible = true;


                int i = 0;

                // adding tags (related words) generated from image to array
                foreach (string tag in result.Description.Tags)

                {
                    tagcount[i] = tag;


                    i++;




                    if (i == 9)
                        break;
                }

                Result.Text = "     " + tagcount[0];



                // home and more buttons visible after image and description generated

                HomeButton.IsVisible = true;
                More.IsVisible = true;

                //greater button visible so this can be clicked after image descriptive word generated
                Greater.IsVisible = true;

                 Caption = result.Description.Captions.First().Text;

                FirstTag = "     " + tagcount[0];

                Greater.Clicked += delegate
                {
                    Result.Text = Caption;
                };

                Less.Clicked += delegate
                {
                    Result.Text = FirstTag;
                };






                file.Dispose();
            }


            catch (Exception)
            {

                await TextToSpeech.SpeakAsync("System Error");

                await DisplayAlert("Error", "System Error", "OK");

            }

        }




        private void Less_Clicked(object sender, EventArgs e)
        {

            Greater.IsVisible = true;
            Less.IsVisible = false;

        }





        private void Greater_Clicked(object sender, EventArgs e)
        {

            if (Describe.IsVisible == true)
            {
                Describe.IsVisible = false;
                Actions.IsVisible = false;
                Things.IsVisible = false;

            }


            Greater.IsVisible = false;
            Less.IsVisible = true;

        }




        private void More_Clicked(object sender, EventArgs e)
        { 
            

            Items.Clear();

            selectedstack.Clear();

            countstack.Clear();

            categorystack.Clear();

            liststack.Clear();

            int k = 0;
            for ( k= 0; k < 9; k++) // k < Settings.Count
            {
                Items.Add(tagcount[k]);

                liststack.Push(tagcount[k]);
            }
            
            countstack.Push(k);

            listView.IsVisible = true;

            More.IsVisible = false;

            Undo.IsVisible = true;

        }



       void Undo_Clicked(object sender, EventArgs e)
        {


            int count = 0;

            //clear current list
            Items.Clear();


            // pop last string from stack
            if (selectedstack.Count != 0)
                selectedstack.Pop();


            if (countstack.Peek() != 0)
            // pop current items from stack
            { for (int i = 0; i < countstack.Peek(); i++)
                    liststack.Pop();
            }


            //if nothing left in stack
            if (liststack.Count == 0)
            {
                More.IsVisible = true;   

                Describe.IsVisible = false;
                Actions.IsVisible = false;
                Things.IsVisible = false;
                Undo.IsVisible = false;


            }

            // pop current list item count
            countstack.Pop();

            // if only 1 count in stack go back to first (original) list so hide describe buttons etc
            if (countstack.Count == 1)
            {
                

                Describe.IsVisible = false;
                Actions.IsVisible = false;
                Things.IsVisible = false;

                foreach (string listitem in liststack)
                {
                    Items.Insert(0, listitem);
                    count++;

                    if (count == countstack.Peek())
                        break;
                }

            }




            if (countstack.Count > 1)
            {
                // pop current category from stack
                categorystack.Pop();

                // update labels to previous
                Describe.Text = "describe  " + selectedstack.Peek();

                Actions.Text = selectedstack.Peek() + " actions";

                Things.Text = selectedstack.Peek() + " things";


                if (categorystack.Peek() == "describe")

                {
                    Describe.TextColor = Color.White;
                    Describe.BackgroundColor = Color.Purple;

                    Things.TextColor = Color.Black;
                    Things.BackgroundColor = Color.White;

                    Actions.TextColor = Color.Black;
                    Actions.BackgroundColor = Color.White;

                }


                if (categorystack.Peek() == "things")
                {

                    Describe.TextColor = Color.Black;
                    Describe.BackgroundColor = Color.White;

                    Things.TextColor = Color.White;
                    Things.BackgroundColor = Color.Purple;


                    Actions.TextColor = Color.Black;
                    Actions.BackgroundColor = Color.White;


                }

                if (categorystack.Peek() == "actions")
                {

                    Describe.TextColor = Color.Black;
                    Describe.BackgroundColor = Color.White;

                    Things.TextColor = Color.Black;
                    Things.BackgroundColor = Color.White;


                    Actions.TextColor = Color.White;
                    Actions.BackgroundColor = Color.Purple;


                }

                

                //add previous items back to list
                
                    foreach (string listitem in liststack)
                    {
                        Items.Insert(0, listitem);
                        count++;

                        //add  correct item count back to list by checking top of count stack
                        if (count == countstack.Peek())
                            break;
                    }


                if (countstack.Peek() == 0)
                    listView.IsVisible = false;

                else
                    listView.IsVisible = true;
            }

            

                
            

        }



        void OnSelection(object sender, ItemTappedEventArgs e)
        {
            Result.Text = FirstTag;
            Less.IsVisible = false;
            Greater.IsVisible = true;
            
            Describe.BackgroundColor = Color.White;
            Describe.TextColor = Color.Black;


            Actions.BackgroundColor = Color.White;
            Actions.TextColor = Color.Black;


          Things.BackgroundColor = Color.White;
            Things.TextColor = Color.Black;

            if (e.Item == null)
            {
                return;
            }

            selected = e.Item.ToString();

            

            Describe.Text = "describe " + selected + "  ";
            Describe.IsVisible = true;

            Actions.Text = selected + " actions  ";
            Actions.IsVisible = true;

            Things.Text = selected + " things   ";
            Things.IsVisible = true;


        }




        private void DescribeGestureRecognizer_Tapped(object sender, EventArgs e)
        {

          Undo.IsVisible = true;

            Describe.TextColor = Color.White;

            Describe.BackgroundColor = Color.Purple;

            Things.TextColor = Color.Black;
            Things.BackgroundColor = Color.White;



            Actions.TextColor = Color.Black;
            Actions.BackgroundColor = Color.White;


            // getting adjectives related to selected string from datamuse api
            WebClient client = new WebClient();
            var response = client.DownloadData("https://api.datamuse.com/words?rel_jjb=" + selected);


            // converting json to list of objects

            var json = System.Text.Encoding.UTF8.GetString(response);
            var adjectives = JsonConvert.DeserializeObject<List<DatamuseWords>>(json);


            // clearing list 
            Items.Clear();

            int i = 0;

            
            //adding words to list 
                for (i = 0; i < adjectives.Count; i++)
                {
                    Items.Add(adjectives[i].word);

                    //  and adding to stack(for undo)
                    liststack.Push(adjectives[i].word);
                

                };

            countstack.Push(i);
        



     selectedstack.Push(selected);

            categorystack.Push("describe");

        }





        private void ThingsGestureRecognizer_Tapped(object sender, EventArgs e)
        {

         Undo.IsVisible = true;

            Things.TextColor = Color.White;

            Things.BackgroundColor = Color.Purple;


            Describe.TextColor = Color.Black;

            Describe.BackgroundColor = Color.White;

            Actions.TextColor = Color.Black;
            Actions.BackgroundColor = Color.White;


            // getting adjectives related to selected string from datamuse api
            WebClient client = new WebClient();
            var response = client.DownloadData("https://api.datamuse.com/words?rel_jja=" + selected);


            // converting json to list of objects

            var json = System.Text.Encoding.UTF8.GetString(response);
            var nouns = JsonConvert.DeserializeObject<List<DatamuseWords>>(json);


            // clearing list 
            Items.Clear();

            int i = 0;
            //adding words to list 

            
                for (i = 0; i < nouns.Count; i++)
                {
                    Items.Add(nouns[i].word);

                    //  and adding to stack(for undo)
                    liststack.Push(nouns[i].word);
                

                };

                countstack.Push(i);
            

            selectedstack.Push(selected);

            categorystack.Push("things");
            
        }





        private void ActionsGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Items.Clear();

         Undo.IsVisible = true;

            Actions.TextColor = Color.White;

            Actions.BackgroundColor = Color.Purple;

            Things.TextColor = Color.Black;
            Things.BackgroundColor = Color.White;


            Describe.TextColor = Color.Black;

            Describe.BackgroundColor = Color.White;




            WebClient client = new WebClient();

            var response = client.DownloadData("https://api.datamuse.com/words?rel_trg=" + selected + "&md=p");

            var json = System.Text.Encoding.UTF8.GetString(response);
            var verbs = JsonConvert.DeserializeObject<List<DatamuseWords>>(json);


            int count = 0;


            if (verbs.Count() != 0)
            { 
               for (int i = 0; i < verbs.Count; i++)
            
                if (verbs[i].tags.Contains("v"))

                {
                    count++;
                }


            } 

                int j = 0;


              int x = -1;

            if (count != 0)
            {
                do
                {
                    for (x = x + 1; x < verbs.Count(); x++)
                    {
                        if (verbs[x].tags.Contains("v"))

                        {
                            Items.Add(verbs[x].word);

                            //  and adding to stack(for undo)
                            liststack.Push(verbs[x].word);
                            j++;
                            break;
                        }


                    }
                    

                } while (j < count);

            }
            

                countstack.Push(j);

                selectedstack.Push(selected);


                categorystack.Push("actions");
        
        

        }






    }

}
