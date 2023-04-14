using System.ComponentModel.DataAnnotations;

namespace hava.Models;

public class Job
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<UserJob> UserJobs { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
}


public class JobPost
{
    [Required(ErrorMessage = "Name is required yo")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "ClientId is required yo")]
    public int ClientId { get; set; }
}

public class JobPut
{
    [Required(ErrorMessage = "Id is required yo")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required yo")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "ClientId is required yo")]
    public int ClientId { get; set; }
}