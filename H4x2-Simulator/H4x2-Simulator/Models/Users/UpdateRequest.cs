namespace H4x2_Simulator.Models.Users;

using System.ComponentModel.DataAnnotations;
using H4x2_Simulator.Entities;

public class UpdateRequest
{
    public string PubKey { get; set; }
    public string? OrkUrls { get; set; }
    public string? SignedUIds { get; set; }

    public List<String> OrkUrlsList
    {
        get { return OrkUrls.Split(',').ToList(); }
        set
        {
            OrkUrls = String.Join(",", value);
        }
    }

    public List<String> SignedUIdsList
    {
        get { return SignedUIds.Split(',').ToList(); }
        set
        {
            SignedUIds = String.Join(",", value);
        }
    }

}