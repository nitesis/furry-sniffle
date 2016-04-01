using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using Flashcards21.ViewModels;

namespace Flashcards21
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            IsolatedStorageSettings localStorage = IsolatedStorageSettings.ApplicationSettings;
            List<DownloadableItemViewModel> itemList = new List<DownloadableItemViewModel>();

            //Wenn nichts gespeichert ist, da wird auch nichts gemacht
            if (!localStorage.Contains("Items"))
            {
                this.Items.Add(new ItemViewModel() { Title = "No cardboxes...", Filename = "Dictumst eleifend facilisi faucibus", Description = "...available, go to download to get some." });
            }
            else
            {
                itemList = localStorage["Items"] as List<DownloadableItemViewModel>;

                foreach (DownloadableItemViewModel item in itemList)
                {
                    this.Items.Add(new ItemViewModel() { Title = item.Title, Filename = item.Filename, Description = item.Description });          
                }
            }

            this.IsDataLoaded = true;
        }

        public bool isAvailable(String filename)
        {
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void newAvailableCardBox(DownloadableItemViewModel item)
        {
            IsolatedStorageSettings localStorage = IsolatedStorageSettings.ApplicationSettings;
            List<DownloadableItemViewModel> itemList;

            if (!localStorage.Contains("Items"))
            {
                itemList = new List<DownloadableItemViewModel>();
                Items.Clear();
            }
            else
            {
                itemList = localStorage["Items"] as List<DownloadableItemViewModel>;
            }

            itemList.Add(item);
            Items.Add(new ItemViewModel() { Title = item.Title, Filename = item.Filename, Description = item.Description });

            localStorage["Items"] = itemList;
            localStorage.Save();
        }

    }
}