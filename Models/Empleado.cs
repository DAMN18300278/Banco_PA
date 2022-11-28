using System;
using System.Collections.Generic;

namespace Banco.Models;

public partial class Empleado
{
    public int Nomina { get; set; }

    public string Curp { get; set; } = null!;

    public virtual Usuario CurpNavigation { get; set; } = null!;

    public virtual ICollection<Gerente> Gerentes { get; } = new List<Gerente>();

    public virtual ICollection<Historial> Historials { get; } = new List<Historial>();
}
