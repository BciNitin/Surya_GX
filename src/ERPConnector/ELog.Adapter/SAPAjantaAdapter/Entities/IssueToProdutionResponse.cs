namespace ELog.Adapter.SAPAjantaAdapter.Entities
{
    public class ProductionResponse
    {
        public string MaterialIssueNoteNo { get; set; }
    }

    public class IssueToProductionResponse
    {
        public ProductionResponse Record { get; set; }
    }
}