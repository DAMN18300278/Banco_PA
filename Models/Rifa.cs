using System;
using System.Collections.Generic;

namespace Banco.Models;

public partial class Rifa
{
    public int NumBoleto { get; set; }

    public int Cuenta { get; set; }

    public DateTime FechaBoleto { get; set; }

    public virtual Cuentum CuentaNavigation { get; set; } = null!;
}
