using AngleSharp;
using Azure.Storage.Blobs;
using CsvHelper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Playwright;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using WebScrapping.Application.Interfaces;
using WebScrapping.Dto.Risks;
using WebScrapping.Utils;

namespace WebScrapping.Application.Implementations
{
    public class RisksApplication : IRisksApplication
    {

        public Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public RisksApplication(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<OffshoreLeaksDto>> GetOffshoreLeaksData(string query)
        {
            if (query.IsNullOrEmpty() || query.Length < 3) throw new Exception(Utils.Constants.Messages.QUERY_TOO_SHORT);

            try
            {

                var connectionString = _configuration["BlobStorageConnectionString"] ?? "";
                var containerName = "csvs";
                var blobName = "OffshoreLeaks.csv";

                var blobClient = new BlobClient(connectionString, containerName, blobName);

                using var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream);
                stream.Position = 0;

                using var reader = new StreamReader(stream, Encoding.UTF8);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var data = csv.GetRecords<OffshoreLeaksDto>().ToList();
                var result = data.Where(x => x.Entity.ToUpper().Contains(query.ToUpper())).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(Utils.Constants.Messages.OFFSHORE_LEAKS_ERROR);
            }

        }

        public async Task<List<WorldBankDebarredFirmsDto>> GetWorldBankDebarredFirmsData(string query)
        {
            if (query.IsNullOrEmpty() || query.Length < 3) throw new Exception(Utils.Constants.Messages.QUERY_TOO_SHORT);

            try
            {
                var url = _configuration["WorldBankDebarredFirmsUrl"] ?? "";
                using var playwright = await Playwright.CreateAsync();
                var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
                var page = await browser.NewPageAsync();

                await page.GotoAsync(url);

                await page.WaitForSelectorAsync("#k-debarred-firms .k-grid-content");

                var tbody = await page.QuerySelectorAllAsync("#k-debarred-firms .k-grid-content table tbody");

                var rows = await tbody[0].QuerySelectorAllAsync("tr");

                List<WorldBankDebarredFirmsDto> result = new List<WorldBankDebarredFirmsDto>();

                foreach (var row in rows)
                {
                    var cells = await row.QuerySelectorAllAsync("td");
                    var values = new List<string>();

                    foreach (var cell in cells)
                    {
                        var text = await cell.InnerTextAsync();
                        values.Add(text.Trim());
                    }

                    bool hasToDate = DateTime.TryParseExact(values[5], "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate);

                    var worldBankDebarredFirm = new WorldBankDebarredFirmsDto()
                    {
                        FirmName = values[0],
                        Address = values[2],
                        Country = values[3],
                        InegibilityPeriod = hasToDate ? "Defined" : values[5],
                        FromDate = DateTime.ParseExact(values[4], "dd-MMM-yyyy", CultureInfo.InvariantCulture),
                        ToDate = hasToDate ? toDate : null,
                        Grounds = values[6],
                    };

                    result.Add(worldBankDebarredFirm);

                }

                var filteredResult = result.Where(x => x.FirmName.ToUpper().Contains(query.ToUpper()) || x.Address.ToUpper().Contains(query.ToUpper()) || x.Country.ToUpper().Contains(query.ToUpper()) || x.Grounds.ToUpper().Contains(query.ToUpper())).ToList();

                return filteredResult;
            }
            catch (Exception ex)
            {
                throw new Exception(Utils.Constants.Messages.WB_DEBARRED_FIRMS_ERROR);
            }

        }

        public async Task<List<SanctionsDto>> GetOFACSanctionsData(string query)
        {
            if (query.IsNullOrEmpty() || query.Length < 3) throw new Exception(Utils.Constants.Messages.QUERY_TOO_SHORT);

            try
            {
                var url = _configuration["OFACSanctionsUrl"] ?? "";
                using var playwright = await Playwright.CreateAsync();
                var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
                var page = await browser.NewPageAsync();

                await page.GotoAsync(url);

                await page.WaitForSelectorAsync("#ctl00_MainContent_Slider1_Boundcontrol");
                await page.FillAsync("#ctl00_MainContent_Slider1_Boundcontrol", "50");
                await page.FillAsync("#ctl00_MainContent_txtLastName", query);
                await page.Keyboard.PressAsync("Enter");

                try
                {
                    await page.WaitForSelectorAsync("#gvSearchResults", new() { Timeout = 5000 });

                    var tbody = await page.QuerySelectorAllAsync("#gvSearchResults tbody");

                    var rows = await tbody[0].QuerySelectorAllAsync("tr");

                    List<SanctionsDto> result = new List<SanctionsDto>();

                    foreach (var row in rows)
                    {
                        var cells = await row.QuerySelectorAllAsync("td");
                        var values = new List<string>();

                        foreach (var cell in cells)
                        {
                            var text = await cell.InnerTextAsync();
                            values.Add(text.Trim());
                        }

                        var sanction = new SanctionsDto
                        {
                            Name = values[0],
                            Address = values[1],
                            Type = values[2],
                            Programs = values[3],
                            List = values[4],
                            Score = int.Parse(values[5]),
                        };

                        result.Add(sanction);

                    }

                    return result;
                }
                catch (Exception ex)
                {
                    return new List<SanctionsDto>();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(Utils.Constants.Messages.OFAC_SANCTIONS_ERROR);
            }

        }

        public async Task<RisksDto> GetRisksScreening(string databases, string query)
        {
            RisksDto response = new RisksDto();

            var pattern = Utils.Constants.Messages.REGEX_PATTERN_DBS;
            if(!Regex.IsMatch(databases, pattern)) throw new Exception(Utils.Constants.Messages.ERROR_PATTERN_DBS);

            var parsedIds = databases.Split(',')
                    .Select(x => int.TryParse(x, out var n) ? n : 0)
                    .ToList();

            if (!parsedIds.All(v => v >= 1 && v <= 3)) throw new Exception(Utils.Constants.Messages.ERROR_NUMBERS_DBS);

            if (query.IsNullOrEmpty() || query.Length < 3) throw new Exception(Utils.Constants.Messages.QUERY_TOO_SHORT);

            try
            {
                if (parsedIds.Contains(1))
                {
                    var offshoreLeaks = await GetOffshoreLeaksData(query);
                    response.OffshoreLeaks = offshoreLeaks;
                }

                if (parsedIds.Contains(2))
                {
                    var worldBankDebarredFirms = await GetWorldBankDebarredFirmsData(query);
                    response.WorldBankDebarredFirms = worldBankDebarredFirms;
                }

                if (parsedIds.Contains(3))
                {
                    var ofacSanctionsList = await GetOFACSanctionsData(query);
                    response.OFACSanctionsList = ofacSanctionsList;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return response;
        }

    }
}
