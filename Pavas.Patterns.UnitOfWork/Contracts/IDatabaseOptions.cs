namespace Pavas.Patterns.UnitOfWork.Contracts;

public interface IDatabaseOptions
{
    /// <summary>
    /// Gets or sets the connection string used to connect to the database.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> representing the connection string for the database.
    /// </value>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the default tenant ID to be used for database records.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> representing the default tenant ID.
    /// </value>
    public string TenantId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether soft delete logic is enabled for entities implementing <see cref="ISoftDelete"/>.
    /// </summary>
    /// <value>
    /// A <see cref="bool"/> indicating whether soft delete is enabled.
    /// </value>
    public bool SoftDelete { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether create database logic />.
    /// </summary>
    /// <value>
    /// A <see cref="bool"/> indicating whether ensure create database is enabled.
    /// </value>
    public bool EnsureCreated { get; set; }
}