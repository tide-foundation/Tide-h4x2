namespace H4x2_Node.Entities;

using System.ComponentModel.DataAnnotations;
public class User
{
    [Key]
    public string UId {get; set;}
    public string Prismi { get; set; }
    public string Cvki { get; set; }

}