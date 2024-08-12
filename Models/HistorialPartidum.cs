using System;
using System.Collections.Generic;

namespace Preguntado.Models;

public partial class HistorialPartidum
{
    public int IdPartida { get; set; }

    public int IdJugador { get; set; }

    public int Aciertos { get; set; }

    public DateTime FechaPartida { get; set; }

    public bool Resultado { get; set; }

    public virtual Jugador IdJugadorNavigation { get; set; } = null!;
}
