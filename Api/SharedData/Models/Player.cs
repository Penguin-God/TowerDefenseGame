using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Models
{
    [Serializable]
    public class Player
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<Skill> skills { get; set; }
        public DateTime Date { get; set; }
    }

    [Serializable]
    public class Skill
    {
        public int SkillId { get; set; }
        public int SkillName { get; set; }
        public int SkillExp { get; set; }
        public int SkillLevel { get; set; }
        //public int OwnerId { get; set; }
        //public Player Owner { get; set; }
    }

}