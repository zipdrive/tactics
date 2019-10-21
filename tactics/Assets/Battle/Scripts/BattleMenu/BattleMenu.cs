using UnityEngine;
using System.Collections.Generic;

public abstract class BattleMenu
{
    public enum UpdateResult
    {
        /// <summary>
        /// Menuing is still being done.
        /// </summary>
        InProgress,

        /// <summary>
        /// A definitive selection has been made.
        /// </summary>
        Completed,

        /// <summary>
        /// Menuing has been canceled, return to previous menu.
        /// </summary>
        Canceled
    }


    protected static BattleManager m_Manager;
    protected static BattleAgent m_Agent;
    protected static Dictionary<string, object> m_Selections;

    public abstract BattleMenu Next { get; set; }

    /// <summary>
    /// Do a per-frame update of the menu.
    /// </summary>
    /// <returns>Canceled if menu canceled, Completed if a final selection has been made, InProgress otherwise.</returns>
    public virtual UpdateResult Update()
    {
        if (Next != null)
        {
            UpdateResult result = Next.Update();

            if (result == UpdateResult.Canceled)
            {
                Next = null;
            }
            else if (result == UpdateResult.Completed)
            {
                return result;
            }
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            Destruct();
            return UpdateResult.Canceled;
        }

        return UpdateResult.InProgress;
    }

    /// <summary>
    /// Construct the menu.
    /// </summary>
    public abstract void Construct();

    /// <summary>
    /// Destroy the menu.
    /// </summary>
    public abstract void Destruct();
}