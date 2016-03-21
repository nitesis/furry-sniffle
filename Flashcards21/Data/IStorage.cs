using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;


namespace Flashcards21.Data
{
    public interface IStorage
    {
        List<String> GetAllCardBoxIDs();
        void StoreCardBox(string filename, string title, string description, int nofCards);
        DAL.CardBoxDTO LoadCardBox(String filename);
        void UpdateCardBox(string _filename, int position, DateTime _lastAccessed, int[] cards);
        void DeleteCardBox(string filename);

        DAL.CardDTO LoadCard(String filename, int position);
        void StoreCard(string boxfilename, int position, string question, string answer, string image);
        BitmapImage LoadImage(string boxname, string imagename);
    }
}
