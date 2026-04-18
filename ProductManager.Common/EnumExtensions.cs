using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ProductManager.Common
{
    // stores enum value together with text that will be shown in UI controls.
    public sealed record EnumWithName<TEnum>(TEnum Value, string DisplayName)
        where TEnum : struct, Enum;

    public static class EnumExtensions
    {
        // returns text for enum value.if DisplayAttribute exists, its text is used.otherwise enum field name is returned.
        public static string GetDisplayName(this Enum value)
        {
            Type type = value.GetType();
            string? name = Enum.GetName(type, value);

            if (name is null)
            {
                return value.ToString();
            }

            FieldInfo? field = type.GetField(name);
            DisplayAttribute? display = field?.GetCustomAttribute<DisplayAttribute>(inherit: false);

            return display?.GetName() ?? name;
        }

        // converts one enum value into a pair . actual value + text for UI.
        public static EnumWithName<TEnum> GetEnumWithName<TEnum>(this TEnum value)
            where TEnum : struct, Enum
        {
            return new EnumWithName<TEnum>(value, value.GetDisplayName());
        }

        // returns all enum values together with their display names. used for Picker binding in forms.
        public static EnumWithName<TEnum>[] GetValuesWithNames<TEnum>()
            where TEnum : struct, Enum
        {
            TEnum[] values = Enum.GetValues<TEnum>();
            EnumWithName<TEnum>[] valuesWithNames = new EnumWithName<TEnum>[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                valuesWithNames[i] = values[i].GetEnumWithName();
            }

            return valuesWithNames;
        }
    }
}