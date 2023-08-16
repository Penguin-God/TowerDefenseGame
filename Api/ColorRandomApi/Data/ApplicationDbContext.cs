using Microsoft.EntityFrameworkCore;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorRandomApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<GameResult> GameResults { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public ApplicationDbContext()
        {
        }
    }
}