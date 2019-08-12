using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace VSEditorBackgroud
{
    internal static class ConfigConsts
    {
        public static readonly string ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Visual Studio 2010\\Settings", "VSEditorBackgroud.config");
        private static Encoding nobomutf8;

        public static Encoding NoBomUTF8
        {
            get
            {
                if (nobomutf8 == null)
                {
                    Interlocked.CompareExchange<Encoding>(ref nobomutf8, new UTF8Encoding(false), null);
                }
                return nobomutf8;
            }
        }

        public static XmlSerializerNamespaces VoidNamespaceMapping
        {
            get
            {
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                return namespaces;
            }
        }
    }

}
