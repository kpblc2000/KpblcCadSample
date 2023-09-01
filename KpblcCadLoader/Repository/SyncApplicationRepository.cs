using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KpblcCadLoader.Data;
using KpblcExtensions.Repository;

namespace KpblcCadLoader.Repository
{
    public class SyncApplicationRepository
    {
        public void SyncronizeApplication(ApplicationToLoad AcadApplication)
        {
            string serverFolder = AcadApplication.ServerPath.TrimEnd(new char[] { '\\' });
            string localFolder = AcadApplication.LocalPath.TrimEnd(new char[] { '\\' });

            if (!Directory.Exists(localFolder))
            {
                Directory.CreateDirectory(localFolder);
            }

            if (!Directory.Exists(serverFolder))
            {
                return;
            }

            CacheRepository cacheRep = new CacheRepository();
            Dictionary<string, string> serverFilesDictionary = new Dictionary<string, string>();
            foreach (string fileName in cacheRep.GetFilesList(AcadApplication.ServerPath, AcadApplication.FileExtensions, AcadApplication.Subfolders))
            {
                serverFilesDictionary.Add(fileName.Substring(AcadApplication.ServerPath.Length + 1).ToLowerInvariant(),
                    fileName);
            }

            Dictionary<string, string> localFilesDictionary = new Dictionary<string, string>();
            foreach (string fileName in cacheRep.GetFilesList(AcadApplication.LocalPath,
                         AcadApplication.FileExtensions, AcadApplication.Subfolders))
            {
                localFilesDictionary.Add(fileName.Substring(AcadApplication.LocalPath.Length + 1).ToLowerInvariant(), fileName);
            }


            foreach (KeyValuePair<string, string> item in localFilesDictionary)
            {
                if (!serverFilesDictionary.ContainsKey(item.Key))
                {
                    try
                    {
                        File.Delete(item.Value);
                    }
                    catch
                    {
                    }
                }
            }

            foreach (KeyValuePair<string, string> item in serverFilesDictionary)
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
