using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KpblcCadLoader.Data;
using KpblcCadLoader.Infrastrucute.Enums;
using KpblcCadLoader.Repository;
using NUnit.Framework;

namespace KpblcUnitTest.AppToLoadRepTests
{
    public class AppToLoadRepositoryTest
    {
        [OneTimeSetUp]
        public void StartUp()
        {
            _loaderSettingsFileName = Path.Combine(Environment.CurrentDirectory, "LoaderSettings.xml");
        }

        [Test, Order(0)]
        public void CheckToRead()
        {
            ApplicationToLoadRepository appRep = new ApplicationToLoadRepository(_loaderSettingsFileName);
            Assert.IsNotEmpty(appRep.Applications);
            Assert.AreEqual(appRep.Applications.Count(), 2);
        }

        [Test, Order(1)]
        public void CheckForManaged()
        {
            ApplicationToLoadRepository appRep = new ApplicationToLoadRepository(_loaderSettingsFileName);
            Assert.AreEqual(appRep.Applications.Where(
                o => o.ApplicationType == ApplicationTypeEnum.Managed)
                .Count(), 1);
            ApplicationToLoad app =
                appRep.Applications.FirstOrDefault(o => o.ApplicationType == ApplicationTypeEnum.Managed);
            Assert.IsTrue(app.Subfolders);

            StringBuilder sb = new StringBuilder(); 

            foreach(var item in app.FileExtensions)
            {
                sb.Append(item + ",");
            }

            Assert.AreEqual(sb.ToString().Trim(','), ".dll,.xml");
        }

        [Test, Order(2)]
        public void CheckForArx()
        {
            ApplicationToLoadRepository appRep = new ApplicationToLoadRepository(_loaderSettingsFileName);
            Assert.AreEqual(appRep.Applications
                .Where(o => o.ApplicationType == ApplicationTypeEnum.Arx)
                .Count(), 1);
            ApplicationToLoad app =
                appRep.Applications.FirstOrDefault(o => o.ApplicationType == ApplicationTypeEnum.Arx);
            Assert.IsFalse(app.Subfolders);

            StringBuilder sb = new StringBuilder();

            foreach (var item in app.FileExtensions)
            {
                sb.Append(item + ",");
            }

            Assert.AreEqual(sb.ToString().Trim(','), ".arx");
        }

        private string _loaderSettingsFileName;
    }
}
