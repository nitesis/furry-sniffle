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

namespace Flashcards21.PersistentData
{
    public class Storage
    {
        public static void getCardbox(String filename,
            out string title,
            out string description,
            out int position,
            out int size,
            out DateTime lastAccessed)
        {
            title = "Hello World";
            description = "Description of the world.";
            position = 42;
            size = 88;
            lastAccessed = DateTime.Now;
            ;
        }
    }
}
