using System;
using System.Collections.Generic;

namespace Banco.Models;

public partial class DatosPrestamo
{
    public int NumFolio { get; set; }

    public DateTime FechaSolicitud { get; set; }

    public DateTime FechaAprobacion { get; set; }

    public DateTime FechaLiquidacion { get; set; }

    public DateTime FechaLimite { get; set; }

    public int NumHistorial { get; set; }

    public virtual Historial NumHistorialNavigation { get; set; } = null!;
}
