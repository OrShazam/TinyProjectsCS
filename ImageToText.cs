using System.Drawing;
using System;
using System.Reflection;
using System.IO;
namespace ImageToText
{
    class Program
    {
        static string getRandomFilename()
        {
            byte[] buffer = new byte[4];
            new Random().NextBytes(buffer);
            string filename = "";
            foreach (byte b in buffer)
            {
                filename += b.ToString("x2"); // hexadecimal format
            }
            return filename;
        }
        static string[] makeText(Bitmap bitmap)
        {
            char[] chars = new char[] { ' ', '`', '.', ',', ':', ';','+', '#', '@' };
            string[] text = new string[bitmap.Height];
            for (int i = 0; i < text.Length; i++) { text[i] = ""; }
            Color currColor; int currValue;
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    currColor = bitmap.GetPixel(j, i);
                    currValue = (int)Math.Floor(currColor.R * 0.3 + currColor.G * 0.59 + currColor.B * 0.11);
                    // an equation to get grayscale value
                    currValue = Math.Min(chars.Length - 1, currValue / chars.Length);
                    text[i] += chars[currValue];
                }       
            }
            return text;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Type \'quit\' to quit.");
            Start:
            Console.Write("Image Path: ");
            string path = Console.ReadLine();
            if (path == "quit") { return; }
            if (!File.Exists(path) || !path.EndsWith(".jpg"))
            {
                Console.WriteLine("Invalid file path");
                goto Start;
            }
            try
            {
                Bitmap bitmap = new Bitmap(path);
                if (bitmap.Width > 200 || bitmap.Height > 200)
                {
                    Console.WriteLine("Text art is useless for such a big image.");
                    goto Start;
                }
                string[] text = makeText(bitmap);
                string picturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                string filename = getRandomFilename() + ".txt";
                string filePath = Path.Combine(picturesPath, filename);
                Console.WriteLine($"Saving output at {filePath}...");
                try
                {
                    File.WriteAllLines(filePath,text);
                }
                catch
                {
                    Console.WriteLine("IO Failure");
                }
            }
            catch
            {
                Console.WriteLine("IO Failure");
            }
        }
    }
}
