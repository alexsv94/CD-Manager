using OrganizerWpf.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace OrganizerWpf.Utilities
{
    public static class FileSystemHelper
    {
        public static List<IFileSystemItem> GetItems<T>(string workDirectory) 
            where T : SerializableModel<T>, new()
        {
            List<IFileSystemItem> itemsList = new();

            if (!CheckDirectory(workDirectory, out DirectoryInfo? currentDir))
                return itemsList;

            var dirs = currentDir!.GetDirectories();

            foreach (var dir in dirs)
            {
                var dirInfo = new DirectoryModel();
                dirInfo.SetAllValues(dir);
                itemsList.Add(dirInfo);
            }

            var files = currentDir!.GetFiles();

            foreach (var file in files)
            {
                if (file.Attributes.HasFlag(FileAttributes.Hidden))
                    continue;

                T fileInfo = new();
                fileInfo.SetDefaultValues(file);

                T? fileMetaData = GetFileMetadata<T>(file.FullName);
                fileInfo.SetValuesFromMetadata(fileMetaData);

                itemsList.Add(fileInfo);
            }

            return itemsList;
        }

        public static SerializableModel<T>? GetFile<T>(string path)
            where T : SerializableModel<T>, new()
        {
            SerializableModel<T> item = new();

            FileInfo file = new(path);
            if (!file.Exists) return null;

            T itemInfo = new();
            itemInfo.SetDefaultValues(file);

            return itemInfo;
        }

        private static bool CheckDirectory(string path, out DirectoryInfo? checkedDir)
        {
            var dirToCheck = new DirectoryInfo(path);

            if (!dirToCheck.Exists)
            {
                if(MessageBox.Show($"Каталога\n{path}\nне существует. Создать?", 
                    "Каталог не найден",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    checkedDir = Directory.CreateDirectory(path);
                    return true;
                }
                else
                {
                    checkedDir = null;
                    return false;
                }
            }
            else
            {
                checkedDir = dirToCheck;
                return true;
            }
        }

        public static T? GetFileMetadata<T>(string filePath) 
            where T : SerializableModel<T>
        {
            FileInfo file = new FileInfo(filePath);
            DirectoryInfo? dir = file.Directory;

            FileInfo[] files = dir!.GetFiles("*.json");

            FileInfo? metaData = null;

            foreach (var metaFile in files)
            {
                string metaFileName = metaFile.Name.Replace(".meta.json", "");
                
                if (file.Name == metaFileName)
                {
                    metaData = metaFile;
                    break;
                }
            }

            if (metaData == null)
            {
                return null;
            }
            else
            {
                return SerializableModel<T>.FromJson(File.ReadAllText(metaData.FullName));
            }            
        }

        public static void SaveFileMetadata<T>(T modelInfo) 
            where T : SerializableModel<T>
        {            
            FileInfo file = new FileInfo(((IFileSystemItem)modelInfo).FullPath!);
            DirectoryInfo directoryInfo = file.Directory!;

            string metaFileName = file.Name + ".meta.json";
            string metaFilePath = Path.Combine(directoryInfo.FullName, metaFileName);

            var modelMetadata = modelInfo;
            string jsonData = modelMetadata.ToJson();

            if (File.Exists(metaFilePath))
            {
                File.Delete(metaFilePath);
            }
            File.WriteAllText(metaFilePath, jsonData);
            File.SetAttributes(metaFilePath, FileAttributes.Hidden);
        }

        public static List<string> GetDroppedFilePaths(string[] paths, bool includeFolders)
        {
            List<string> filePaths = new List<string>();

            foreach (string obj in paths)
            {
                if (Directory.Exists(obj) && !includeFolders)
                {                    
                    filePaths.AddRange(Directory.GetFiles(obj, "*.*", SearchOption.AllDirectories));
                    filePaths.Add(obj);
                }

                if (includeFolders) filePaths.Add(obj);
            }

            return filePaths;
        }

        public static void CopyItems(string workDirectory, List<string> itemsToCopy)
        {
            if (string.IsNullOrEmpty(workDirectory)) return;

            foreach (var item in itemsToCopy)
            { 
                FileInfo fileInfo = new(item);
                DirectoryInfo directoryInfo = new(item);

                if (directoryInfo.Exists)
                {
                    string destinationPath = Path.Combine(workDirectory, directoryInfo.Name);
                    CopyDirectory(directoryInfo.FullName, destinationPath);
                }
                else if (fileInfo.Exists)
                {
                    FileInfo? fileToCheck = new DirectoryInfo(workDirectory)
                                            .GetFiles()
                                            .FirstOrDefault(x => x.Name == fileInfo.Name);

                    CopyFile(fileToCheck, fileInfo, workDirectory);
                }
            }
        }

        private static void CopyFile(FileInfo? checkedFile, FileInfo fileToCopy, string workDir)
        {
            if (CheckCopyPosibility(checkedFile))                
                fileToCopy.CopyTo(Path.Combine(workDir, fileToCopy.Name));
        }

        private static void CopyDirectory(string sourceDir, string destinationDir)
        {
            if (!CheckCopyPosibility(new DirectoryInfo(destinationDir))) return;

            DirectoryInfo dir = new(sourceDir);

            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(destinationDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir);
            }
        }

        private static bool CheckCopyPosibility(FileSystemInfo? item)
        {
            if (item != null && item.Exists)
            {
                string typeOfElement = item is FileInfo ? "Файл" : "Папка";
                
                var result = MessageBox.Show($"{typeOfElement} {item.Name} уже существует. Заменить?",
                "Копирование",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

                if (result == MessageBoxResult.Yes)
                {
                    if (item is DirectoryInfo dir)
                        dir.Delete(true);
                    else
                        item.Delete();

                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }

        public static string? RenameItem(string oldPath, string newItemName)
        {
            string? newPath = null;

            if (File.Exists(oldPath))
            {
                FileInfo file = new FileInfo(oldPath);

                newPath = Path.Combine(file.Directory!.FullName, newItemName);

                try
                {
                    File.Move(oldPath, newPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Файл занят другим процессом",
                        "Ошибка переименования",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return oldPath;
                }                

                return newPath;
            }
            else if (Directory.Exists(oldPath))
            {
                DirectoryInfo dir = new DirectoryInfo(oldPath);                
                
                if (dir.Parent != null)
                {
                    newPath = Path.Combine(dir.Parent.FullName, newItemName);                    
                }
                else
                {
                    newPath = Path.Combine(dir.FullName, newItemName);                    
                }

                try
                {
                    Directory.Move(oldPath, newPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Папка занята другим процессом",
                        "Ошибка переименования",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return oldPath;
                }
                
            }

            return newPath;
        }

        public static void OpenFile(string path)
        {
            var p = new Process
            {
                StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true,
                }
            };

            p.Start();
        }
    }
}
