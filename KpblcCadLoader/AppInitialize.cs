using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.AutoCAD.Runtime;
using KpblcCadLoader.Data;
using KpblcCadLoader.Repository;

namespace KpblcCadLoader
{
    public  class AppInitialize : IExtensionApplication
    {
        public void Initialize()
        {
            string settingsFileName = Path.Combine(Path.GetDirectoryName(typeof(AppInitialize).Assembly.Location),
                "loadersettings.xml");
            if (!File.Exists(settingsFileName))
            {
                MessageBox.Show("Не найден файл настроек, работа остановлена", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            ApplicationToLoadRepository appRep = new ApplicationToLoadRepository(settingsFileName);
            SyncApplicationRepository syncRep = new SyncApplicationRepository();
            foreach (ApplicationToLoad app in appRep.Applications)
            {
                // 1. Откешировать на локальную машину (обновить то бишь)
                syncRep.SyncronizeApplication(app);
                // 2. Загрузить в зависимости от типа приложения
            }
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }
    }
}
