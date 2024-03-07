using Domain.Entities.Users;
using Domain.Entities.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Driven.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id).HasConversion(UserIdValueConverter.Default);
        
        builder.Property(user => user.Name).HasMaxLength(100).IsRequired();
        builder.Property(user => user.Name).HasConversion(NameValueConverter.Default);
        builder.Property(user => user.Email).HasMaxLength(250).IsRequired();
        builder.Property(user => user.Email).HasConversion(EmailValueConverter.Default);
    }
}

internal sealed class UserIdValueConverter: ValueConverter<UserId, Guid>
{
    private UserIdValueConverter(ConverterMappingHints mappingHints)
        : base(
            id => id.Id,
            value => new UserId(value),
            mappingHints)
    { }

    public static UserIdValueConverter Default { get; }
        = new (new ConverterMappingHints());
}

internal sealed class EmailValueConverter: ValueConverter<Email, string>
{
    private EmailValueConverter(ConverterMappingHints mappingHints)
        : base(
            id => id.Value,
            value => new Email(value),
            mappingHints)
    { }

    public static EmailValueConverter Default { get; }
        = new (new ConverterMappingHints());
}

internal sealed class NameValueConverter: ValueConverter<Name, string>
{
    private NameValueConverter(ConverterMappingHints mappingHints)
        : base(
            id => id.Value,
            value => new Name(value),
            mappingHints)
    { }

    public static NameValueConverter Default { get; }
        = new (new ConverterMappingHints());
}