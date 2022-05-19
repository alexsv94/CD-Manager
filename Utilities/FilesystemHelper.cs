using OrganizerWpf.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OrganizerWpf.Utilities
{
    public class FilesystemHelper
    {
        public List<T> GetFiles<T>(string workDirectory) 
            where T : SerializableModel<T>, new()
        {
            List<T> filesList = new();

            if (!CheckDirectory(workDirectory, out DirectoryInfo? currentDir))
                return filesList;

            var files = currentDir!.GetFiles();            

            foreach (var file in files)
            {
                if (file.Attributes.HasFlag(FileAttributes.Hidden))
                    continue;

                T fileInfo = new();
                fileInfo.SetDefaultValues(file);

                T? fileMetaData = GetFileMetadata<T>(file.FullName);
                fileInfo.SetValuesFromMetadata(fileMetaData);

                filesList.Add(fileInfo);
            }

            return filesList;
        }

        private bool CheckDirectory(string path, out DirectoryInfo? checkedDir)
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

        private T? GetFileMetadata<T>(string filePath) where T : SerializableModel<T>
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

        public void SetFileMetadata<T>(T modelInfo) where T : SerializableModel<T>
        {            
            FileInfo file = new FileInfo(((IFileInfo)modelInfo).FilePath!);
            DirectoryInfo directoryInfo = file.Directory!;

            string metaFileName = file.Name + ".meta.json";
            string metaFilePath = Path.Combine(directoryInfo.FullName, metaFileName);

            var modelMetadata = modelInfo;
            string jsonData = SerializableModel<T>.ToJson(modelMetadata);

            if (File.Exists(metaFilePath))
            {
                File.Delete(metaFilePath);
            }
            File.WriteAllText(metaFilePath, jsonData);
            File.SetAttributes(metaFilePath, FileAttributes.Hidden);
        }

        public List<string> GetDroppedFiles(string[] paths)
        {
            List<string> filePaths = new List<string>();

            foreach (string obj in paths)
            {
                if (Directory.Exists(obj))
                {
                    filePaths.AddRange(Directory.GetFiles(obj, "*.*", SearchOption.AllDirectories));
                }
                else
                {
                    filePaths.Add(obj);
                }
            }

            return filePaths;
        }

        public void CopyFiles(string workDirectory, List<string> filesToCopy)
        {
            if (string.IsNullOrEmpty(workDirectory)) return;

            foreach (var file in filesToCopy)
            {
                FileInfo fileInfo = new FileInfo(file);
                FileInfo? candidate = new DirectoryInfo(workDirectory)
                                            .GetFiles()
                                            .FirstOrDefault(x => x.Name == fileInfo.Name);

                if (candidate == null)
                {
                    File.Copy(fileInfo.FullName!, Path.Combine(workDirectory, fileInfo.Name));
                }

                if (candidate != null &&
                    MessageBox.Show($"Файл {fileInfo.Name} уже есть в рабочей папке. Заменить?",
                    "Копирование файла",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    File.Delete(candidate.FullName);
                    File.Copy(fileInfo.FullName, candidate.FullName);
                }
            }
        }

        public string RenameFile(string oldPath, string newFileName)
        {
            FileInfo file = new FileInfo(oldPath);
            string newPath = Path.Combine(file.Directory!.FullName, newFileName);

            File.Move(oldPath, newPath);

            return newPath;
        }
        public void OpenFile(string path)
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
