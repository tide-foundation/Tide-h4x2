namespace H4x2_Vendor.Entities;

using System.ComponentModel.DataAnnotations;
public class User
{
    [Key]
    public string UId { get; set; }
    public string Secret { get; set; }
    

}