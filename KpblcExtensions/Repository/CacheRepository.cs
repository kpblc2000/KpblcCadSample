using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KpblcExtensions.Repository
{
    public class CacheRepository
    {
        /// <summary> Синхронизирует каталоги </summary>
        /// <param name="SourceFolderName">Каталог-источник</param>
        /// <param name="DestinationFolderName">Каталог-получатель</param>
        /// <param name="FileExtensionsToSync">Список расширений файлов, которые надо синхронизировать</param>
        /// <param name="EraseLocalNotFoundOnSource">Удалять локальные файлы, если их нет в источнике</param>
        public void SyncronizeFolders(string SourceFolderName, string DestinationFolderName,
            string[] FileExtensionsToSync, bool EraseLocalNotFoundOnSource=true)
        {
            if (!Directory.Exists(SourceFolderName) && !Directory.Exists(DestinationFolderName))
            {
                throw new DirectoryNotFoundException(
                    $"Нет каталога-источника {SourceFolderName} и нет каталога-получателя {DestinationFolderName}");
            }

            if (!Directory.Exists(SourceFolderName) && Directory.Exists(DestinationFolderName))
            {
                return;
            }

            if (Directory.Exists(SourceFolderName) && !Directory.Exists(DestinationFolderName))
            {
                Directory.CreateDirectory(DestinationFolderName);
                foreach (string fileName in GetFilesList(SourceFolderName, FileExtensionsToSync))
                {
                    File.Copy(fileName, Path.Combine(DestinationFolderName, Path.GetFileName(fileName)));
                }
                return;
            }

            Dictionary<string, string> sourceFileNamesDictionary = new Dictionary<string, string>();
            foreach (string item in GetFilesList(SourceFolderName, FileExtensionsToSync))
            {
                sourceFileNamesDictionary.Add(Path.GetFileName(item).ToUpper(), item);
            }
            Dictionary<string, string> destFileNamesDictionary = new Dictionary<string, string>();
            foreach (string item in GetFilesList(DestinationFolderName, FileExtensionsToSync).ToList())
            {
                destFileNamesDictionary.Add(Path.GetFileName(item).ToUpper(), item);
            }

            if (EraseLocalNotFoundOnSource)
            {
                foreach (KeyValuePair<string, string> pair in destFileNamesDictionary)
                {
                    if (!sourceFileNamesDictionary.ContainsKey(pair.Key))
                    {
                        File.Delete(pair.Value);
                    }
                }
            }

            foreach (KeyValuePair<string, string> item in sourceFileNamesDictionary)
            {
                if (!destFileNamesDictionary.ContainsKey(item.Key))
                {
                    File.Copy(item.Value, Path.Combine(DestinationFolderName, Path.GetFileName(item.Value)));
                }
                else
                {
                    CopyFileOnDemand(item.Value, destFileNamesDictionary[item.Key]);
                }
            }
        }

        /// <summary> Копирует или обновляет файл </summary>
        /// <param name="SourceFileName">Полный путь исходного файла</param>
        /// <param name="DestinationFileName">Полный путь куда копировать</param>
        /// <returns>Имя скопированого файла либо пустую строку, если копирование не удалось</returns>
        public string CopyFileOnDemand(string SourceFileName, string DestinationFileName)
        {
            if (!File.Exists(SourceFileName) && !File.Exists(DestinationFileName))
            {
                throw new FileNotFoundException($"Не найден {SourceFileName} и нет {DestinationFileName}");
            }

            if (!File.Exists(SourceFileName) && File.Exists(DestinationFileName))
            {
                return DestinationFileName;
            }

            if (File.Exists(SourceFileName) && !File.Exists(DestinationFileName))
            {
                string destFolder = Path.GetDirectoryName(DestinationFileName);
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }
                File.Copy(SourceFileName, DestinationFileName);
                return DestinationFileName;
            }

            if (File.GetLastWriteTimeUtc(SourceFileName) > File.GetLastWriteTimeUtc(DestinationFileName))
            {
                File.Copy(SourceFileName, DestinationFileName, true);
                return SourceFileName;
            }

            return SourceFileName;
        }

        private IEnumerable<string> GetFilesList(string FolderName, string[] FileExtensions)
        {
            string[] lowerExtensions = FileExtensions.Select(o => o.ToLowerInvariant()).ToArray();
            return Directory.EnumerateFiles(FolderName, "*.*", SearchOption.TopDirectoryOnly)
                .Where(o => lowerExtensions.Contains(Path.GetExtension(o).ToLowerInvariant()));
        }
    }
}
