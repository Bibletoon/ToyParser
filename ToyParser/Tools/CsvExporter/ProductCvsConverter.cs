using System.Globalization;
using System.Text;

namespace ToyParser.Tools.CsvExporter;

public class ProductCvsConverter : ModelCsvConverter<Product>
{
    public override string ToCsv(Product model)
    {
        var resultBuilder = new StringBuilder();
        resultBuilder.Append(PrepareString(model.Name));
        resultBuilder.Append(',');
        resultBuilder.Append(PrepareString(model.Region));
        resultBuilder.Append(',');
        resultBuilder.Append(PrepareArray(model.Breadcrumbs));
        resultBuilder.Append(',');
        resultBuilder.Append(model.IsAvailable);
        resultBuilder.Append(',');
        resultBuilder.Append(model.Price?.ToString(CultureInfo.InvariantCulture));
        resultBuilder.Append(',');
        resultBuilder.Append(model.OldPrice?.ToString(CultureInfo.InvariantCulture));
        resultBuilder.Append(',');
        resultBuilder.Append(PrepareString(model.Url));
        resultBuilder.Append(',');
        resultBuilder.Append(PrepareArray(model.Images));

        return resultBuilder.ToString();
    }
}