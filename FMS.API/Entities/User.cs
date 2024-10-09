namespace FMS.API.Entities;

public class User : IAuditableEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string PasswordHash { get; set; }
    public DateTimeOffset CreatedOnUtc {  get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}
