using System;
using System.Collections.Generic;

namespace Banco.Models;

public partial class Historial
{
    public int NumHistorial { get; set; }

    public double Cantidad { get; set; }

    public byte PagoRealizados { get; set; }

    public byte PagoPendientes { get; set; }

    public int NumCuenta { get; set; }

    public int? NominaEncargado { get; set; }

    public byte? Estado { get; set; }

    public virtual ICollection<DatosPrestamo> DatosPrestamos { get; } = new List<DatosPrestamo>();

    public virtual Empleado? NominaEncargadoNavigation { get; set; }

    public virtual Cuentum NumCuentaNavigation { get; set; } = null!;
}
