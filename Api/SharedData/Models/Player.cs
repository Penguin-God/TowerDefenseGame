using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedData.Models
{
    [Serializable]
    public class Player
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Gold { get; set; }
        public int Gem { get; set; }
        public DateTime Date { get; set; }

        public List<Skill> Skills { get; set; }
    }


}