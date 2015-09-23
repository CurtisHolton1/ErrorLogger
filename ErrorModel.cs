using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ErrorLogger
{
    public class ErrorModel
    {
        public enum SeverityEnum
        {
            [XmlEnum(Name="Warning")]
           Warning,
            [XmlEnum(Name = "Alert")]
           Alert,
            [XmlEnum(Name = "Error")]
           Error
        }

        public SeverityEnum Severity { get; set; }
        public string AppName { get; set; }
        public DateTime Time { get; set; }
        public string MethodName { get; set; }
        public string ExceptionDetails { get; set; }
        public string Message { get; set; }

        public ErrorModel(Exception e, string appName, string methodName, SeverityEnum severity, string message = "")
        {
            this.Time = DateTime.Now;
            this.Severity = severity;
            this.AppName = appName;
            this.MethodName = methodName;
            this.ExceptionDetails = GetExceptionDetails(e);
            this.Message = message;
        }
        
        private ErrorModel()
        {
            //needed for serialization
        }
        private string GetExceptionDetails(Exception exception)
        {
            if (exception == null)
            {
                return "";
            }
            try
            {
                var properties = exception.GetType().GetProperties();
                var fields = properties
                              .Select(property => new
                              {
                                  Name = property.Name,
                                  Value = property.GetValue(exception, null)
                              })
                              .Select(x => String.Format(
                                  "{0} = {1}",
                                  x.Name,
                                  x.Value != null ? x.Value.ToString() : String.Empty
                              ));
                return String.Join("\n", fields);

            }
            catch (Exception e)
            {
                
            }
            return "";
        }
    }
}
