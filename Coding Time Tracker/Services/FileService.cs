using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Coding_Time_Tracker.Services
{
    /// <summary>
    /// Service that operates with files which store the data of the application.
    /// </summary>
    public class FileService
    {
        /// <summary>
        /// Initiator
        /// </summary>
        /// <param name="fileName">The name of the file to work on.</param>
        public FileService(string fileName)
        {
            _fileName = fileName;
        }


        /// <summary>
        /// Reads a value from the file given a specific key (line property).
        /// </summary>
        /// <param name="linePropertyToRead">The line property (key) to find the value for.</param>
        /// <returns>The value that matches the given key.</returns>
        public string ReadLineProperty(LineProperties linePropertyToRead)
        {
            var lines = File.ReadAllLines(_fileName);

            foreach (var line in lines)
            {
                LineProperties lineProperty = GetLineProperty(line);
                if(lineProperty == linePropertyToRead)
                {
                    return line.Split(": ")[1];
                }
            }
            return string.Empty;
        }



        /// <summary>
        /// Writes a value to the file given the specific key (line property) and the data to write.
        /// </summary>
        /// <param name="linePropertyToWrite">The line property (key) to write the value for.</param>
        /// <param name="data">The value to write.</param>
        /// <returns>If the operation was completed successfully.</returns>
        public bool WriteLineProperty(LineProperties linePropertyToWrite, string data)
        {
            try
            {
            if (!File.Exists(_fileName))
            {
                using (StreamWriter sw = new StreamWriter(_fileName))
                {
                    sw.WriteLine($"Time created: {DateTime.Now}");
                }
            }
            var lines = File.ReadAllLines(_fileName);

            if (!PropertyExistsInFile(linePropertyToWrite)) // if property does not exist in file, create it
            {
                using (StreamWriter sw = new StreamWriter(_fileName, true))
                {
                    sw.WriteLine($"{linePropertyToWrite}: {data}");
                }
                return true;
            }

            for (int i = 0; i < lines.Length; i++) // otherwise, find the line with the given property and change it
            {
                LineProperties fileProperty = GetLineProperty(lines[i]);
                if (fileProperty == linePropertyToWrite)
                {
                    lines[i] = $"{linePropertyToWrite}: {data}";
                }
            }
            File.WriteAllLines(_fileName, lines);
            }
            catch { return false; }
            return true;
        }



        /// <summary>
        /// checks if a line with a given property exists in the file.
        /// </summary>
        /// <param name="lineProperty">The key (line property) to check for.</param>
        /// <returns>Whether a given key (line property) exists in the file.</returns>
        private bool PropertyExistsInFile(LineProperties lineProperty)
        {
            var lines = File.ReadAllLines(_fileName);

            foreach (var line in lines)
            {
                if (GetLineProperty(line) == lineProperty)
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
        private LineProperties GetLineProperty(string line)
        {
            string dataType = line.Split(": ")[0]; //gets the property of a line
            switch (dataType)
            {
                case "TotalTime":
                    return LineProperties.TotalTime;
                default:
                    return LineProperties.None;
            }
        }

        private string _fileName;
    }
}
