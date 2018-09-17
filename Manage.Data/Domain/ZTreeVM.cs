using System.Collections.Generic;
using Newtonsoft.Json;

namespace Manage.Data.Domain
{
    public class ChildrenItem
    {
        public string name { get; set; }
        public int Permission_Id { get; set; }
        public int Role_Id { get; set; }
        [JsonProperty("checked")]
        public bool Checked { get; set; }
    }

    public class Children
    {
        public string name { get; set; }
        public List<ChildrenItem> children { get; set; }
    }

    public class Parent
    {
        public string name { get; set; }
        public bool open { get; set; }
        public List<Children> children { get; set; }
    }

    public class Root
    {
        public List<Parent> parent { get; set; }
    }
}
