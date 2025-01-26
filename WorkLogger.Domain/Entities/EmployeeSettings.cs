namespace WorkLogger.Domain.Entities;

public class EmployeeSettings
{
    public long Id { get; set; }
    
    public Guid EmployeeId { get; set; }
    public ApplicationUser Employee { get; set; }

    public string FullName { get; set; }
    public DateTimeOffset ContractStartedDate { get; set; }
    public int VacationDaysPerYear { get; set; }
    
    public bool IsRequiredToFill { get; set; } 
}
