using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models;



namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {        
        
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {}

        public DbSet<Value> Values { get; set; }


        public DbSet<User> Users { get; set; }


        public DbSet<Photo> Photos { get; set; }

        // public async Task<List<PeopleType>> GetAsyncPeopleList()
        // {
        //     List<PeopleType> peopleList = new List<PeopleType>()
        //     {
        //     new PeopleType(){ Name = "Frank", Gender = "M" },
        //     new PeopleType(){ Name = "Rose", Gender = "F" },
        //     new PeopleType(){ Name = "Mark", Gender = "M" },
        //     new PeopleType(){ Name = "Andrew", Gender = "M" },
        //     new PeopleType(){ Name = "Sam", Gender = "M" },
        //     new PeopleType(){ Name = "Lisa", Gender = "F" },
        //     new PeopleType(){ Name = "Lucy", Gender = "F" }
        //     };

        //     var result = from e in peopleList
        //                 where e.Gender == "M"
        //                 select e;
        //     return await result.AsQueryable().ToListAsync();
        // }

    }
}