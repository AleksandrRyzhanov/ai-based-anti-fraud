using System.IO;
using System.Threading.Tasks;

namespace DotNetEmailClassifierApi.Services
{
    public class SchemaProvider
    {
        private readonly string _schemaPath;

        public SchemaProvider(string schemaPath)
        {
            _schemaPath = schemaPath;
        }

        public async Task<string> GetSchemaJsonAsync()
        {
            System.Console.WriteLine($"Trying to load schema from: {_schemaPath}");
            if (!File.Exists(_schemaPath))
                throw new FileNotFoundException("Schema file not found.", _schemaPath);

            return await File.ReadAllTextAsync(_schemaPath);
        }
    }
}