using System;
using UnityEngine;

[Serializable]
public class Objective
{
    public string id;         // Identificador (ej: "Crystal", "Fuel")
    public int required;      // Cuántos necesita
    public int current;       // Cuántos tiene

    // Completado si el actual igual (o mayor porque veta a saber tú...) al requerido
    public bool IsComplete => current >= required;

    // Puede recibir si el actual es menor al requerido
    public bool CanReceive => current < required;

    public void Add()
    {
        current++;
    }
}