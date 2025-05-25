using WebScrapping.Application.Interfaces;
using WebScrapping.Dto.Datasets;

namespace WebScrapping.Application.Implementations
{
    public class DatasetApplication : IDatasetApplication
    {
        public List<DatasetDto> GetDatasets()
        {
            List<DatasetDto> datasets = new()
            {
                new DatasetDto
                {
                    Id = 1,
                    Name = "Offshore Leaks Database"
                },
                new DatasetDto
                {
                    Id = 2,
                    Name = "World Bank Debarred Firms"
                },
                new DatasetDto
                {
                    Id = 3,
                    Name = "OFAC Sanctions List"
                },
            };

            return datasets;
        }
    }
}
