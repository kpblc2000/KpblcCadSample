using KpblcCadLoader.Infrastrucute.Enums;

namespace KpblcCadLoader.Data
{
    public  class ApplicationToLoad
    {
        /// <summary> Имя приложения </summary>
        public string ApplicationName { get; set; }
        /// <summary> Локальный каталог приложения </summary>
        public string LocalPath { get; set; }
        /// <summary> Имя основного модуля с расширением </summary>
        public string MainModuleName { get; set; }
        /// <summary> Тип приложения - определяет методику загрузки </summary>
        public ApplicationTypeEnum ApplicationType { get; set; }

        /// <summary> Путь, откуда надо грузить приложение </summary>
        public string ServerPath { get; set; }

        /// <summary> Расширения файлов для кеширования, разделенные ";" </summary>
        public string[] FileExtensions { get; set; }

        /// <summary> Проходить по подкаталогам </summary>
        public bool Subfolders { get; set; }
    }
}
