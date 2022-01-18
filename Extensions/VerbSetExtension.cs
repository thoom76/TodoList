using CommandLine;

namespace TodoList.Extensions;

public static class VerbSetExtension
{
    /// <summary>
    /// Executes <paramref name="action"/> if parsed values are of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="TTarget">Type of the target instance built with parsed value.</typeparam>
    /// <typeparam name="TParam">Type of the extra parameter.</typeparam>
    /// <param name="result">An verb result instance.</param>
    /// <param name="action">The <see cref="Action{TTarget, TParam}"/> to execute.</param>
    /// <param name="parameter">The extra parameter of type <typeparam name="TParam"/> which is passed to the <see cref="Action{TTarget, TParam}"/>.</param>
    /// <returns>The same <paramref name="result"/> instance.</returns>
    public static ParserResult<object> WithParsed<TTarget, TParam>(this ParserResult<object> result, Action<TTarget, TParam> action, TParam parameter)
    {
        var parsed = result as Parsed<object>;
        if (parsed != null)
        {
            if (parsed.Value is TTarget)
            {
                action((TTarget)parsed.Value, parameter);
            }
        }
        return result;
    }
}