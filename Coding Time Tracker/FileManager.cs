using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Coding_Time_Tracker
{
    /// <summary>
    /// Operates with files which store the data of the application.
    /// </summary>
    public class FileManager
    {
        /// <summary>
        /// Initiates the object with the given filename.
        /// </summary>
        /// <param name="fileLocation">The name of the file to work on.</param>
        public FileManager(string fileLocation)
        {
            _filePath = fileLocation;
            CreateFileIfMissing();
        }


        /// <summary>
        /// Reads a value from the file given a specific key.
        /// </summary>
        /// <param name="lineKey">The line key to find the value for.</param>
        /// <returns>The value that matches the given key.</returns>
        public string ReadValue(LineKey lineKey)
        {
            var lines = File.ReadAllLines(_filePath);

            foreach (var line in lines)
            {
                LineKey currentlineKey = GetLineKey(line);
                if (currentlineKey == lineKey)
                {
                    return line.Split(": ")[1];
                }
            }
            return string.Empty;
        }





        /// <summary>
        /// Writes a value to the file given the specific key and the data to write.
        /// </summary>
        /// <param name="lineKey">The key to write the value for.</param>
        /// <param name="data">The value to write.</param>
        /// <returns>If the operation was completed successfully.</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool WriteValue(LineKey lineKey, string data)
        {
            if(lineKey == LineKey.None || lineKey == LineKey.TimeCreated)
            {
                throw new ArgumentException("Invalid key value.");
            }

            try
            {
                var lines = File.ReadAllLines(_filePath);

                if (!KeyExistsInFile(lineKey)) // if key does not exist in file, create it
                {
                    using (StreamWriter sw = new StreamWriter(_filePath, true))
                    {
                        sw.WriteLine($"{lineKey}: {data}");
                    }
                    return true;
                }

                for (int i = 0; i < lines.Length; i++) // otherwise, find the line with the given key and change it
                {
                    LineKey filekey = GetLineKey(lines[i]);
                    if (filekey == lineKey)
                    {
                        lines[i] = $"{lineKey}: {data}";
                    }
                }
                File.WriteAllLines(_filePath, lines);
            }
            catch
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Removes a line from the file with the given key.
        /// </summary>
        /// <param name="key">The key for the line.</param>
        /// <returns>Whether a line was removed or not.</returns>
        public bool RemoveLine(LineKey key)
        {
            bool removed = false;
            try
            {
                //Save all lines into an array.
                string[] lines = File.ReadAllLines(_filePath);

                //Clear the file
                File.WriteAllText(_filePath, string.Empty);

                using (StreamWriter sw = new StreamWriter(_filePath, true))
                {
                    foreach (string line in lines)
                    {
                        if (GetLineKey(line) == key) //if the key of the line matches the key that needs to be removed, skip writing that line.
                        {
                            removed = true;
                            continue;
                        }
                        sw.WriteLine(line); // Write all lines except the one that has to be removed.
                    }
                }
            }
            catch
            {
                return false;
            }
            return removed;
        }


        /// <summary>
        /// Checks if a line with a given key exists in the file.
        /// </summary>
        /// <param name="lineKey">The key to check for.</param>
        /// <returns>Whether a given key exists in the file.</returns>
        public bool KeyExistsInFile(LineKey lineKey)
        {
            var lines = File.ReadAllLines(_filePath);

            foreach (var line in lines)
            {
                if (GetLineKey(line) == lineKey)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Extracts the key from a line of text.
        /// </summary>
        /// <param name="line">The text to extract the key from.</param>
        /// <returns>The key that was found. Returns the "None" type if no other valid key was found.</returns>
        private LineKey GetLineKey(string line)
        {
            string dataType = line.Split(": ")[0]; //gets the key of a line
            switch (dataType)
            {
                case "TotalTime":
                    return LineKey.TotalTime;
                case "TimeCreated":
                    return LineKey.TimeCreated;
                default:
                    return LineKey.None;
            }
        }


        /// <summary>
        /// Checks if a file with the given path exists and creates a new one in case it does not.
        /// </summary>
        private void CreateFileIfMissing()
        {
            if (!File.Exists(_filePath))
            {
                using(StreamWriter sw = new StreamWriter(_filePath))
                {
                    sw.WriteLine($"{LineKey.TimeCreated}: {DateTime.Now.ToString()}");
                }
            }
        }


        /// <summary>
        /// The location and name of the file to work on.
        /// </summary>
        private string _filePath;
    }
}
