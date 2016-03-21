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
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Windows.Media.Imaging;

namespace Flashcards21.Data
{
    public static class DAL
    {
        private static IStorage _storage;

        public class DALException : Exception
        {
            public DALException(Exception inner) : base("DAL Exception", inner) { }
        }

        public struct CardBoxDTO
        {
            public CardBoxDTO(string filename, string name, string description, int position, DateTime accessed, int[] order) : this()
            {
                // parameter validation
                filename = filename.Trim();
                if (filename == null)
                    throw new ArgumentNullException("filename");
                if (filename.Trim().Equals(""))
                    throw new ArgumentException("no printable characters", "filename");
                if (name == null)
                    throw new ArgumentNullException("name");
                if (name.Trim().Equals(""))
                    throw new ArgumentException("no printable characters", "name");
                if (accessed == null)
                    throw new ArgumentNullException("accessed");
                if (order == null)
                    throw new ArgumentNullException("order");
                if (position < 0 || position > order.Length)
                    throw new IndexOutOfRangeException("param position must be in range of array order");

                // member initialization
                try
                {
                    Filename = filename.Trim();
                    Name = name.Trim();
                    Description = description == null ? "" : description;
                    Position = position;
                    LastAccessed = accessed;
                    this.order = new int[order.Length];
                    for (int i = 0; i < order.Length; i++) this.order[i] = order[i];
                }
                catch (Exception e)
                {
                    throw new DALException(e);
                }
            }

            public CardBoxDTO(CardBoxDTO orig, int position, DateTime accessed, int[] order = null) : this()
            {
                int len = order == null ? orig.order.Length : order.Length;
                // parameter validation
                if (accessed == null)
                    throw new ArgumentNullException("accessed");
                if (position < 0 || position > len)
                    throw new IndexOutOfRangeException("param position must be in range of array order");

                // member initialization
                try
                {
                    Filename = orig.Filename;
                    Name = orig.Name;
                    Description = orig.Description;
                    Position = position;
                    LastAccessed = accessed;
                    this.order = new int[len];
                    if (order == null) order = orig.order;
                    for (int i = 0; i < order.Length; i++) this.order[i] = order[i];
                }
                catch (Exception e)
                {
                    throw new DALException(e);
                }
            }

            public string Filename;

            public string Name;

            public String Description;

            public int Position;

            public DateTime LastAccessed;

            [XmlIgnore]
            public int Size { get { return order.Length; } }

            public int[] order;
            [XmlIgnore]
            public int this[int index] { get { return order[index]; } }
        }

        public struct CardDTO
        {
            public CardDTO(String question, String answer, String picture) : this()
            {
                if (question == null) throw new ArgumentNullException("question");
                if (answer == null) throw new ArgumentNullException("answer");
                Question = question;
                Answer = answer;
                Picture = picture;
            }

            public String Question { get; set; }
            public String Answer { get; set; }
            public String Picture { get; set; }
        }

        public static IStorage Storage
        {
            private get { return _storage; }
            set
            {
                if (_storage != null)
                    throw new System.InvalidOperationException();
                else if (value == null)
                    throw new System.NullReferenceException();
                else
                    _storage = value;
            }
        }

        public static CardBoxDTO getCardBoxFor(String filename)
        {
            if (_storage == null)
                throw new TypeInitializationException("Flashcards.Data.DAL", null);
            if (filename == null)
                throw new ArgumentException("", filename);
            else
                return _storage.LoadCardBox(filename);
        }

        public static void storeCard(string boxfilename, int position, string question, string answer, string imagefilename)
        {
            if (_storage == null)
                throw new TypeInitializationException("Flashcards.Data.DAL", null);
            if (boxfilename == null)
                throw new ArgumentException("", boxfilename);
            else
                _storage.StoreCard(boxfilename, position, question, answer, imagefilename);
        }

        public static CardDTO getCard(String filename, int position)
        {
            if (_storage == null)
                throw new TypeInitializationException("Flashcards.Data.DAL", null);
            if (filename == null)
                throw new ArgumentException("", filename);
            else
                return _storage.LoadCard(filename, position);
        }

        public static BitmapImage getImage(String boxname, String imagename)
        {
            if (_storage == null)
                throw new TypeInitializationException("Flashcards.Data.DAL", null);
            if (boxname == null)
                throw new ArgumentException("", boxname);
            if (imagename == null)
                throw new ArgumentException("", imagename);
            else
                return _storage.LoadImage(boxname, imagename);
        }

        public static void storeCardBox(String filename, int position, DateTime lastAccessed, int[] cards = null)
        {
            if (_storage == null)
                throw new TypeInitializationException("Flashcards.Data.DAL", null);
            if (filename == null)
                throw new ArgumentException("", filename);
            else
                _storage.UpdateCardBox(filename, position, lastAccessed, cards);
        }

        public static void storeCardBox(String filename, String title, String description, int nofCards)
        {
            if (_storage == null)
                throw new TypeInitializationException("Flashcards.Data.DAL", null);
            if (filename == null)
                throw new ArgumentException("", filename);
            else
                _storage.StoreCardBox(filename, title, description, nofCards);
        }

        public static List<String> AllCardBoxIDs
        {
            get { return _storage.GetAllCardBoxIDs(); }
        }

        public static void DeleteCardBox(String id)
        {
            _storage.DeleteCardBox(id);
        }

    }
}
