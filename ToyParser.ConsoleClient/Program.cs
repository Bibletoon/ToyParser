// See https://aka.ms/new-console-template for more information

using AngleSharp;
using ToyParser;

var configuration = Configuration.Default;
var loader = new PageLoader(new HttpClient(), configuration);
var categoryParser = new CategoryParser(loader);

var product = categoryParser.Parse("https://www.toy.ru/catalog/boy_transport/");