using AngleSharp;
using ToyParser;
using ToyParser.Models;
using ToyParser.Tools.CsvExporter;

var configuration = Configuration.Default.WithLocaleBasedEncoding();
var loader = new PageLoader(configuration);
var categoryParser = new CategoryParser(loader);

var products = await categoryParser.Parse("https://www.toy.ru/catalog/boy_transport/", Region.SaintPetersburg);
products.AddRange(await categoryParser.Parse("https://www.toy.ru/catalog/boy_transport/", Region.RostovOnDon));

var productConverter = new ProductCvsConverter();
var exporter = new CsvExporter<Product>(productConverter);

await using var fileStream = File.Create("products.csv");
await exporter.Export(products, fileStream);