using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContactsManager.Core.Domain.Entities
{
    /// <summary>
    /// Person domain model class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }
        [StringLength(40)] //nvarchar(40)
        public string? PersonName { get; set; }
        [StringLength(40)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        [StringLength(200)]
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        //[Column("TaxIdentificationNumber",TypeName="varchar(8)")]
        public string? TIN { get; set; }
        [ForeignKey(nameof(CountryID))]
        public virtual Country? Country { get; set; }

        public override string ToString()
        {

            return new StringBuilder().AppendLine($"PersonID: {PersonID}")
            .AppendLine($"PersonName: {PersonName}")
            .AppendLine($"Email: {Email}")
            .AppendLine($"DateOfBirth: {DateOfBirth?.ToString("MM/dd/yyyy")}")
            .AppendLine($"Gender: {Gender}")
            .AppendLine($"CountryID: {CountryID}")
            .AppendLine($"Address: {Address}")
            .AppendLine($"ReceiveNewsLetters: {ReceiveNewsLetters}").ToString();
        }
    }
}
