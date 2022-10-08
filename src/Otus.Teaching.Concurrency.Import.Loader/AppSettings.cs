using System;
using System.Configuration;
using System.IO;
using Otus.Teaching.Concurrency.Import.DataAccess.EF;

namespace Otus.Teaching.Concurrency.Import.Loader
{
    public static class AppSettings
    {
        private static bool _isGenerateDataByProcess;
        private static string _xmlGeneratorFullFileName;
        private static string _typeDb;
        private static string _dbConnectionString;
        private static int _numData;
        private static short _numThreads;

        public static string DataFilePath { get; set; }

        public static bool IsGenerateDataByProcess { get { return _isGenerateDataByProcess; } }
        public static string XmlGeneratorFullFileName { get { return _xmlGeneratorFullFileName; } }
        public static string TypeDb { get { return _typeDb; } }
        public static string DbConnectionString { get { return _dbConnectionString; } }
        public static int NumData { get { return _numData; } }
        public static short NumThreads { get { return _numThreads; } }


        /// <summary>
        /// Показать сообщения об ошибке при инициализации
        /// </summary>
        public static event Action<string> DisplayMessageError;

        /// <summary>
        /// Инициализация приложения
        /// </summary>
        /// <returns>True - инициализация выполнена</returns>
        public static bool Init()
        {
            if (!bool.TryParse(ConfigurationManager.AppSettings["IsGenerateDataByProcess"], out _isGenerateDataByProcess))
            {
                DisplayMessageError?.Invoke("Invalid configuration parametr \"IsGenerateDataByProcess\". It's must be \"True\" or \"False\".");
                return false;
            }

            _xmlGeneratorFullFileName = ConfigurationManager.AppSettings["XmlGeneratorFullFileName"];
            if (_isGenerateDataByProcess == true && !File.Exists(XmlGeneratorFullFileName))
            {
                DisplayMessageError?.Invoke("Wrong path to xml data generator - configuration parametr \"XmlGeneratorFullFileName\".");
                return false;
            }

            _typeDb = ConfigurationManager.AppSettings["TypeDB"];
            if (_typeDb == null || (!_typeDb.Equals(TypeDataBase.PostgreSQL) && !_typeDb.Equals(TypeDataBase.SQLite)))
            {
                DisplayMessageError?.Invoke("Invalid configuration parametr \"TypeDataBase\".It's must be \"PostgreSQL\" or \"SQLite\".");
                return false;
            }

            _dbConnectionString = ConfigurationManager.AppSettings["DBConnectionString"];
            if (_dbConnectionString == null || _dbConnectionString.Length == 0)
            {
                DisplayMessageError?.Invoke("Invalid configuration parametr \"DBConnectionString\".It's can't be empty");
                return false;
            }

            if (!int.TryParse(ConfigurationManager.AppSettings["NumData"], out _numData) || _numData <= 0)
            {
                DisplayMessageError?.Invoke($"Invalid configuration parametr \"NumData\". It's must be from \"1\" to \"{int.MaxValue}\"");
                return false;
            }

            if (!short.TryParse(ConfigurationManager.AppSettings["NumThreads"], out _numThreads) || _numThreads < 0 || _numThreads > 1000)
            {
                DisplayMessageError?.Invoke($"Invalid configuration parametr \"NumThreads\". It's must be from \"0\" to \"1000\"");
                return false;
            }

            if(_numThreads > _numData)
            {
                DisplayMessageError?.Invoke($"Configuration parametr \"NumThreads\" must be lover than \"NumData\"");
                return false;
            }

            if (DataFilePath == null)
                DataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "customers.xml");

            return true;
        }
    }
}
