using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Repository.Context
{
    public class APIMovieDbContext : DbContext
    {
        public APIMovieDbContext(DbContextOptions options) : base(options)
        {
        }

        protected APIMovieDbContext()
        {
        }
    }
}
