using System;
using System.Configuration;
using System.IO;
using Otus.Teaching.Concurrency.Import.DataAccess.EF;
using Otus.Teaching.Concurrency.Import.Common;

namespace Otus.Teaching.Concurrency.Import.Loader
{
    public static class AppSettings
    {
        private static bool _isGenerateDataByProcess;
        private static string _generatorFullFileName;
        private static string _typeFile;
        private static string _typeDb;
        private static string _dbConnectionString;
        private static int _numData;
        private static short _numThreads;
        private static bool _isUseThreadPool;

        public static string DataFilePath { get; set; }

        public static bool IsGenerateDataByProcess { get { return _isGenerateDataByProcess; } }
        public static string GeneratorFullFileName { get { return _generatorFullFileName; } }

        public static string TypeFile { get { return _typeFile; } }
        public static string TypeDb { get { return _typeDb; } }
        public static string DbConnectionString { get { return _dbConnectionString; } }
        public static int NumData { get { return _numData; } }
        public static short NumThreads { get { return _numThreads; } }
        public static bool IsUseThreadPool { get { return _isUseThreadPool; } }

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

            _generatorFullFileName = ConfigurationManager.AppSettings["GeneratorFullFileName"];
            if (_isGenerateDataByProcess == true && !File.Exists(GeneratorFullFileName))
            {
                DisplayMessageError?.Invoke("Wrong path to xml data generator - configuration parametr \"XmlGeneratorFullFileName\".");
                return false;
            }

            _typeFile = ConfigurationManager.AppSettings["TypeFile"];
            if (_typeFile == null || (!_typeFile.Equals(TypesFiles.Xml) && !_typeFile.Equals(TypesFiles.Csv)))
            {
                DisplayMessageError?.Invoke("Invalid configuration parametr \"TypeFile\".It's must be \"xml\" or \"csv\".");
                return false;
            }

            _typeDb = ConfigurationManager.AppSettings["TypeDB"];
            if (_typeDb == null || (!_typeDb.Equals(TypesDatabases.PostgreSQL) && !_typeDb.Equals(TypesDatabases.SQLite)))
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

            if (!bool.TryParse(ConfigurationManager.AppSettings["IsUseThreadPool"], out _isUseThreadPool))
            {
                DisplayMessageError?.Invoke("Invalid configuration parametr \"IsUseThreadPool\". It's must be \"True\" or \"False\".");
                return false;
            }

            if (_numThreads > _numData)
            {
                DisplayMessageError?.Invoke($"Configuration parametr \"NumThreads\" must be lover than \"NumData\"");
                return false;
            }

            if (DataFilePath == null)
                DataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"customers.{_typeFile}");

            return true;
        }
    }
}
