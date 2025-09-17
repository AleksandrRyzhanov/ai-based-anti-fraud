using System.IO;
using System.Threading.Tasks;
using DotNetEmailClassifierApi.Models;
using Newtonsoft.Json;

namespace DotNetEmailClassifierApi.Services
{
    public class IssueTypesProvider
    {
        private readonly string _issueTypesPath;

        public IssueTypesProvider(string issueTypesPath)
        {
            _issueTypesPath = issueTypesPath;
        }

        public async Task<IssueTypes> GetIssueTypesJsonAsync()
        {
            System.Console.WriteLine($"Trying to load issue types from: {_issueTypesPath}");
            if (!File.Exists(_issueTypesPath))
                throw new FileNotFoundException("Issue types file not found.", _issueTypesPath);

            var content = await File.ReadAllTextAsync(_issueTypesPath);

            return JsonConvert.DeserializeObject<IssueTypes>(content) ?? new IssueTypes();
        }
    }
}