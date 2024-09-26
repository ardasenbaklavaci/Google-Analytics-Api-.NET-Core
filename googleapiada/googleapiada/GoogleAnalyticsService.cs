using Google.Apis.AnalyticsData.v1beta;
using Google.Apis.AnalyticsData.v1beta.Data;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using DateRange = Google.Apis.AnalyticsData.v1beta.Data.DateRange;
using Dimension = Google.Apis.AnalyticsData.v1beta.Data.Dimension;
using Metric = Google.Apis.AnalyticsData.v1beta.Data.Metric;

public class GoogleAnalyticsService
{
    private readonly string[] Scopes = { AnalyticsReportingService.Scope.AnalyticsReadonly };
    //private readonly string _serviceAccountEmail = "your_service_account_mail"; // Replace with your service account email
    private readonly string _keyFilePath = AppContext.BaseDirectory + "key.json"; // Replace with your service account key
    private readonly string _propertyId = "properties/your_property_id"; // Replace with your Google Analytics View (Profile) ID, keep "properties/"

    public async Task GetAnalyticsDataAsync()
    {
        GoogleCredential credential;

        // Load the service account credential from the JSON file
        using (var stream = new System.IO.FileStream(_keyFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream)
                .CreateScoped(Scopes);
        }

        // Create the Google Analytics Data API service for GA4
        var analyticsService = new AnalyticsDataService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Your App Name", // Replace with your app name
        });

        // Create a request for GA4 data
        RunReportRequest reportRequest = new RunReportRequest
        {
            DateRanges = new[] { new DateRange { StartDate = "30daysAgo", EndDate = "today" } },
            Metrics = new[] { new Metric { Name = "sessions" } }, // Replace with relevant GA4 metrics
            Dimensions = new[] { new Dimension { Name = "pageTitle" } } // Replace with relevant GA4 dimensions
        };

        // Call the API using Property ID
        var request = analyticsService.Properties.RunReport(reportRequest, _propertyId);
        RunReportResponse response = await request.ExecuteAsync();

        // Define the path for the output.txt file in the project directory
        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output.txt");

        // Serialize the whole response to JSON format
        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });

        // Write the entire response to the output.txt file
        await File.WriteAllTextAsync(outputPath, jsonResponse);

        Console.WriteLine($"The whole analytics response has been written to {outputPath}");
    }
}
