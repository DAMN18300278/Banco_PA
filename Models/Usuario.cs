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

    public DateTime Cumpleaños { get; set; }

    public string Contraseña { get; set; } = null!;

    public virtual ICollection<Cuentum> Cuenta { get; } = new List<Cuentum>();

    public virtual ICollection<Empleado> Empleados { get; } = new List<Empleado>();
}
