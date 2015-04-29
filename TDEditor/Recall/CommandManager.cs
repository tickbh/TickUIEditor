using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEditor.Recall
{
    public class CommandManager
    {

        private Stack undoStack    = new Stack() ;
        private Stack reverseStack = new Stack() ;
        private int saveIndex = 0;

        public CommandManager()
        {
        }

        public void AddCommand(CommandBase command)
        {
            if (!command.CheckVaild())
            {
                return;
            }
            this.undoStack.Push(command);
            this.reverseStack.Clear();
        }


        public CommandBase Undo()
        {
            if (this.undoStack.Count == 0)
            {
                return null;
            }
            CommandBase command = (CommandBase)this.undoStack.Pop() ;
            if(command == null)
            {
                return null;
            }
            command.Undo();
            this.reverseStack.Push(command) ;
            return command;
        }

        public CommandBase Redo()
        {
            if (this.undoStack.Count == 0)
            {
                return null;
            }
            CommandBase command = (CommandBase)this.reverseStack.Pop() ;
            if(command == null)
            {
                return null;
            }
            command.Redo();
            this.undoStack.Push(command) ;
            return command;
        }

        public void Save()
        {
            saveIndex = this.undoStack.Count;
        }

        public bool IsModify()
        {
            return saveIndex != this.undoStack.Count;
        }
    }
}
