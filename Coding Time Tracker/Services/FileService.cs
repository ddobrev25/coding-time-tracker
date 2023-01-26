using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Coding_Time_Tracker.Services
{
    public class FileService
    {
        private string _fileName;

        public FileService(string fileName)
        {
            _fileName = fileName;
        }
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
        public bool WriteFileProperty(LineProperties linePropertyToWrite, string data)
        {
            //try
            //{
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
            //}
            //catch { return false; }
            return true;
        }
        private bool PropertyExistsInFile(LineProperties lineProperty) //checks if a line with a given property exists in the file 
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
    }
}
