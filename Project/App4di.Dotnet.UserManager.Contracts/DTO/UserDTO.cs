namespace App4di.Dotnet.UserManager.Contracts.DTO;

public class UserDTO
{
    public int UserId { get; set; }
    public required string LoginName { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public required string BirthPlace { get; set; }
    public required string AddressCity { get; set; }
}

