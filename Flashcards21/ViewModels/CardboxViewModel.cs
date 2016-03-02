using System;
using System.ComponentModel;
using Flashcards21.PersistentData;

namespace Flashcards21.ViewModels
{
    public class CardboxViewModel : INotifyPropertyChanged
    {
        private String _name;
        private String _description;
        private String _filename;
        private int _position;
        private DateTime _lastAccessed;
        private int[] cards;
        private bool answerExpected = false;
        private Random rand = new Random();

        public CardboxViewModel(String filename)
        {
            if (filename == null || filename.Trim().Equals("")) throw new ArgumentNullException("filename");

            int size = 0;
            Storage.getCardbox(filename, out _name, out _description, out _position, out size, out _lastAccessed);
            _filename = filename;
            cards = new int[size];
        }

        public String Filename { get { return _filename; } }

        public String Name { get { return _name; } }

        public String Description { get { return _description; } }

        public int Size { get { return cards.Length; } }

        public int Position
        {
            get { return _position; }
            private set
            {
                if (_position != value)
                {
                    _position = value;
                    NotifyPropertyChanged("Position");
                    NotifyPropertyChanged("LearningProgress");
                    NotifyPropertyChanged("BoxNotDone");
                }
            }
        }

        public String LearningProgress { get { return "" + Position + " of " + Size; } }

        public Boolean BoxNotDone { get { return _position != cards.Length; } }

        public DateTime LastAccessed { get { return _lastAccessed; } }

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
}
