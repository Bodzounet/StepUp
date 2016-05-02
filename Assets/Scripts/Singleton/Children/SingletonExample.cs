using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// Utilisation : SingletonExample.Instance.Montralala
/// NE PAS mettre ce script sur un objet dans la scène, et NE PAS en faire un préfab.
/// Juste, l'appeler comme indiqué plus haut.
/// Il se chargera d'exister.
/// 
/// les variables stockées ici traversent les scènes.
/// PAS les références, mais c'est inhérent à Unity. (donc ne pas stocker de GO, collider, etc...)
/// 
/// Awake, Start, Update, ... -> fortement déconseillées. Très fortement déconseillées.
/// </summary>
public class SingletonExample : Singleton<SingletonExample> 
{
    private int pipi;
    public int Pipi
    {
        get { return pipi; }
        set { pipi = value; }
    }

    private GameObject caca; // attention, si ce GO étant dans une autre scène et qu'il y a eu un changement entre tps, la référence n'est plus valide et donc il vaut null.
    public GameObject Caca
    {
        get { return caca; }
        set { caca = value; }
    }

    public void ResetMonSingleton()
    {
        pipi = 0;
        caca = null;
    }

    public int doAwesomeStuff(int i)
    {
        return i;
    }
}
