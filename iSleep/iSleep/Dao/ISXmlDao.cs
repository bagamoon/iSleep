using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using System.IO.IsolatedStorage;
using System.IO;
using iSleep.Model;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace iSleep.Dao
{
    public class ISXmlDao
    {
        public void Write<T>(T model, string xmlFilePath)
        {
            Write(model, xmlFilePath, FileMode.Create);
        }

        public void Write<T>(T model, string xmlFilePath, FileMode mode)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;

            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = isolatedStorage.OpenFile(xmlFilePath, mode))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                    {
                        serializer.Serialize(xmlWriter, model);
                    }
                }
            }
        }

        public T Read<T>(string xmlFilePath)
        {
            T data = default(T);

            try
            {
                using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isolatedStorage.FileExists(xmlFilePath))
                    {
                        using (IsolatedStorageFileStream stream = isolatedStorage.OpenFile(xmlFilePath, FileMode.Open))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(T));
                            data = (T)serializer.Deserialize(stream);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                
            }

            return data;
        }

        public void Remove(string xmlFilePath)
        {
            try
            {
                using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isolatedStorage.FileExists(xmlFilePath))
                    {
                        isolatedStorage.DeleteFile(xmlFilePath);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void RemoveAll()
        {
            try
            {
                using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    isolatedStorage.Remove();
                }
            }
            catch (Exception ex)
            {

            }
        }

    }    
}
