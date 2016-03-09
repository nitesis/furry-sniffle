using System;
using System.ComponentModel;
using System.Windows.Navigation;
using Flashcards21.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Flashcards21.Views
{
    public partial class CardboxPage : PhoneApplicationPage
    {
        private CardboxViewModel _cardbox;
        public string Filename { get; set; }

        public CardboxPage()
        {
            InitializeComponent();
            appbarLearnBtn = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            appbarShuffleBtn = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            appbarResetBtn = ApplicationBar.Buttons[2] as ApplicationBarIconButton;
            appbarSyncBtn = ApplicationBar.Buttons[3] as ApplicationBarIconButton;
        }

        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string filename = "";
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out filename))
            {
                //Jedes GUI Objekt hat ein Property DataContext, darauf wird ein Objekt geholt und auf diesem dann ein Property name geholt
                //Binding von View zu ViewModel
                DataContext = new CardboxViewModel(filename);
                _cardbox = DataContext as CardboxViewModel;
                _cardbox.PropertyChanged += PositionChangedHandler;
                updateGUI();
            }
        }

        private void updateGUI()
        {
            appbarLearnBtn.IsEnabled = _cardbox.BoxNotDone;
            appbarResetBtn.IsEnabled = _cardbox.Position != 0;
            progressanimation.To = _cardbox.Position;
            storyboard.Begin();
        }

        void PositionChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Position"))
            {
                updateGUI();
            }
        }

        private void OnSyncClick(object sender, EventArgs e)
        {
            // so something when user wants to sync
        }

        private void OnResetClick(object sender, EventArgs e)
        {
            // reset card box's learning statistics
        }

        private void OnShuffleClick(object sender, EventArgs e)
        {
            // shuffle cards
        }

        private void OnLearnClick(object sender, EventArgs e)
        {
            // start learning by navigating to question page
        }

    }
}