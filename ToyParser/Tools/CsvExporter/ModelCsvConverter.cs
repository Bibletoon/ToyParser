namespace ToyParser.Tools.CsvExporter;

public abstract class ModelCsvConverter<TModel>
{
    protected string PrepareString(string value)
    {
        var result = value.Replace("\"", "\"\"");
        return result.Contains(',') || result.StartsWith('[') ? $"\"{result}\"" : result;
    }

    protected string PrepareArray(string[] array)
    {
        return $"[{array.Select(PrepareString).Aggregate((x, y) => $"{x},{y}")}]";
    }

    public abstract string ToCsv(TModel model);
}