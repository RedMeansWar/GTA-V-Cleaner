using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GTA_V_Cleaner
{
    internal class Program
    {
        #region Variables
        protected static readonly string EXE_NAME = "GTA V Cleaner.exe";
        protected static int itemsMoved, folderSkipped;

        // List of all default GTA V files
        protected static readonly List<string> FILES_TO_SKIP = new()
        {
            "bink2w64.dll", "common.rpf", "d3dcompiler_46.dll", "d3dcsx_46.dll", "GFSDK_ShadowLib.win64.dll", "GFSDK_TXAA.win64.dll", "GFSDK_TXAA_AlphaResolve.win64.dll",
            "GPUPerfAPIDX11-x64.dll", "GTA5.exe", "GTAVLanguageSelect.exe", "GTAVLauncher.exe", "NvPmApi.Core.win64.dll", "PlayGTAV.exe", "steam_api64.dll", "version.txt",
            "x64a.rpf", "x64b.rpf", "x64c.rpf", "x64d.rpf", "x64e.rpf", "x64f.rpf", "x64g.rpf", "x64h.rpf", "x64i.rpf", "x64j.rpf", "x64k.rpf", "x64l.rpf", "x64m.rpf", "x64n.rpf",
            "x64o.rpf", "x64p.rpf", "x64q.rpf", "x64r.rpf", "x64s.rpf", "x64t.rpf", "x64u.rpf", "x64v.rpf", "x64w.rpf", "installscript.vdf"
        };

        // List of all default GTA V folders
        protected static readonly List<string> FOLDERS_TO_SKIP = new()
        {
            "Installers", "Redistributables", "update", "x64", "modded"
        };
        #endregion

        #region Constructor
        static void Main()
        {
            // EXE must be in GTA V directory to use it correctly.
            string gtaDirectoryPath = Directory.GetCurrentDirectory();

            // Create the "modded" folder inside the GTA 5 directory if it doesn't exist
            string moddedDirectoryPath = Path.Combine(gtaDirectoryPath, "Modded");
            if (!Directory.Exists(moddedDirectoryPath))
            {
                Directory.CreateDirectory(moddedDirectoryPath);
            }

            // Get all files and folders in the main GTA 5 directory (not in subdirectories)
            string[] allItems = Directory.GetFileSystemEntries(gtaDirectoryPath, "*");

            // Initialize counters for the number of items moved and folders skipped
            int itemsMoved = 0;
            int foldersSkipped = 0;

            // Identify items that are not in the skip lists
            IEnumerable<string> itemsToMove = allItems
                .Where(item =>
                    !FILES_TO_SKIP.Any(skipFile => string.Equals(Path.GetFileName(item), skipFile, StringComparison.OrdinalIgnoreCase))
                    && !FOLDERS_TO_SKIP.Any(skipFolder => string.Equals(Path.GetFileName(item), skipFolder, StringComparison.OrdinalIgnoreCase))
                    && !string.Equals(Path.GetFileName(item), EXE_NAME, StringComparison.OrdinalIgnoreCase));

            // Move the identified items to the "modded" directory
            foreach (var item in itemsToMove)
            {
                try
                {
                    if (File.Exists(item))
                    {
                        // File
                        string destinationPath = Path.Combine(moddedDirectoryPath, Path.GetFileName(item));
                        File.Move(item, destinationPath);
                        itemsMoved++;
                    }
                    else if (Directory.Exists(item))
                    {
                        // Folder
                        string destinationPath = Path.Combine(moddedDirectoryPath, Path.GetFileName(item));
                        Directory.Move(item, destinationPath);
                        itemsMoved++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error moving {item}: {ex.Message}");
                    foldersSkipped++; // Increment the foldersSkipped counter in case of an error
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cleanup complete.");
            Console.ResetColor();
            
            Console.WriteLine($"{itemsMoved} item(s) moved to \"Modded\". {foldersSkipped} folder(s) skipped. Press any key to exit...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            Environment.Exit(0); // Exit the application
        }
        #endregion
    }
}
