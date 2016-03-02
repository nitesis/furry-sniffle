using System;
using System.ComponentModel;

namespace Flashcards21
{
    /// <summary>
    /// Serves as a simple ViewModel for the items of the MainPage. All the items are immutable,
    /// ie the cardboxes titles and descriptions will not change while the main page is displayed.
    /// Therefore this ViewModel does not implement the interface INotifyPropertyChanged.
    /// </summary>
    public class ItemViewModel
    {
        private string _filename;
        /// <summary>
        /// The Filename is used to retrieve persistent data of a cardbox from the isolated store.
        /// </summary>
        public string Filename
        {
            get
            {
                return _filename;
            }
            set
            {
                if (value != null)
                {
                    _filename = value;
                }
                else
                {
                    throw new ArgumentNullException("Filename must not be null");
                }
            }
        }

        /// <summary>
        /// The title of the cardbox.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A description of the content of the cardbox.
        /// </summary>
        public string Description { get; set; }
    }
}