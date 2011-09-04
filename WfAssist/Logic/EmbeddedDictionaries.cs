using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Ionic.Zip;
using System.Text;

namespace WfAssist.Logic
{
    public static class EmbeddedDictionaries
    {
        private static T WithResourceStream<T>(string name, Func<Stream, T> f)
        {
            using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("WfAssist.{0}", name)))
            {
                return f(s);
            }
        }

        public static WfDatabase LoadSwedishDatabase()
        {
            var culture = CultureInfo.GetCultureInfo("sv-SE");
            var words = WithResourceStream(
               "Dictionaries.dsso-words-iso88591-1.46.zip",
                s =>
                    {
                        var zip = ZipFile.Read(s);
                        foreach (var entry in zip)
                        {
                            using (var ms = new MemoryStream())
                            {
                                entry.Extract(ms);
                                ms.Position = 0;
                                using (var r = new StreamReader(ms, Encoding.GetEncoding(culture.TextInfo.ANSICodePage)))
                                {
                                    var x = r.ReadToEnd();
                                    return x.Split(',').ToList();
                                }
                            }
                        }
                        return new List<string>();
                    });
                
            return new WfDatabase(words, culture);
        } 
    }
}