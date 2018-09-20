using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OnNameInput : BroadcastEvent<OnNameInput>
{
    public string newName;

    public OnNameInput(string newName) { this.newName = newName; }
}
