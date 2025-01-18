# Repository Pattern in .NET

The Repository Pattern is a design pattern that abstracts the data access layer, providing a centralized location for managing and querying data. It promotes separation of concerns, testability, and maintainability by decoupling business logic from the data access layer.

## Why Use the Repository Pattern?
- **Abstraction:** Provides a clean abstraction of data access logic.
- **Testability:** Makes unit testing easier by allowing mock repositories.
- **Maintainability:** Centralizes data access logic, reducing duplication.
- **Flexibility:** Supports multiple data sources (e.g., databases, APIs).

## Key Components
1. **Repository Interface**: Defines the methods for interacting with the data source.
2. **Repository Implementation**: Implements the repository interface, containing the actual data access logic.
3. **Unit of Work (Optional)**: Coordinates multiple repositories to maintain data consistency.
4. **Entities/Models**: Represents the data structures being worked on.

---

## Example: Repository Pattern with a Blog Application

### Scenario
We are building a blog application with the following requirements:
- CRUD operations for `Blog` and `Post` entities.
- Centralized data access logic using the Repository Pattern.

### Step 1: Define Entities
```csharp
public class Blog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
```

### Step 2: Create the Repository Interface
```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveAsync();
}
```

### Step 3: Implement the Generic Repository
```csharp
public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
```

### Step 4: Create Specific Repositories
```csharp
public interface IBlogRepository : IRepository<Blog>
{
    Task<Blog> GetBlogWithPostsAsync(int blogId);
}

public class BlogRepository : Repository<Blog>, IBlogRepository
{
    public BlogRepository(DbContext context) : base(context) { }

    public async Task<Blog> GetBlogWithPostsAsync(int blogId)
    {
        return await _context.Set<Blog>()
            .Include(b => b.Posts)
            .FirstOrDefaultAsync(b => b.Id == blogId);
    }
}
```

### Step 5: Configure Dependency Injection
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<BlogContext>(options =>
        options.UseSqlServer("YourConnectionString"));

    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<IBlogRepository, BlogRepository>();
}
```

### Step 6: Use the Repository in a Controller
```csharp
[ApiController]
[Route("api/[controller]")]
public class BlogsController : ControllerBase
{
    private readonly IBlogRepository _blogRepository;

    public BlogsController(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogs()
    {
        var blogs = await _blogRepository.GetAllAsync();
        return Ok(blogs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlogById(int id)
    {
        var blog = await _blogRepository.GetBlogWithPostsAsync(id);
        if (blog == null) return NotFound();
        return Ok(blog);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog(Blog blog)
    {
        await _blogRepository.AddAsync(blog);
        await _blogRepository.SaveAsync();
        return CreatedAtAction(nameof(GetBlogById), new { id = blog.Id }, blog);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlog(int id, Blog updatedBlog)
    {
        if (id != updatedBlog.Id) return BadRequest();
        _blogRepository.Update(updatedBlog);
        await _blogRepository.SaveAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlog(int id)
    {
        var blog = await _blogRepository.GetByIdAsync(id);
        if (blog == null) return NotFound();
        _blogRepository.Delete(blog);
        await _blogRepository.SaveAsync();
        return NoContent();
    }
}
```

---

## Benefits of the Repository Pattern
- **Centralized Data Access Logic**: Reduces duplication and improves consistency.
- **Ease of Testing**: Mock repositories allow for straightforward unit testing.
- **Improved Flexibility**: Easily switch between data sources or update the data access logic.

## Conclusion
The Repository Pattern is a powerful tool for building maintainable and testable applications. By implementing this pattern, you can simplify data access logic and focus on business requirements without worrying about the underlying data source.

Happy coding!
