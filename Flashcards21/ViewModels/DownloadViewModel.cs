using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml;

namespace Flashcards21.ViewModels
{
    public class DownloadViewModel : INotifyPropertyChanged
    {
        private const string index = "index.xml";

        //private const string serverAddress = "http://localhost:80/";
        private const string serverAddress = "http://web.fhnw.ch/plattformen/mad/flashcards/";

        public ObservableCollection<DownloadableItemViewModel> Items { get; private set; }
        private bool _isDownloadInProgress = false;

        public bool IsDataLoaded { get; private set; }

        public DownloadViewModel()
        {
            this.IsDataLoaded = false;
            this.Items = new ObservableCollection<DownloadableItemViewModel>();
        }

        public bool IsDownloadInProgress
        {
            get
            {
                return _isDownloadInProgress;
            }

            private set
            {
                if (value != _isDownloadInProgress)
                {
                    _isDownloadInProgress = value;
                    ProgressVisibility = _isDownloadInProgress ? Visibility.Visible : Visibility.Collapsed;
                    NotifyPropertyChanged("IsDownloadInProgress");
                    NotifyPropertyChanged("ProgressVisibility");
                }
            }
        }

        public Visibility ProgressVisibility
        {
            get;
            private set;
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

        internal void LoadData()
        {
            IsDataLoaded = false;
            IsDownloadInProgress = true;
            WebClient web = new WebClient();
            web.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadProgress);
            web.OpenReadCompleted += new OpenReadCompletedEventHandler(readCompleted);
            web.OpenReadAsync(new Uri(serverAddress + "overview.xml"));
        }

        private bool locallyUnknown(string file)
        {
            MainViewModel m = (MainViewModel)App.ViewModel;
            return !m.isAvailable(file);
        }

        private void readCompleted(object sender, OpenReadCompletedEventArgs args)
        {
            if (!args.Cancelled && args.Error == null)
            {
                XmlReader reader = XmlReader.Create(args.Result);
                try
                {
                    reader.MoveToContent();
                    reader.ReadToDescendant("Cardbox");
                    do
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                String file = reader.GetAttribute("Filename");
                                if (locallyUnknown(file))
                                {
                                    String name = reader.GetAttribute("Title");
                                    String desc = reader.GetAttribute("Description");
                                    String size = reader.GetAttribute("Size");
                                    String images = reader.GetAttribute("Images");
                                    int bytes = Int32.Parse(size);
                                    bool img = Boolean.Parse(images);
                                    DownloadableItemViewModel item = new DownloadableItemViewModel(name, desc, file, bytes, img);
                                    Items.Add(item);
                                }
                                break;
                        }
                    } while (reader.Read());
                    IsDataLoaded = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Cannot access list of available card boxes: " + e.Message);
                }
                finally
                {
                    IsDownloadInProgress = false;
                }
            }
        }

        private void downloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            //do nothing
        }

    }
}
