using AngleSharp;
using ToyParser;
using ToyParser.Tools.CsvExporter;

var configuration = Configuration.Default;
var loader = new PageLoader(configuration);
var categoryParser = new CategoryParser(loader);

var products = await categoryParser.Parse("https://www.toy.ru/catalog/boy_transport/");
var productConverter = new ProductCvsConverter();
var exporter = new CsvExporter<Product>(productConverter);

await using var fileStream = File.Create("products.csv");
await exporter.Export(products, fileStream);