using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EwpApi.Helper
{
    
    public class XmlGenerator
    {
        /// <summary>
        /// Generates xml form of the given entity T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseObj"></param>
        /// <returns></returns>
        public string GenerateXml<T>(T responseObj)
        {
            XmlSerializer xser = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            var xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);

            var xmlStr = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww, settings))
                {
                    try {
                        xser.Serialize(writer, responseObj, xns);
                        xmlStr = sww.ToString();
                    }
                    catch(Exception e) {
                        Log.Error(e.Message);
                    } 
                    finally {
                        writer.Close();
                        sww.Close();
                    }
                    
                    
                }
            }
            return xmlStr;

        }
    }
}
