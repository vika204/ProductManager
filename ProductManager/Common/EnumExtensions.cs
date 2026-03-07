using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProductManager.Common
{
    // stores enum value together with its display text for ui binding
    public sealed record EnumWithName<TEnum>(TEnum Value, string DisplayName)
        where TEnum : struct, Enum;

    public static class EnumExtensions
    {
        // reads display name from the Display attribute if it exists
        public static string GetDisplayName(this Enum value)
        {
            Type type = value.GetType();
            string? name = Enum.GetName(type, value);

            if (name is null)
            {
                return value.ToString();
            }

            FieldInfo? field = type.GetField(name);
            DisplayAttribute? display = field?.GetCustomAttribute<DisplayAttribute>();

            // fallback to enum field name if no Display attribute is defined
            return display?.Name ?? name;
        }

        // creates a value-display pair for a single enum value
        public static EnumWithName<TEnum> GetEnumWithName<TEnum>(this TEnum value)
            where TEnum : struct, Enum
        {
            return new EnumWithName<TEnum>(value, value.GetDisplayName());
        }

        // returns all enum values together with their display names
        public static EnumWithName<TEnum>[] GetValueWithNames<TEnum>()
            where TEnum : struct, Enum
        {
            TEnum[] values = Enum.GetValues<TEnum>();
            EnumWithName<TEnum>[] result = new EnumWithName<TEnum>[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                result[i] = values[i].GetEnumWithName();
            }

            return result;
        }
    }
}