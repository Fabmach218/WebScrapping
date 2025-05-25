namespace WebScrapping.Dto.Risks
{
    public class WorldBankDebarredFirmsDto
    {
        public string FirmName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string InegibilityPeriod { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Grounds { get; set; } = string.Empty;
    }
}
