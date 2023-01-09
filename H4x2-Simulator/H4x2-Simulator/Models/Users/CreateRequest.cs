namespace H4x2_Simulator.Models.Users;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CreateRequest
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string PubKey { get; set; }

    
    public string? OrkUrls { get; set; }

   
    public List<String> OrkUrlsList
    {
        get { return OrkUrls.Split(',').ToList(); }
        set
        {
            OrkUrls = String.Join(",", value);
        }
    }

    public string? SignedUIds { get; set; }

    public List<String> SignedUIdsList
    {
        get { return SignedUIds.Split(',').ToList(); }
        set
        {
            SignedUIds = String.Join(",", value);
        }
    }

}


