using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Flashcards21;

using Flashcards21.Data;


namespace Flashcards21.ViewModels
{
    public class DownloadViewModel : INotifyPropertyChanged
    {
        private const string index = "index.xml";
        Downloader downloader;

        private const string serverAddress = "http://web.fhnw.ch/plattformen/mad/flashcards/";

        #region Possible modes for an available item: Available, Downloading, Initializing, Done
        internal enum ItemMode
        {
            Available, Downloading, Initializing, Done
        }
        #endregion

        #region AvailableItem representing a cardbox that was found on server but not on phone
        public class AvailableItem : INotifyPropertyChanged
        {
            private bool hasImages;
            private int size;

            private const string SIZE = "Size: ";
            private const string DOWNLOAD = "Downloading...";
            private const string UNPACK = "Initializing...";
            private const string DONE = "Download completed";
            private ItemMode mode = ItemMode.Available;

            public AvailableItem(String name, String description, String filename, int size, bool hasImages)
            {
                this.Name = name; this.Description = description; this.Filename = filename;
                this.size = size; this.hasImages = hasImages;
            }

            public string Name { get; private set; }
            public string Description { get; private set; }
            public string Filename { get; private set; }

            public string Size
            {
                get
                {
                    return (mode == ItemMode.Available) ? BytesToString(size) : "";
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

            public string HasImages
            {
                get
                {
                    return "../Images/images" + ((hasImages) ? "yes" : "no") + ".png";
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

        }
        #endregion

        public DownloadViewModel()
        {
            this.IsDataLoaded = false;
            this.Items = new ObservableCollection<AvailableItem>();
        }

        #region Download of Overview
        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<AvailableItem> Items { get; private set; }
        private bool _isDownloadInProgress = false;

        public bool IsDataLoaded
        {
            get;
            private set;
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

        private void downloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            //do nothing
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
                                    AvailableItem item = new AvailableItem(name, desc, file, bytes, img);
                                    Items.Add(item);
                                }
                                break;
                        }
                    } while (reader.Read());
                    IsDataLoaded = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Cannot access list of available card boxes");
                }
                finally
                {
                    IsDownloadInProgress = false;
                }
            }
        }

        private bool locallyUnknown(string file)
        {
            MainViewModel m = (MainViewModel)App.ViewModel;
            return !m.isAvailable(file);
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
        #endregion

        internal void download(int index)
        {
            AvailableItem item = Items[index];

            if (item.Mode == ItemMode.Available)
            {
                downloader = new Downloader(item);
                WebClient web = new WebClient();
                web.OpenReadCompleted += new OpenReadCompletedEventHandler(downloader.itemReadCompleted);
                item.Mode = ItemMode.Downloading;
                web.OpenReadAsync(new Uri(serverAddress + item.Filename + ".zip"));
            }
        }

        class Downloader
        {
            private AvailableItem item;

            public Downloader(AvailableItem item)
            {
                this.item = item;
            }

            public void itemReadCompleted(object sender, OpenReadCompletedEventArgs args)
            {
                item.Mode = ItemMode.Initializing;
                if (!args.Cancelled && args.Error == null)
                {
                    IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                    storage.CreateDirectory(item.Filename);

                    // unzip files into new directory.
                    Unzip(args.Result, storage);
                    // look index.xml and process its contents to create card box and cards.
                    ProcessXML(storage);
                    // remove index.xml file as it is now obsolete.
                    storage.DeleteFile(Path.Combine(item.Filename, index));
                    // tell Overview that new cardbox is available
                    App.ViewModel.newAvailableCardBox(item.Filename);
                }
                item.Mode = ItemMode.Done;
            }

            private void ProcessXML(IsolatedStorageFile storage)
            {
                IsolatedStorageFileStream file = new IsolatedStorageFileStream(Path.Combine(item.Filename, index), FileMode.Open, storage);
                XmlReader reader = XmlReader.Create(file);
                try
                {
                    reader.ReadToFollowing("Cardbox");
                    string filename = reader.GetAttribute("Filename");
                    string name = reader.GetAttribute("Title");

                    reader.ReadToFollowing("Description");
                    string desc = reader.ReadElementContentAsString();

                    int cardCounter = 0;

                    if (reader.ReadToFollowing("Card"))
                    {
                        do
                        {
                            string imagename = reader.GetAttribute("Image");

                            XmlReader subreader = reader.ReadSubtree();

                            string question = null;
                            string answer = null;
                            if (subreader.ReadToFollowing("Question"))
                            {
                                question = subreader.ReadElementContentAsString();
                                if (subreader.ReadToFollowing("Answer"))
                                {
                                    answer = subreader.ReadElementContentAsString();
                                }
                                if (question == null || answer == null)
                                {
                                    throw new NullReferenceException("Question or Answer was empty");
                                }
                            }
                            DAL.storeCard(filename, cardCounter, question, answer, imagename);
                            cardCounter++;
                        } while (reader.ReadToFollowing("Card"));

                        DAL.storeCardBox(filename, name, desc, cardCounter);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Cannot access list of available card boxes");
                }
                finally
                {
                    file.Close();
                    file.Dispose();
                }
            }

            private void Unzip(Stream zipStream, IsolatedStorageFile storage)
            {
                ZipInputStream zipFile = new ZipInputStream(zipStream);
                ZipEntry zipEntry = zipFile.GetNextEntry();
                while (zipEntry != null)
                {
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];     // 4K is optimum

                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(item.Filename, entryFileName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (IsolatedStorageFileStream file = storage.CreateFile(fullZipToPath))
                    {
                        StreamUtils.Copy(zipFile, file, buffer);
                    }
                    zipEntry = zipFile.GetNextEntry();
                }
            }
        }
    }

}
