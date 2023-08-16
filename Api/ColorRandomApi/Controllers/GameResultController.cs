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
    [Route("[controller]")]
    [ApiController]
    public class GameResultController : ControllerBase
    {
        ApplicationDbContext _context;

        public GameResultController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public GameResult AddResult([FromBody] GameResult gameResult)
        {
            _context.GameResults.Add(gameResult);
            _context.SaveChanges();

            return gameResult;
        }

        [HttpGet]
        public List<GameResult> GetResults()
        {
            List<GameResult> results = _context.GameResults
                .OrderByDescending(item => item.Id)
                .ToList();

            return results;
        }

        [HttpGet("{id}")]
        public GameResult GetResults(int id)
        {
            GameResult result = _context.GameResults
                .Where(item => item.Id == id)
                .FirstOrDefault();

            return result;
        }

        //[HttpPut]
        //public bool UpdateResult([FromBody] GameResult gameResult)
        //{
        //    var findResult = _context.GameResults
        //        .Where(x => x.Id == gameResult.Id)
        //        .FirstOrDefault();

        //    if (findResult == null)
        //        return false;

        //    findResult.SkillName1 = gameResult.SkillName1;
        //    _context.SaveChanges();

        //    return true;
        //}


        //[HttpDelete]
        //public bool DeleteSkill(int id)
        //{
        //    var findResult = _context.Skills
        //        .Where(x => x.SkillId == id)
        //        .FirstOrDefault();

        //    if (findResult == null)
        //        return false;

        //    _context.Skills.Remove(findResult);
        //    _context.SaveChanges();

        //    return true;
        //}
    }
}
