using ColorRandomApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorRandomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public Player AddSkill([FromBody] Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();

            return player;
        }

        [HttpGet]
        public List<Player> GetPlayers()
        {
            List<Player> results = _context.Players
                .OrderByDescending(item => item.UserId)
                .ToList();

            return results;
        }

        [HttpGet("{id}")]
        public Player GetPlayers(int id)
        {
            Player result = _context.Players
                .Where(item => item.Id == id)
                .FirstOrDefault();

            return result;
        }

        [HttpPut]
        public bool UpdatePlayer([FromBody] Player player)
        {
            var findResult = _context.Players
                .Where(x => x.Id == player.Id)
                .FirstOrDefault();

            if (findResult == null)
                return false;

            findResult.Name = player.Name;
            findResult.Skills = player.Skills;
            _context.SaveChanges();

            return true;
        }

        [HttpDelete]
        public bool DeletePlaer(int id)
        {
            var findResult = _context.Players
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (findResult == null)
                return false;

            _context.Players.Remove(findResult);
            _context.SaveChanges();

            return true;
        }
    }
}
