using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Models
{
    [Serializable]
    public class GameResult
    {
        public int Id { get; set; }
        public string SkillName1 { get; set; }
        public string SkillName2 { get; set; }
        public DateTime GameTime { get; set; }
        public string winSkill { get; set; }    
    }
}
