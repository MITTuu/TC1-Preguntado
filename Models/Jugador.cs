using System;
using System.Collections.Generic;

namespace Preguntado.Models;

public partial class Jugador
{
    public int IdJugador { get; set; }

    public string Nombre { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public virtual ICollection<HistorialPartidum> HistorialPartida { get; set; } = new List<HistorialPartidum>();
}
