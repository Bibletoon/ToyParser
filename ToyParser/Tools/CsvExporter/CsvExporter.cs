namespace ToyParser.Tools.CsvExporter;

public class CsvExporter<T>
{
    private readonly ModelCsvConverter<T> _converter;

    public CsvExporter(ModelCsvConverter<T> converter)
    {
        _converter = converter;
    }

    public async Task Export(IEnumerable<T> models, Stream stream)
    {
        var writer = new StreamWriter(stream);
        foreach (var model in models)
        {
            await writer.WriteLineAsync(_converter.ToCsv(model));
        }
        await writer.FlushAsync();
    }
}