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
            // ObservableCollection (zeigt zur Laufzeit Änderungen an) ist abgeleitet von DependencyObject
            this.Items = new ObservableCollection<ItemViewModel>();
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
            // Sample data; replace with real data
            /* this.Items.Add(new ItemViewModel() { Title = "cardbox one", Filename = "Maecenas praesent accumsan bibendum", Description = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox two", Filename = "Dictumst eleifend facilisi faucibus", Description = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox three", Filename = "Habitant inceptos interdum lobortis", Description = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox four", Filename = "Nascetur pharetra placerat pulvinar", Description = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox five", Filename = "Maecenas praesent accumsan bibendum", Description = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox six", Filename = "Dictumst eleifend facilisi faucibus", Description = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox seven", Filename = "Habitant inceptos interdum lobortis", Description = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox eight", Filename = "Nascetur pharetra placerat pulvinar", Description = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox nine", Filename = "Maecenas praesent accumsan bibendum", Description = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox ten", Filename = "Dictumst eleifend facilisi faucibus", Description = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox eleven", Filename = "Habitant inceptos interdum lobortis", Description = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox twelve", Filename = "Nascetur pharetra placerat pulvinar", Description = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox thirteen", Filename = "Maecenas praesent accumsan bibendum", Description = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox fourteen", Filename = "Dictumst eleifend facilisi faucibus", Description = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
             this.Items.Add(new ItemViewModel() { Title = "cardbox fifteen", Filename = "Habitant inceptos interdum lobortis", Description = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            */

            IsolatedStorageSettings localStorage = IsolatedStorageSettings.ApplicationSettings;
            List<DownloadableItemViewModel> itemList = new List<DownloadableItemViewModel>();

            //Wenn nichts gespeichert ist, da wird auch nichts gemacht
            if (!localStorage.Contains("Items"))
            {
                this.Items.Add(new ItemViewModel() { Title = "cardbox fourteen", Filename = "Dictumst eleifend facilisi faucibus", Description = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
                this.Items.Add(new ItemViewModel() { Title = "cardbox fifteen", Filename = "Habitant inceptos interdum lobortis", Description = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });

            }
            else
            {

                itemList = IsolatedStorageSettings.ApplicationSettings["Items"]
                                                                          as List<DownloadableItemViewModel>;

                foreach (DownloadableItemViewModel item in itemList)
                {
                    this.Items.Add(new ItemViewModel() { Title = item.Title, Filename = "huhu", Description = item.Description });
          
                }
            }

            localStorage.Save();
            //this.Items.Add(new ItemViewModel() { Title = "cardbox sixteen", Filename = "Nascetur pharetra placerat pulvinar", Description = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

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

        internal void newAvailableCardBox(string filename)
        {
            throw new NotImplementedException();
        }
    }
}