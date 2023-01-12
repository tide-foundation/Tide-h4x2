namespace H4x2_Simulator.Models.Users;

using System.ComponentModel.DataAnnotations;
using H4x2_Simulator.Entities;

public class UpdateRequest
{
    public string PubKey { get; set; }
    public string[] OrkUrls { get; set; }
    public string SignedUID { get; set; }

}