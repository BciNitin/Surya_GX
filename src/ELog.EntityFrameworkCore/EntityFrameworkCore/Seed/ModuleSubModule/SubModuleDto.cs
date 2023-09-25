namespace ELog.EntityFrameworkCore.EntityFrameworkCore.Seed.ModuleSubModule
{
    public class SubModuleSeedDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string SubModuleType { get; set; }
        public int Sequence { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsApprovalRequired { get; set; }
        public bool IsApprovalWorkflowRequired { get; set; }
    }
}