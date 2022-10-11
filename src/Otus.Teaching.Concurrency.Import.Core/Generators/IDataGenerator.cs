namespace Otus.Teaching.Concurrency.Import.Handler.Data
{
    public interface IDataGenerator
    {
        void Generate(string fileName, int dataCount);
    }
}