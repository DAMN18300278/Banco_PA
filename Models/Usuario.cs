using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Banco.Models;

public partial class Usuario
{
    public string Curp { get; set; } = null!;

    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Ingrese un nombre valido por favor")]
    public string NombreS { get; set; } = null!;

    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Ingrese un nombre valido por favor")]
    public string ApellidoP { get; set; } = null!;

    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Ingrese un nombre valido por favor")]
    public string ApellidoM { get; set; } = null!;

    public bool? Autorizada { get; set; }
    
    [datevalidation]
    public DateTime Cumpleaños { get; set; }
    [StringLength(20)]
    public string Contraseña { get; set; } = null!;

    public virtual ICollection<Cuentum> Cuenta { get; } = new List<Cuentum>();

    public virtual ICollection<Empleado> Empleados { get; } = new List<Empleado>();
}

public class datevalidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        value = (DateTime)value;
        DateTime dt = new DateTime(1962, 1, 1, 12, 0, 0);
        // This assumes inclusivity, i.e. exactly six years ago is okay
        if (dt.CompareTo(value) < 0)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("No se aceptan nacimientos antes de 1961");
        }
    }
}

