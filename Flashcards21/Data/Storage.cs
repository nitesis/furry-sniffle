using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Media.Imaging;


// comment
namespace Flashcards21.Data
{
    public class Storage : IStorage
    {
        private const string index = "box.xml";
        IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
        
        public Storage(bool doReset)
        {
            if (doReset)
            {
                reset();
            }
        }

        public void reset()
        {
            deleteAll();

            int[] order = new int[12];
            for (int i = 0; i < 12; i++) order[i] = i;

            StoreCardBox("zahlen-englisch.zip", "Zahlen Englisch", "Zahlen von 1 - 12", 8, DateTime.Now, order);
            StoreCardBox("Languages", "Sprachen", "Welche Sprache wird wo gesprochen?", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            StoreCardBox("Cars", "Autos", "Kennen Sie diese Automarken?", 1, DateTime.Now, new int[] { 1, 2, 3, 4 });
            StoreCardBox("Airplains", "Flugzeuge", "Kennen Sie diese Flugzeuge?", 2, DateTime.Now, new int[] { 1, 2, 3, 4 });
            StoreCardBox("Capitals", "Hauptstädte", "Welches ist die Hauptstadt von...?", 3, DateTime.Now, new int[] { 1, 2, 3, 4 });
            StoreCardBox("Continents", "Kontinente", "Auf welchem Kontinent liegt...?", 4, DateTime.Now, new int[] { 1, 2, 3, 4 });
            StoreCardBox("Mountains", "Berge", "Wo liegt Berg X?, Wie hoch ist er?", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            StoreCardBox("History", "Geschichte", "Wann passierte was?", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            StoreCardBox("English", "Englisch", "Englische Vokabeln pauken", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            StoreCardBox("Francais", "Französisch", "Französische Vokabeln pauken", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            StoreCardBox("Celebrities", "Berühmtheiten", "Wer ist das?", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });
            StoreCardBox("Sports", "Sport", "Sportergebnisse", 0, DateTime.Now, new int[] { 1, 2, 3, 4 });

            StoreCard("zahlen-englisch.zip", 0, "Eins", "one", null);
            StoreCard("zahlen-englisch.zip", 1, "Zwei", "two", null);
            StoreCard("zahlen-englisch.zip", 2, "Drei", "three", null);
            StoreCard("zahlen-englisch.zip", 3, "Vier", "four", null);
            StoreCard("zahlen-englisch.zip", 4, "Fünf", "five", null);
            StoreCard("zahlen-englisch.zip", 5, "Sechs", "six", null);
            StoreCard("zahlen-englisch.zip", 6, "Sieben", "seven", null);
            StoreCard("zahlen-englisch.zip", 7, "Acht", "eight", null);
            StoreCard("zahlen-englisch.zip", 8, "Neun", "nine", null);
            StoreCard("zahlen-englisch.zip", 9, "Zehn", "ten", null);
            StoreCard("zahlen-englisch.zip", 10, "Elf", "eleven", null);
            StoreCard("zahlen-englisch.zip", 11, "Zwölf", "twelve", null);
        }

        private void deleteAll()
        {
            string[] dirnames = storage.GetDirectoryNames();
            foreach (string dir in dirnames)
            {
                deleteDirectoryContents(dir);
                storage.DeleteDirectory(dir);
            }
        }

        private void deleteDirectoryContents(String dirname)
        {
            string[] files = storage.GetFileNames(Path.Combine(dirname, "*"));
            foreach (string filename in files)
            {
                storage.DeleteFile(Path.Combine(dirname, filename));
            }
        }

        public DAL.CardBoxDTO LoadCardBox(string filename)
        {
            DAL.CardBoxDTO result;
            IsolatedStorageFileStream file = null;
            try
            {
                file = storage.OpenFile(Path.Combine(filename, index), FileMode.Open);
                XmlSerializer xml = new XmlSerializer(typeof(DAL.CardBoxDTO));
                result = (DAL.CardBoxDTO)xml.Deserialize(file);
                return result;
            }
            catch (Exception e)
            {
                throw new DAL.DALException(e);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
        }

        public void UpdateCardBox(string filename, int position, DateTime lastAccessed, int[] cardpos = null)
        {
            IsolatedStorageFileStream file = null;
            try
            {
                DAL.CardBoxDTO cb = LoadCardBox(filename);
                DAL.CardBoxDTO cbnew = new DAL.CardBoxDTO(cb, position, lastAccessed, cardpos);
                storage.DeleteFile(Path.Combine(filename, index));
                file = storage.CreateFile(Path.Combine(filename, index));
                XmlSerializer xml = new XmlSerializer(typeof(DAL.CardBoxDTO));
                xml.Serialize(file, cbnew);
            }
            catch (Exception e)
            {
                throw new DAL.DALException(e);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
        }

        public void StoreCardBox(string filename, string title, string description, int nofCards)
        {
            DateTime lastAccessed = DateTime.Now;
            int[] cardpos = new int[nofCards];
            for (int i = 0; i < nofCards; i++) cardpos[i] = i;
            IsolatedStorageFileStream file = null;
            try
            {
                DAL.CardBoxDTO cb = new DAL.CardBoxDTO(filename, title, description, 0, lastAccessed, cardpos);
                storage.CreateDirectory(filename);
                file = storage.CreateFile(Path.Combine(filename, index));
                XmlSerializer xml = new XmlSerializer(typeof(DAL.CardBoxDTO));
                xml.Serialize(file, cb);
            }
            catch (Exception e)
            {
                throw new DAL.DALException(e);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
        }

        public void StoreCardBox(string filename, string name, string description,
            int position, DateTime lastAccessed, int[] cardpos)
        {
            IsolatedStorageFileStream file = null;
            try
            {
                DAL.CardBoxDTO cb = new DAL.CardBoxDTO(filename, name, description, position, lastAccessed, cardpos);
                storage.CreateDirectory(filename);
                file = storage.CreateFile(Path.Combine(filename, index));
                XmlSerializer xml = new XmlSerializer(typeof(DAL.CardBoxDTO));
                xml.Serialize(file, cb);
            }
            catch (Exception e)
            {
                throw new DAL.DALException(e);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
        }

        public DAL.CardDTO LoadCard(string filename, int position)
        {
            DAL.CardDTO result;
            IsolatedStorageFileStream file = null;
            try
            {
                file = storage.OpenFile(Path.Combine(filename, position.ToString("D5")), FileMode.Open);
                XmlSerializer xml = new XmlSerializer(typeof(DAL.CardDTO));
                result = (DAL.CardDTO)xml.Deserialize(file);
                return result;
            }
            catch (Exception e) 
            {
                throw new DAL.DALException(e);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
        }

        public void StoreCard(string boxfilename, int position, string question, string answer, string image)
        {
            DAL.CardDTO card = new DAL.CardDTO(question, answer, image);
            IsolatedStorageFileStream file = null;
            try
            {
                file = storage.CreateFile(Path.Combine(boxfilename, position.ToString("D5")));
                XmlSerializer xml = new XmlSerializer(typeof(DAL.CardDTO));
                xml.Serialize(file, card);
            }
            catch (Exception e)
            {
                throw new DAL.DALException(e);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
        }

        public List<string> GetAllCardBoxIDs()
        {
            string[] boxes = storage.GetDirectoryNames();
            List<string> result = new List<string>();
            foreach (var b in boxes)
            {
                result.Add(b);
            }
            return result;
        }

        public void DeleteCardBox(string filename)
        {
            deleteDirectoryContents(filename);
            storage.DeleteDirectory(filename);
        }



        public BitmapImage LoadImage(string boxname, string imagename)
        {
            IsolatedStorageFileStream file = new IsolatedStorageFileStream(Path.Combine(boxname, imagename), FileMode.Open, storage);
            BitmapImage bmp = new BitmapImage();
            bmp.SetSource(file);
            return bmp;
        }
    }
}
