using System;
using System.Collections.Generic;

namespace Banco.Models;

public partial class Cuentum
{
    public int NumCuenta { get; set; }

    public string Usuario { get; set; } = null!;

    public double Saldo { get; set; } = 10000;

    public bool Prestamo_Activo { get; set; } = false;

    public virtual ICollection<Historial> Historials { get; } = new List<Historial>();

    public virtual ICollection<Rifa> Rifas { get; } = new List<Rifa>();

    public virtual Usuario? UsuarioNavigation { get; set; }
}
