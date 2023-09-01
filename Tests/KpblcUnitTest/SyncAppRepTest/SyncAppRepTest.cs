using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KpblcCadLoader.Data;
using KpblcCadLoader.Infrastrucute.Enums;
using KpblcCadLoader.Repository;
using KpblcExtensions.Repository;
using NUnit.Framework;

namespace KpblcUnitTest.SyncAppRepTest
{
    public class SyncAppRepTest
    {
        [OneTimeSetUp]
        public void StartUp()
        {
            _appWithSubFolders = new ApplicationToLoad
            {
                ApplicationName = "SubFolders",
                LocalPath = Path.Combine(Environment.GetEnvironmentVariable("temp"), @"LocalPath\AppWithSubfolders"),
                MainModuleName = "name",
                ApplicationType = ApplicationTypeEnum.Unknown,
                ServerPath = Path.Combine(Environment.GetEnvironmentVariable("temp"), @"ServerPath\AppWithSubFolders"),
                FileExtensions = new string[]
                {
                    ".txt",
                    ".dll",
                    ".log"
                },
                Subfolders = true
            };

            InitializeApp(_appWithSubFolders, new List<string>()
                {
                    "file1.txt",
                    "file2.log",
                    "file1234.arx",
                    "asdfwert.xml",
                    @"SubFolder\file3.dll"
                }
            );

            _appWithoutSubFolders = new ApplicationToLoad
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
            InitializeApp(_appWithoutSubFolders, new List<string>()
            {
                "file01.txt",
                "file02.log",
                "file03.arx",
                "file04.dll"
            });

            _syncRep = new SyncApplicationRepository();
            _cacheRep = new CacheRepository();
        }

        [Test, Order(0)]
        public void CheckIni()
        {
            Assert.IsTrue(Directory.Exists(_appWithSubFolders.ServerPath));
            Assert.IsTrue(Directory.Exists(_appWithoutSubFolders.ServerPath));
        }

        [Test, Order(1)]
        public void CheckAppWithSubFolders()
        {
            _syncRep.SyncronizeApplication(_appWithSubFolders);

            Assert.IsTrue(Directory.Exists(_appWithSubFolders.LocalPath));
            List<string> serverFiles = new List<string>(
                _cacheRep.GetFilesList(_appWithSubFolders.ServerPath, _appWithSubFolders.FileExtensions,
                    _appWithSubFolders.Subfolders)
                .Select(o => o.Substring(_appWithSubFolders.ServerPath.Length)
                    )
                .OrderBy(o => o)
                    );
            List<string> localFiles = new List<string>(
                _cacheRep.GetFilesList(_appWithSubFolders.LocalPath, _appWithSubFolders.FileExtensions,
                        _appWithSubFolders.Subfolders)
                    .Select(o => o.Substring(_appWithSubFolders.LocalPath.Length))
                    .OrderBy(o => o));
            Assert.AreEqual(serverFiles.Count, localFiles.Count);
        }

        [Test, Order(2)]
        public void CheckAppWithoutSubFolders()
        {
            _syncRep.SyncronizeApplication(_appWithoutSubFolders);

            Assert.IsTrue(Directory.Exists(_appWithoutSubFolders.LocalPath));
            List<string> serverFiles = new List<string>(
                _cacheRep.GetFilesList(_appWithoutSubFolders.ServerPath, _appWithoutSubFolders.FileExtensions,
                        _appWithoutSubFolders.Subfolders)
                    .Select(o => o.Substring(_appWithoutSubFolders.ServerPath.Length)
                    )
                    .OrderBy(o => o)
            );
            List<string> localFiles = new List<string>(
                _cacheRep.GetFilesList(_appWithoutSubFolders.LocalPath, _appWithoutSubFolders.FileExtensions,
                        _appWithoutSubFolders.Subfolders)
                    .Select(o => o.Substring(_appWithoutSubFolders.LocalPath.Length))
                    .OrderBy(o => o));
            Assert.AreEqual(serverFiles.Count, localFiles.Count);
        }

        [OneTimeTearDown]
        public void ClearAll()
        {
            if (Directory.Exists(_appWithoutSubFolders.LocalPath))
            {
                Directory.Delete(_appWithoutSubFolders.LocalPath, true);
            }

            if (Directory.Exists(_appWithoutSubFolders.ServerPath))
            {
                Directory.Delete(_appWithoutSubFolders.ServerPath, true);
            }

            if (Directory.Exists(_appWithSubFolders.LocalPath))
            {
                Directory.Delete(_appWithSubFolders.LocalPath, true);
            }

            if (Directory.Exists(_appWithSubFolders.ServerPath))
            {
                Directory.Delete(_appWithSubFolders.ServerPath, true);
            }
        }

        private void InitializeApp(ApplicationToLoad App, IEnumerable<string> FileNames)
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

                using (StreamWriter sWriter = new StreamWriter(filePath, false))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        sWriter.WriteLine($"String {i}");
                    }
                }
            }
        }

        private ApplicationToLoad _appWithSubFolders;
        private ApplicationToLoad _appWithoutSubFolders;
        private SyncApplicationRepository _syncRep;
        private CacheRepository _cacheRep;

        /*
        [OneTimeSetUp]
        public void StartUp()
        {
            _settingsFileName = Path.Combine(Environment.CurrentDirectory, "LoaderSettings.xml");
            _appRep = new ApplicationToLoadRepository(_settingsFileName);
        }

        [Test, Order(0)]
        public void UpdateManagedApp()
        {
            SyncApplicationRepository syncRep = new SyncApplicationRepository();
            ApplicationToLoad app =
                _appRep.Applications.FirstOrDefault(o => o.ApplicationType == ApplicationTypeEnum.Managed);
            syncRep.SyncronizeApplication(app);

            Assert.IsTrue(Directory.Exists(app.LocalPath));

        }

        private string _settingsFileName;
        private ApplicationToLoadRepository _appRep;
        */
    }
}
