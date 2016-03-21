using System;
using System.Net;

using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

namespace Flashcards21.Data
{
    public class MockStorage : IStorage
    {
        private List<DAL.CardDTO> cards = new List<DAL.CardDTO>();
        private Dictionary<String, DAL.CardBoxDTO> boxes = new Dictionary<string, DAL.CardBoxDTO>();
        private DateTime _lastAccessed = DateTime.Now;
        private int[] order;
        public MockStorage()
        {
            reset();
        }

        public void reset()
        {
            cards.Clear();
            cards.Add(new DAL.CardDTO("Eins", "one", null));
            cards.Add(new DAL.CardDTO("Zwei", "two", null));
            cards.Add(new DAL.CardDTO("Drei", "three", null));
            cards.Add(new DAL.CardDTO("Vier", "four", null));
            cards.Add(new DAL.CardDTO("Fünf", "five", null));
            cards.Add(new DAL.CardDTO("Sechs", "six", null));
            cards.Add(new DAL.CardDTO("Sieben", "seven", null));
            cards.Add(new DAL.CardDTO("Acht", "eight", null));
            cards.Add(new DAL.CardDTO("Neun", "nine", null));
            cards.Add(new DAL.CardDTO("Zehn", "ten", null));
            cards.Add(new DAL.CardDTO("Elf", "eleven", null));
            cards.Add(new DAL.CardDTO("Zwölf", "twelve", null));

            order = new int[cards.Count];
            for (int i = 0; i < order.Length; i++) order[i] = i;

            boxes["TestFilename"] = new DAL.CardBoxDTO("TestFilename", "TestName", "TestDescription", 8, _lastAccessed, order);
            boxes["Languages"] = new DAL.CardBoxDTO("Languages", "Sprachen", "Welche Sprache wird wo gesprochen?", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            boxes["Cars"] = new DAL.CardBoxDTO("Cars", "Autos", "Kennen Sie diese Automarken?", 1, DateTime.Now, new int[] { 1, 2, 3, 4 });
            boxes["Airplains"] = new DAL.CardBoxDTO("Airplains", "Flugzeuge", "Kennen Sie diese Flugzeuge?", 2, DateTime.Now, new int[] { 1, 2, 3, 4 });
            boxes["Capitals"] = new DAL.CardBoxDTO("Capitals", "Hauptstädte", "Welches ist die Hauptstadt von...?", 3, DateTime.Now, new int[] { 1, 2, 3, 4 });
            boxes["Continents"] = new DAL.CardBoxDTO("Continents", "Kontinente", "Auf welchem Kontinent liegt...?", 4, DateTime.Now, new int[] { 1, 2, 3, 4 });
            boxes["Mountains"] = new DAL.CardBoxDTO("Mountains", "Berge", "Wo liegt Berg X?, Wie hoch ist er?", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            boxes["History"] = new DAL.CardBoxDTO("History", "Geschichte", "Wann passierte was?", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            boxes["English"] = new DAL.CardBoxDTO("English", "Englisch", "Englische Vokabeln pauken", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            boxes["Francais"] = new DAL.CardBoxDTO("Francais", "Französisch", "Französische Vokabeln pauken", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            boxes["Celebrities"] = new DAL.CardBoxDTO("Celebrities", "Berühmtheiten", "Wer ist das?", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            boxes["Sports"] = new DAL.CardBoxDTO("Sports", "Sport", "Sportergebnisse", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
        }

        public DateTime LastAccessed
        {
            get { return _lastAccessed; }
            set { _lastAccessed = value; }
        }

        public DAL.CardBoxDTO LoadCardBox(string filename)
        {
            try
            {
                return boxes[filename];
            }
            catch (Exception e)
            {
                throw new DAL.DALException(e);
            }
        }

        public DAL.CardDTO LoadCard(string filename, int position)
        {
            if (filename.Trim().Equals("TestFilename"))
            {
                try
                {
                    return cards[position];
                }
                catch (Exception e)
                {
                    throw new DAL.DALException(e);
                }
            }
            else
                throw new DAL.DALException(null);
        }

        public void storeCardBox(string filename, int position, DateTime lastAccessed)
        {
            try
            {
                DAL.CardBoxDTO cb = boxes[filename];
                boxes[filename] = new DAL.CardBoxDTO(filename, cb.Name, cb.Description, position, lastAccessed, order);
            }
            catch (Exception e)
            {
                throw new DAL.DALException(e);
            }
        }

        public void UpdateCardBox(string filename, int position, DateTime lastAccessed, int[] cardpos)
        {
            try
            {
                DAL.CardBoxDTO cb = boxes[filename];
                boxes[filename] = new DAL.CardBoxDTO(filename, cb.Name, cb.Description, position, lastAccessed, cardpos);
            }
            catch (Exception e)
            {
                throw new DAL.DALException(e);
            }
        }

        public List<String> GetAllCardBoxIDs()
        {
            List<String> result = new List<String>();
            foreach (var b in boxes)
            {
                result.Add(b.Key);
            }
            return result;
        }


        public void DeleteCardBox(string filename)
        {
            boxes.Remove(filename);
        }


        public void CreateCardBox(string filename, string title, string description, int nofCards)
        {
            throw new NotImplementedException();
        }


        public void StoreCardBox(string filename, string title, string description, int nofCards)
        {
            throw new NotImplementedException();
        }

        public void StoreCard(string boxfilename, int position, string question, string answer, string image)
        {
            throw new NotImplementedException();
        }


        public BitmapImage LoadImage(string boxname, string imagename)
        {
            throw new NotImplementedException();
        }
    }
}

