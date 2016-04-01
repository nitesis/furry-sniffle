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

namespace Flashcards21.ViewModels
{
    public class DownloadableItemViewModel : INotifyPropertyChanged
    {
        internal enum ItemMode
        {
            Available, Downloading, Initializing, Done
        }

        private bool hasImages;
        private int size;

        private const string SIZE = "Size: ";
        private const string DOWNLOAD = "Downloading...";
        private const string UNPACK = "Initializing...";
        private const string DONE = "Download completed";
        private ItemMode mode = ItemMode.Available;

        public DownloadableItemViewModel() { }
        public DownloadableItemViewModel(String title, String description, String filename, int size, bool hasImages)
        {
            this.Title = title; this.Description = description; this.Filename = filename;
            this.size = size; this.hasImages = hasImages;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }

        public string Size
        {
            get
            {
                return (mode == ItemMode.Available) ? BytesToString(size) : "";
            }
            set
            {
                int tmp = 0;
                if (Int32.TryParse(value, out tmp))
                {
                    size = tmp;
                }
            }
        }

        internal ItemMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
                NotifyPropertyChanged("SizeLabel");
                NotifyPropertyChanged("Size");
            }
        }

        public String SizeLabel
        {
            get
            {
                switch (mode)
                {
                    case ItemMode.Available: return SIZE;
                    case ItemMode.Downloading: return DOWNLOAD;
                    case ItemMode.Initializing: return UNPACK;
                    case ItemMode.Done: return DONE;
                    default: return SIZE;
                }
            }
        }

        public String HasImages
        {
            get
            {
                return "../Images/images" + ((hasImages) ? "yes" : "no") + ".png";
            }
            set
            {
                String tmp = value.ToLower();
                if (tmp.Equals("false"))
                {
                    hasImages = false;
                }
                else if (tmp.Equals("true"))
                {
                    hasImages = true;
                }
            }
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

        static String BytesToString(int bytes)
        {
            int magnitude = 1024 * 1024 * 1024; // gigabytes
            String[] units = new String[] { "bytes", "kB", "MB", "GB" };
            int i = 3;

            while (bytes / magnitude == 0)
            {
                magnitude /= 1024;
                i--;
            }

            if (i == 0)
            {
                return "" + bytes + " " + units[i];
            }
            else
            {
                return "" + Math.Round((double)bytes / magnitude, 2) + " " + units[i];
            }
        }
    }
}
