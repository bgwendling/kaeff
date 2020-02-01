using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Object_interaction.Interfaces
{
    public interface ICoffee
    {
        string Name { get; }
        int MilkType { get; set; }
        int BeanID { get; set; }
    }
}
