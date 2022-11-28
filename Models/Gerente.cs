using System;
using System.Collections.Generic;

namespace Banco.Models;

public partial class Gerente
{
    public int NumNom { get; set; }

    public int Nomina { get; set; }

    public int DiasVacaciones { get; set; }

    public virtual Empleado NominaNavigation { get; set; } = null!;
}
