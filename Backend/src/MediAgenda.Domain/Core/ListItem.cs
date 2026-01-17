using System;
using System.Collections.Generic;
using System.Text;

namespace MediAgenda.Domain.Core
{
    public class ListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ListItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
