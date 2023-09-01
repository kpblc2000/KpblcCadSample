using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using KpblcCadLoader.Data;
using KpblcCadLoader.Infrastrucute.Enums;

namespace KpblcCadLoader.Repository
{
    public class ApplicationToLoadRepository
    {
        public ApplicationToLoadRepository(string LoaderSettingsXmlFile)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Root));
            Root root;
            using (FileStream fileStream = new FileStream(LoaderSettingsXmlFile, FileMode.Open))
            {
                root = xmlSerializer.Deserialize(fileStream) as Root;
            }

            Applications = root.Applications.Select(o =>
                {
                    var temp = o;
                    ApplicationToLoad entity = new ApplicationToLoad()
                    {
                        ApplicationName = o.Name,
                        MainModuleName = o.AppName,
                    };
                    if (o.AppType.Equals(ApplicationTypeEnum.Managed.ToString(),
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        entity.ApplicationType = ApplicationTypeEnum.Managed;
                    }
                    else if (o.AppType.Equals(ApplicationTypeEnum.Arx.ToString(),
                                 StringComparison.InvariantCultureIgnoreCase))
                    {
                        entity.ApplicationType = ApplicationTypeEnum.Arx;
                    }
                    else if (o.AppType.Equals(ApplicationTypeEnum.Lsp.ToString(),
                                 StringComparison.InvariantCultureIgnoreCase))
                    {
                        entity.ApplicationType = ApplicationTypeEnum.Lsp;
                    }

                    entity.ServerPath = o.Path.OrderByDescending(p => p.Priority)
                        .FirstOrDefault(p => Directory.Exists(p.Value))?.Value ?? string.Empty;

                    entity.LocalPath = Path.Combine(Environment.GetEnvironmentVariable("appdata"), o.LocalPath);

                entity.FileExtensions = o.FileExtensions.Split(new char[] { ',', ';' });

                    if (bool.TryParse(o.Subfolders, out bool subfolders))
                    {
                        entity.Subfolders = subfolders;
                    }

                return entity;
            })
                .Where(o =>
                    !string.IsNullOrEmpty(o.ServerPath)
                    && o.ApplicationType != ApplicationTypeEnum.Unknown
                    && !o.LocalPath.Equals(Environment.GetEnvironmentVariable("appdata"), StringComparison.InvariantCultureIgnoreCase)
                    );
        }

        #region  Сериализация


        // Примечание. Для запуска созданного кода может потребоваться NET Framework версии 4.5 или более поздней версии и .NET Core или Standard версии 2.0 или более поздней.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Root
        {

            private RootPath[] loaderPathsField;

            private RootItem[] applicationsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Path", IsNullable = false)]
            public RootPath[] LoaderPaths
            {
                get
                {
                    return this.loaderPathsField;
                }
                set
                {
                    this.loaderPathsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
            public RootItem[] Applications
            {
                get
                {
                    return this.applicationsField;
                }
                set
                {
                    this.applicationsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RootPath
        {

            private string valueField;

            private sbyte priorityField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public sbyte Priority
            {
                get
                {
                    return this.priorityField;
                }
                set
                {
                    this.priorityField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RootItem
        {

            private RootItemPath[] pathField;

            private string nameField;

            private string appTypeField;

            private string localPathField;

            private string appNameField;

            private string fileExtensionsField;

            private string subfoldersField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Path")]
            public RootItemPath[] Path
            {
                get
                {
                    return this.pathField;
                }
                set
                {
                    this.pathField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AppType
            {
                get
                {
                    return this.appTypeField;
                }
                set
                {
                    this.appTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string LocalPath
            {
                get
                {
                    return this.localPathField;
                }
                set
                {
                    this.localPathField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string AppName
            {
                get
                {
                    return this.appNameField;
                }
                set
                {
                    this.appNameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string FileExtensions
            {
                get
                {
                    return this.fileExtensionsField;
                }
                set
                {
                    this.fileExtensionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Subfolders
            {
                get
                {
                    return this.subfoldersField;
                }
                set
                {
                    this.subfoldersField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RootItemPath
        {

            private string valueField;

            private byte priorityField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte Priority
            {
                get
                {
                    return this.priorityField;
                }
                set
                {
                    this.priorityField = value;
                }
            }
        }



        #endregion

        public IEnumerable<ApplicationToLoad> Applications { get; private set; }
    }
}
