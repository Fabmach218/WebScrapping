namespace WebScrapping.Dto.Risks
{
    public class RisksDto
    {
        public List<OffshoreLeaksDto>? OffshoreLeaks { get; set; }
        public List<WorldBankDebarredFirmsDto>? WorldBankDebarredFirms { get; set; }
        public List<SanctionsDto>? OFACSanctionsList { get; set; }
    }
}
