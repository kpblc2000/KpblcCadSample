using KpblcCadLoader.Data;
using KpblcCadLoader.Infrastrucute.Enums;
using KpblcCadLoader.Repository;
using KpblcExtensions.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpblcUnitTest.SyncAppRepTest
{

    public class SyncAppRepTest
    {

        [OneTimeSetUp]
        public void StartUp()
        {
            _syncAppRep = new SyncApplicationRepository();
            _cacheRep = new CacheRepository();
        }

        [Test, Order(0)]
        public void CheckAppWithSubfolders()
        {
            ApplicationToLoad app = new ApplicationToLoad()
            {
                ApplicationName = "test",
                LocalPath = Path.Combine(Environment.GetEnvironmentVariable("temp"), @"LocalPath\Subfolders"),
                MainModuleName = "test",
                ApplicationType = ApplicationTypeEnum.Unknown,
                ServerPath = Path.Combine(Environment.GetEnvironmentVariable("temp"), @"ServerPath\Subfolders"),
                FileExtensions = new string[]
                {
                    ".xml",
                    ".dll",
                    ".log"
                }
            };

            InitializeApplication(app, new List<string>
            {
                "file01.txt",
                "file02.dll",
                "file03.log",
                "file04.arx",
                @"SubFolder01\file05.xml",
            });

            _syncAppRep.SyncronizeApplication(app);

            Assert.IsTrue(Directory.Exists(app.LocalPath));

            List<string> serverFiles = new List<string>(
                _cacheRep.GetFilesList(app.ServerPath, app.FileExtensions, app.Subfolders
                )
                .Select(o => o.Substring(app.ServerPath.Length + 1))
                .OrderBy(o => o)
                );

            List<string> localFiles = new List<string>(
                _cacheRep.GetFilesList(app.LocalPath, app.FileExtensions, app.Subfolders)
                .Select(o => o.Substring(app.LocalPath.Length + 1))
                .OrderBy(o => o));

            Assert.AreEqual(serverFiles.Count, localFiles.Count);
        }

        [Test, Order(1)]
        public void CheckAppNoSubfolders()
        {
            ApplicationToLoad app = new ApplicationToLoad()
            {
                ApplicationName = "NoSubFolders",
                LocalPath = Path.Combine(Environment.GetEnvironmentVariable("temp"), @"LocalPath\AppWithoutSubfolders"),
                MainModuleName = "name",
                ApplicationType = ApplicationTypeEnum.Unknown,
                ServerPath = Path.Combine(Environment.GetEnvironmentVariable("temp"),
      @"ServerPath\AppWithoutSubFolders"),
                FileExtensions = new string[]
  {
      ".txt",
      ".dll",
      ".log"
  },
                Subfolders = false
            };

            InitializeApplication(app, new List<string>()
            {
                "file11.txt",
                "file12.log",
                "file13.arx",
                "file14.dll" });

            _syncAppRep.SyncronizeApplication(app);

            Assert.IsTrue(Directory.Exists(app.LocalPath));

            List<string> serverFiles = new List<string>(
                _cacheRep.GetFilesList(app.ServerPath, app.FileExtensions, app.Subfolders
                )
                .Select(o => o.Substring(app.ServerPath.Length + 1))
                .OrderBy(o => o)
                );

            List<string> localFiles = new List<string>(
                _cacheRep.GetFilesList(app.LocalPath, app.FileExtensions, app.Subfolders)
                .Select(o => o.Substring(app.LocalPath.Length + 1))
                .OrderBy(o => o));

            Assert.AreEqual(serverFiles.Count, localFiles.Count);

        }

        private void InitializeApplication(ApplicationToLoad App, IEnumerable<string> FileNames)
        {
            if (Directory.Exists(App.ServerPath))
            {
                Directory.Delete(App.ServerPath, true);
            }

            Directory.CreateDirectory(App.ServerPath);
            foreach (string file in FileNames)
            {
                string filePath = Path.Combine(App.ServerPath, file);
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }

                using (StreamWriter sWriter = new StreamWriter(filePath, true))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        sWriter.WriteLine($"String {i}");
                    }
                }
            }
        }

        private SyncApplicationRepository _syncAppRep;
        private CacheRepository _cacheRep;
    }
}
