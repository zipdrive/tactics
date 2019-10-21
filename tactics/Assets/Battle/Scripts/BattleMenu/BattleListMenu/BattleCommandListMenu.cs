﻿using System.Collections.Generic;

public class BattleCommandListMenu : BattleListMenu
{
    private Stack<BattleMenu> m_Next = new Stack<BattleMenu>();

    public override BattleMenu Next
    {
        get
        {
            return m_Next.Count > 0 ? m_Next.Peek() : null;
        }

        set
        {
            if (value == null && m_Next.Count > 0)
            {
                m_Next.Pop();

                if ((Next == null || Next is BattleListMenu) && m_UI == null)
                {
                    Construct();
                }

                BattleMenu menu = Next;
                while (menu != null)
                {
                    menu.Construct();
                    menu = menu.Next;
                }

                if (m_Next.Count == 0) m_Command = null;
            }
        }
    }

    private BattleCommand m_Command;

    public BattleCommand Command
    {
        get
        {
            return m_Command;
        }

        set
        {
            m_Command = value;

            if (m_Command != null)
            {
                m_Selections = new Dictionary<string, object>();
            }
        }
    }


    protected class CommandOption : Option
    {
        public readonly BattleCommand command;

        public CommandOption(BattleCommand command, bool canMove, bool canAct)
        {
            this.command = command;

            labels = new string[] { command.Label };

            if (command.Expends == BattleCommand.Type.Move) enabled = canMove;
            else if (command.Expends == BattleCommand.Type.Action) enabled = canAct;
            else enabled = true;
            enabled = enabled & command.Enabled(m_Agent);

            description = new string[] { command.Description };
        }

        public override UpdateResult OnSelect(BattleMenu menu)
        {
            BattleCommandListMenu commandMenu = menu as BattleCommandListMenu;

            if (commandMenu != null)
            {
                commandMenu.Command = command;
                return UpdateResult.Completed;
            }

            return UpdateResult.InProgress;
        }
    }


    public BattleCommandListMenu(BattleManager manager, BattleAgent agent, bool canMove, bool canAct)
    {
        m_Manager = manager;
        m_Agent = agent;

        foreach (BattleCommand command in m_Agent.BaseCharacter.Commands)
        {
            Add(new CommandOption(command, canMove, canAct));
        }

        Add(new CommandOption(AssetHolder.Commands["End Turn"], canMove, canAct));
    }

    public override void Construct()
    {
        m_Manager.grid.Selector.SelectedTile = m_Agent.Coordinates;
        m_Manager.grid.Selector.Snap();

        base.Construct();
        BattleSelector.Frozen = true;
    }

    public override void Destruct()
    {
        if (m_UI != null)
        {
            base.Destruct();
        }

        BattleSelector.Frozen = false;
    }

    public UpdateResult Update(out BattleAction decision)
    {
        UpdateResult result = Update();

        if (result == UpdateResult.Completed)
        {
            BattleMenu menu = Next;
            while (menu != null)
            {
                menu.Destruct();
                menu = menu.Next;
            }

            if (m_Next.Count == m_Command.Selections.Count)
            {
                // Destroy this menu
                Destruct();

                // Construct final decision using m_Command.Actions
                List<BattleAction> sequence = new List<BattleAction>();

                foreach (BattleCommandAction action in m_Command.Actions)
                    sequence.Add(action.Construct(m_Agent, m_Selections));

                decision = sequence.Count == 1 ? sequence[0] : new BattleSequenceAction(sequence);
                return result;
            }
            else
            {
                m_Next.Push(m_Command.Selections[m_Next.Count].Construct(m_Agent, m_Selections));
                Next.Construct();

                if (!(Next is BattleListMenu))
                {
                    Destruct();
                }
            }
        }

        decision = null;
        return result;
    }
}