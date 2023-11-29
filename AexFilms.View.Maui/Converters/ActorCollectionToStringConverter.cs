using AexFilms.DataAccess.Entities;

using System.Globalization;

namespace AexFilms.View.Maui.Converters;

/// <summary>
///     A converter that converts a collection of <see cref="Actor"/> into a comma-separated string of actor full names.
/// </summary>
internal class ActorCollectionToStringConverter : IValueConverter
{
    /// <summary>
    ///     Converts the provided value, which should be a collection of <see cref="Actor"/>, into a comma-separated string
    /// of actor full names.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The type of the target property (not used in this implementation).</param>
    /// <param name="parameter">An optional parameter (not used in this implementation).</param>
    /// <param name="culture">The culture to use for the conversion (not used in this implementation).</param>
    /// <returns>A string containing comma-separated actor full names.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ICollection<Actor> actorCollection)
            return null;

        var actorFullNameCollection = actorCollection.Select(actor => actor.FullName);
        return string.Join(", ", actorFullNameCollection);
    }

    /// <summary>
    ///     Converts the provided string value back to the original collection of <see cref="Actor"/>.
    ///     This operation is not implemented and will throw a <see cref="NotImplementedException"/>.
    /// </summary>
    /// <remarks>(Not implemented)</remarks>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The type of the target property (not used in this implementation).</param>
    /// <param name="parameter">An optional parameter (not used in this implementation).</param>
    /// <param name="culture">The culture to use for the conversion (not used in this implementation).</param>
    /// <returns>An object (not used in this implementation).</returns>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
