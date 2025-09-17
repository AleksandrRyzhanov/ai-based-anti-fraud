# DotNet Email Classifier API

## Overview

The DotNet Email Classifier API is a .NET-based web service that classifies email content using an AI model. It supports topic-based concern analysis and returns relevant scores and reasoning for each request.

## Project Structure

```
ai-based-anti-fraud/
├── DotNetEmailClassifierApi/
│   ├── src/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Services/
│   │   ├── Resources/
│   │   ├── Properties/
│   │   ├── run-benchmark.ps1
│   │   └── DotNetEmailClassifierApi.csproj
│   ├── benchmark/
│   └── README.md (moved to root)
├── data/
├── .github/
└── README.md
```

## Setup Instructions

1. **Clone the Repository**

   ```bash
   git clone <repository-url>
   cd ai-based-anti-fraud
   ```

2. **Restore Dependencies**

   ```bash
   cd DotNetEmailClassifierApi/src
   dotnet restore
   ```

3. **Run the Application**
   ```bash
   dotnet run
   ```
   The API will be available at the port specified in `launchSettings.json` (default: http://localhost:50082).

## Usage

### Classify Email

Send a POST request to `/api/emailclassification/classify` with JSON body:

```json
{
  "message": "Your email text here.",
  "topicid": "1",
  "model": "deepseek-r1-spamassassin"
}
```

### Response Example

```json
{
  "relevantScore": 7,
  "irrelevancyReasons": [
    "The message clearly relates to financial reporting issues and auditing failures, which is directly relevant to the topic."
  ],
  "elapsedTime": 31.59
}
```

## Benchmarking

- Use `run-benchmark.ps1` in `src` to test all topics and message types.
- Results are saved in `/benchmark` with model name and timestamp for comparison.

## Dependencies

- .NET 6.0 or later
- See `DotNetEmailClassifierApi.csproj` for details.

## Contributing

Contributions are welcome! Please submit a pull request or open an issue for enhancements or bug fixes.

## License

MIT License. See LICENSE file for details.
