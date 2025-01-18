# **Migrations in Entity Framework**

This document provides an overview of how to use migrations in Entity Framework for different scenarios, including setting up migrations for new and existing databases, adding or modifying classes, handling mistakes, and downgrading a database. Additionally, it explains best practices for naming migrations and describing SQL functions used in migration files.

---

## **1. Code First with a New Database**

When creating a new database from scratch using Entity Framework:

### Steps:
1. Define your models in code (e.g., classes representing entities).
2. Add a `DbContext` class with `DbSet` properties for each model.
3. Enable migrations by running the following command in the Package Manager Console:
   ```shell
   Enable-Migrations
   ```
   This creates a `Migrations` folder with a `Configuration.cs` file.
4. Create an initial migration:
   ```shell
   Add-Migration InitialCreate
   ```
   **Naming Convention:**  
   Use a clear and descriptive name for the migration, such as `InitialCreate`, to indicate that it is the initial migration for the project.
5. Apply the migration to the database:
   ```shell
   Update-Database
   ```
6. A new database is created based on your model definitions.

---

## **2. Code First with an Existing Database**

When using an existing database with Entity Framework:

### Steps:
1. Reverse-engineer the database using the `Entity Framework Power Tools` or the `Scaffold-DbContext` command.
   ```shell
   Scaffold-DbContext "YourConnectionString" Microsoft.EntityFrameworkCore.SqlServer
   ```
2. Enable migrations:
   ```shell
   Enable-Migrations
   ```
3. Add an initial migration to capture the current state of the database:
   ```shell
   Add-Migration InitialDatabaseSync
   ```
   **Naming Convention:**  
   Use names like `InitialDatabaseSync` to signify that this migration aligns the code with the existing database.
4. Verify the migration script and apply it:
   ```shell
   Update-Database
   ```

---

## **3. Adding a New Class**

When introducing a new entity to the project:

### Steps:
1. Create a new class representing the entity.
2. Add a `DbSet` property for the new class in the `DbContext`.
3. Add a migration to include the new entity:
   ```shell
   Add-Migration AddNewEntityName
   ```
   **Naming Convention:**  
   Use a name like `AddProductTable` or `AddOrderDetailsEntity` to clearly describe the added entity.
4. Apply the migration:
   ```shell
   Update-Database
   ```

---

## **4. Modifying an Existing Class**

When changing the structure of an existing entity (e.g., adding, removing, or renaming properties):

### Steps:
1. Update the class definition with the desired changes.
2. Add a migration:
   ```shell
   Add-Migration UpdateEntityNameOrPropertyChange
   ```
   **Naming Convention:**  
   Use a name like `AddPriceColumnToProducts` or `RenameCustomerNameToFullName` to reflect the modification.
3. Apply the migration:
   ```shell
   Update-Database
   ```

---

## **5. Deleting an Existing Class**

When removing an entity from the project:

### Steps:
1. Delete the class file.
2. Remove the corresponding `DbSet` property from the `DbContext`.
3. Add a migration:
   ```shell
   Add-Migration RemoveEntityName
   ```
   **Naming Convention:**  
   Use a name like `RemoveProductTable` to clearly indicate what is being deleted.
4. Apply the migration:
   ```shell
   Update-Database
   ```

---

## **6. Recovery from a Mistake**

If a mistake occurs during the migration process:

### Steps:
1. Roll back the database to the previous migration:
   ```shell
   Update-Database -TargetMigration PreviousMigrationName
   ```
2. Correct the mistake in the code or migration file.
3. Generate a new migration if necessary:
   ```shell
   Add-Migration CorrectedMigration
   ```
4. Apply the corrected migration:
   ```shell
   Update-Database
   ```

---

## **7. Migrations Downgrading a Database**

To downgrade a database to a previous state:

### Steps:
1. Identify the target migration:
   ```shell
   Get-Migrations
   ```
2. Roll back to the target migration:
   ```shell
   Update-Database -TargetMigration TargetMigrationName
   ```

### Example:
If you want to downgrade to the initial migration:
```shell
Update-Database -TargetMigration InitialCreate
```

---

## **8. Adding SQL Functions in Migration Files**

If your migration requires SQL functions, you can include custom SQL code in the migration file.

### Steps:
1. Open the generated migration file in the `Migrations` folder.
2. Use the `Sql` method inside the `Up` and `Down` methods to execute raw SQL commands.

### Example:
```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql("CREATE FUNCTION GetCustomerOrders (@CustomerId INT) RETURNS TABLE AS RETURN (SELECT * FROM Orders WHERE CustomerId = @CustomerId)");
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql("DROP FUNCTION IF EXISTS GetCustomerOrders");
}
```

**Best Practice:**  
- Always include corresponding `Down` logic to revert changes made by the `Up` method.

---

## **Tips and Best Practices**

- **Naming Migrations:**  
  Use clear and concise names that describe the purpose of the migration, such as `AddNewColumnToOrders` or `UpdateUserSchema`.

- **Deleting a Model:**  
    - create migration to delete first the relations
    - create migration to save the data exists in the table
    - dorp the table
- **Recovery from mistakes:**  
    Migrations like git commits we can't update it but we create new one.

- **Review Migration Files:**  
  Always review the generated migration files for correctness before applying them.

- **Source Control:**  
  Commit migration files to version control to track database changes.

- **Staging Environment:**  
  Test migrations in a staging environment before deploying to production.

- **Model-Database Sync:**  
  Ensure that your models and database schema remain in sync to avoid runtime errors.

---

With these steps and practices, you can effectively manage migrations in Entity Framework while maintaining a clear and traceable history of database changes.

