# Overriding Conventions in Entity Framework

Entity Framework provides default conventions to map classes and their properties to database schema. However, there are scenarios where these conventions do not fit your requirements, and you need to override them. This can be done using **Data Annotations** or the **Fluent API**.

---

## **1. Overriding Conventions with Data Annotations**

Data Annotations are attributes that can be applied directly to your model classes and properties to configure the database schema.

### **Examples**

### 1.1 Defining Primary Keys
By default, Entity Framework considers a property named `Id` or `<ClassName>Id` as the primary key. To explicitly define a primary key:

```csharp
using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public int ProductCode { get; set; }
    public string Name { get; set; }
}
```

### 1.2 Setting Column Names
Override the default column name using the `Column` attribute:

```csharp
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    public int Id { get; set; }

    [Column("Product_Name")]
    public string Name { get; set; }
}
```

### 1.3 Defining String Length
Control the maximum length of a string field using the `MaxLength` or `StringLength` attributes:

```csharp
public class Product
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }
}
```

### 1.4 Ignoring Properties
Exclude a property from mapping using the `NotMapped` attribute:

```csharp
public class Product
{
    public int Id { get; set; }

    [NotMapped]
    public string TempData { get; set; }
}
```

### 1.5 Configuring Relationships
Specify relationships between entities using attributes such as `ForeignKey`:

```csharp
public class Order
{
    public int Id { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    public int ProductId { get; set; }
}
```

---

## **2. Overriding Conventions with Fluent API**

The Fluent API provides a more powerful and flexible way to configure the database schema. Configurations are applied in the `OnModelCreating` method of the `DbContext` class.

### **Examples**

### 2.1 Defining Primary Keys

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>()
        .HasKey(p => p.ProductCode);
}
```

### 2.2 Setting Column Names

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>()
        .Property(p => p.Name)
        .HasColumnName("Product_Name");
}
```

### 2.3 Configuring Column Types and Length

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>()
        .Property(p => p.Name)
        .HasMaxLength(100)
        .HasColumnType("nvarchar");
}
```

### 2.4 Ignoring Properties

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>()
        .Ignore(p => p.TempData);
}
```

### 2.5 Configuring Relationships
#### One-to-Many Relationship

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Order>()
        .HasOne(o => o.Product)
        .WithMany(p => p.Orders)
        .HasForeignKey(o => o.ProductId);
}
```

#### Many-to-Many Relationship

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<ProductCategory>()
        .HasKey(pc => new { pc.ProductId, pc.CategoryId });

    modelBuilder.Entity<ProductCategory>()
        .HasOne(pc => pc.Product)
        .WithMany(p => p.ProductCategories)
        .HasForeignKey(pc => pc.ProductId);

    modelBuilder.Entity<ProductCategory>()
        .HasOne(pc => pc.Category)
        .WithMany(c => c.ProductCategories)
        .HasForeignKey(pc => pc.CategoryId);
}
```

---

## **Comparison: Data Annotations vs Fluent API**

| Feature                  | Data Annotations                     | Fluent API                          |
|--------------------------|---------------------------------------|--------------------------------------|
| **Ease of Use**          | Simple and intuitive for basic tasks | More verbose, suitable for advanced configurations |
| **Configuration Scope**  | Limited to attributes                | Provides full control over configuration |
| **Maintainability**      | Directly tied to model classes       | Centralized in `OnModelCreating`    |
| **Advanced Features**    | Not supported                        | Supports complex mappings            |

---

## **Best Practices**

1. Use Data Annotations for simple configurations, such as string length or renaming columns.
2. Use the Fluent API for advanced configurations and relationships.
3. Maintain consistency: avoid mixing Data Annotations and Fluent API for the same property.
4. Keep the `OnModelCreating` method organized by grouping configurations logically.
5. Always test your configurations in a staging environment before applying to production.

---

With these tools, you can tailor your database schema to meet your specific requirements, ensuring both flexibility and maintainability in your application.

