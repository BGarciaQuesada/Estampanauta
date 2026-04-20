using System;
using UnityEngine;

// Esta clase almacena la información de un objeto especifico
// Se emplea a través de ObjetiveUI y NO SE ASIGNA A NADA (es una plantilla, vamos)

[Serializable]
public class Objective
{
    public string id;         // Identificador (ej: "Crystal", "Fuel")
    public int required;      // Cuántos necesita
    public int current;       // Cuántos tiene

    // Completado si el actual igual (o mayor porque vete a saber tú...) al requerido
    public bool IsComplete => current >= required;

    // Puede recibir si el actual es menor al requerido
    public bool CanReceive => current < required;

    public void Add()
    {
        current++;
    }
}