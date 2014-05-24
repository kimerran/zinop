using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zinop
{
    class Program
    {
        private static void Log(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[INFO ] " + msg);
        }

        private static void Err(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] " + msg);
        }


        static void Main(string[] args)
        {
           
            string cur_path = args[0];
            int err_count = 0;

            // Get the filenames
            List<string> filenames = System.IO.Directory.EnumerateFiles(cur_path).ToList();

            // Get unique extension list
            List<string> extensions = new List<string>();
            foreach (var item in filenames)
            {
                var arr = item.Split('.');
                if (arr.Length != 1)
                {
                    string ext = (string)arr[arr.Length - 1];
                    extensions.Add(ext.ToLower());
                }
            }

            extensions = extensions.Distinct<string>().ToList();
            Log("Found " + extensions.Count + " extensions");

            // Create directory for zinop
            System.IO.Directory.CreateDirectory(cur_path + "/zinop");

            // Process each extension
            foreach (var extension in extensions)
            {
                // create sub-directory for each extension
                System.IO.Directory.CreateDirectory(cur_path + "/zinop/" + extension);

                // Get all files matching extension
                List<string> matches = filenames.Where(o => o.ToLower().Contains("." + extension)).ToList();

                foreach (var match in matches)
                {
                    var arr = match.Split('\\');
                    var filename = arr[arr.Length - 1];
                    var source = cur_path + "\\" + filename;
                    var destin = cur_path + "\\zinop\\" + extension + "\\" + filename;

                    try
                    {
                        Log("Moving " + filename);
                        System.IO.File.Move(source, destin);
                    }
                    catch (Exception e)
                    {
                        Err(e.Message);
                        err_count++;
                    }

                }

            }

            if (err_count != 0)
            {
                Err("Finished with errors - " + err_count);
            }
            Log("Press any key to continue...");
            Console.Read();
        }
    }
}
