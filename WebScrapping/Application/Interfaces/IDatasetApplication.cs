using WebScrapping.Dto.Datasets;

namespace WebScrapping.Application.Interfaces
{
    public interface IDatasetApplication
    {
        List<DatasetDto> GetDatasets();
    }
}
