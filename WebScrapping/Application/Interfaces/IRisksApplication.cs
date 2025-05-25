using WebScrapping.Dto.Risks;

namespace WebScrapping.Application.Interfaces
{
    public interface IRisksApplication
    {
        Task<List<OffshoreLeaksDto>> GetOffshoreLeaksData(string query);
        Task<List<WorldBankDebarredFirmsDto>> GetWorldBankDebarredFirmsData(string query);
        Task<List<SanctionsDto>> GetOFACSanctionsData(string query);
        Task<RisksDto> GetRisksScreening(string databases, string query);
    }
}
