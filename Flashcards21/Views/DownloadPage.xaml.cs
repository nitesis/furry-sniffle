﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Flashcards21.ViewModels;

namespace Flashcards21.Views
{
    public partial class DownloadPage : PhoneApplicationPage
    {
        private DownloadViewModel download;
        public DownloadPage()
        {
            InitializeComponent();
            download = new DownloadViewModel();
            DataContext = download;
            this.Loaded += new RoutedEventHandler(DownloadableCardboxes_Loaded);
        }

        private void selectionChanged(object sender, SelectionChangedEventArgs e)
        {
       
            // If selected index is -1 (no selection) do nothing
            if (DownloadListBox.SelectedIndex != -1)
            {
                int chosenCardboxID = DownloadListBox.SelectedIndex;

                download.download(chosenCardboxID);
            }
        }

        private void DownloadableCardboxes_Loaded(object sender, RoutedEventArgs e)
        {
            download.LoadData();
        }
    }
}