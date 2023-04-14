using System.ComponentModel.DataAnnotations;

namespace hava.Models;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContactInformation { get; set; }

    public ICollection<Job> Jobs { get; set; } 
}




public class ClientGet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContactInformation { get; set; }
}

public class ClientPost
{
    [Required(ErrorMessage = "Name is required yo")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "ContactInformation is required yo")]
    public string ContactInformation { get; set; }
}

public class ClientPut
{
    [Required(ErrorMessage = "Id is required")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required yo")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "ContactInformation is required yo")]
    public string ContactInformation { get; set; }
}

public class ClientConverter
{
    public static ClientGet ClientToClientGet(Client Client)
    {
        return new ClientGet()
        {
            Id = Client.Id,
            Name = Client.Name,
            ContactInformation = Client.ContactInformation,
        };
    }
}