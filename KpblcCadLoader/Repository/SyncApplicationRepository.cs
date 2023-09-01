using KpblcCadLoader.Data;
using KpblcExtensions.Repository;
using System.Collections.Generic;
using System.IO;

namespace KpblcCadLoader.Repository
{
    public class SyncApplicationRepository
    {
        /// <summary> Синхронизирует серверный каталог приложения и его локальную копию </summary>
        /// <param name="AcadApplication"></param>
        public void SyncronizeApplication(ApplicationToLoad AcadApplication)
        {
            string serverFolder = AcadApplication.ServerPath.TrimEnd('/');
            string localFolder = AcadApplication.LocalPath.TrimEnd('/');

            if (!Directory.Exists(serverFolder))
            {
                return;
            }

            if (!Directory.Exists(localFolder))
            {
                Directory.CreateDirectory(localFolder);
            }

            CacheRepository _cacheRep = new CacheRepository();
            Dictionary<string, string> serverFilesDict = new Dictionary<string, string>();
            foreach (string fileName in _cacheRep.GetFilesList(AcadApplication.ServerPath, AcadApplication.FileExtensions, AcadApplication.Subfolders))
            {
                serverFilesDict.Add(fileName.Substring(AcadApplication.ServerPath.Length + 1).ToLowerInvariant(), fileName);
            }

            Dictionary<string, string> localFilesDict = new Dictionary<string, string>();
            foreach (string fileName in _cacheRep.GetFilesList(AcadApplication.LocalPath, AcadApplication.FileExtensions, AcadApplication.Subfolders))
            {
                localFilesDict.Add(fileName.Substring(AcadApplication.LocalPath.Length + 1).ToLowerInvariant(), fileName);
            }

            foreach (KeyValuePair<string, string> item in localFilesDict)
            {
                if (!serverFilesDict.ContainsKey(item.Key))
                {
                    try
                    {
                        File.Delete(item.Value);
                    }
                    catch { }
                }
            }

            foreach (KeyValuePair<string, string> item in serverFilesDict)
            {
                string localFileName = Path.Combine(AcadApplication.LocalPath, item.Key);
                if (!File.Exists(localFileName)
                    || File.GetLastWriteTimeUtc(localFileName) < File.GetLastWriteTimeUtc(item.Value)
                    )
                {
                    string folderName = Path.GetDirectoryName(localFileName);
                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }
                    File.Copy(item.Value, localFileName, true);
                }
            }
        }
    }
}
