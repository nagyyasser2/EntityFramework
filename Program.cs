
using System.Data.Entity;
using System.Collections.Generic;

namespace EntityFramework
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CourseLevel Level { get; set; }
        public float FullPrice { get; set; }
        public Author Author { get; set; }
        public IList<Tag> Tags { get; set; }        
    }
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Course> Courses { get; set; }

    }
    public class Tag {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Course> courses { get; set; }
    }
    public enum CourseLevel
    {
        Beginner = 1,
        Intermidiate = 2,   
        Advanced = 3,
    }

    public class dbContext: DbContext
    {
        public dbContext():base("name=DefaultConnection")
        {
            
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Tag> Tags { get;  set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
