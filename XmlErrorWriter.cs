using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using ErrorLogger;

namespace ErrorLogger
{
     
    public static class XmlErrorWriter
    {   
        private static List<ErrorModel> _errors;
        private static string SaveLocation { get; set; }

        static XmlErrorWriter()
        {
            _errors = new List<ErrorModel>();
        }
        public static string AddError(ErrorModel model, string saveLocation = "Errors.xml")
        {
            SaveLocation = saveLocation;
            _errors = Deserialize();
            _errors.Add(model);     
            SerializeAndSave();    

            return SaveLocation;
        }
        private static void SerializeAndSave()
        {
            try
            {
                    var serializer = new XmlSerializer(typeof(List<ErrorModel>));
                    TextWriter writer = new StreamWriter(SaveLocation);
                    serializer.Serialize(writer,_errors);
                    writer.Close();
                    writer.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static List<ErrorModel> Deserialize(string saveLocation = "Errors.xml")
        {
            SaveLocation = saveLocation;
            SaveLocation = HttpContext.Current.Server.MapPath(saveLocation);

            var deserialized = new List<ErrorModel>();
            try
            {
                var serializer = new XmlSerializer(typeof(List<ErrorModel>));
                var reader = new StreamReader(SaveLocation);
                deserialized = (List<ErrorModel>)serializer.Deserialize(reader);
                reader.Close();

                if (deserialized == null)
                    deserialized = new List<ErrorModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return deserialized;

        }
    
    }
}
