using Midnight_Commander.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Midnight_Commander.Services
{
    public class DriveService
    {
        public static double Total { get; set; }

        public static DirectoryInfo[] Directories(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            try
            {
                return dirInfo.GetDirectories();
            }
            catch
            {
                return dirInfo.Parent.GetDirectories();
            }
        }
        public static FileInfo[] Files(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            try
            {
                return dirInfo.GetFiles();
            }
            catch
            {
                return dirInfo.Parent.GetFiles();
            }
        }
        public static long Drive(string drive)
        {
            DriveInfo drinfo = new DriveInfo(drive);
            Total = ((drinfo.AvailableFreeSpace / 1048576D) / (drinfo.TotalSize / 1048576D))*100;
            return drinfo.AvailableFreeSpace/1073741824;
        }
        public static DriveInfo[] Drives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            return drives;
        }
        
    }
}
