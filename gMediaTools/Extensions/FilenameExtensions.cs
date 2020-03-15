using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Extensions
{
    public static class FileNameExtensions
    {
        public static string GetFileExtension(this string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            if (fileExtension.Length > 1 && fileExtension.StartsWith("."))
            {
                return fileExtension.Substring(1);
            }
            else
            {
                return fileExtension;
            }
        }

        public static string GetNewFileName(this string fileName, int counterPaddingDigits = 1)
        {
            // Get filename without extension
            string filenameExtensionless = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName));
            
            // Get filename extension
            string filenameExtension = fileName.GetFileExtension();

            // Check if this script already exists and create a new one
            int alreadyExistingFileCounter = 0;
            while (File.Exists(fileName))
            {
                alreadyExistingFileCounter++;
                // Pad counter with '0' characters
                string alreadyExistingFileCounterString = alreadyExistingFileCounter.ToString().PadLeft(counterPaddingDigits, '0');

                fileName = $"{filenameExtensionless}.{alreadyExistingFileCounterString}.{filenameExtension}";
            }

            return fileName;
        }
    }
}
